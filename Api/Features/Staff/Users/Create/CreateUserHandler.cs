using FluentValidation;
using Harmonix.Common;
using Harmonix.Domain.Common;
using Harmonix.Domain.Common.Errors;
using Harmonix.Domain.Common.Services;
using Harmonix.Domain.Users;
using Harmonix.Domain.Users.Enums;
using Harmonix.Domain.Users.ValueObjects;
using Harmonix.Infrastructure.Auth;
using Harmonix.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Harmonix.Api.Features.Staff.Users.Create;

public class CreateUserHandler : BaseHandler<CreateUserRequest, CreateUserResponse>
{
    private readonly HarmonixDbContext _context;
    private readonly IEmailUniqueChecker _emailChecker;
    private readonly IPasswordHasher _passwordHasher;

    public CreateUserHandler(
        HarmonixDbContext context,
        IEmailUniqueChecker emailChecker,
        IPasswordHasher passwordHasher,
        IValidator<CreateUserRequest> validator)
        : base(validator)
    {
        _context = context;
        _emailChecker = emailChecker;
        _passwordHasher = passwordHasher;
    }

    protected override async Task<Result<CreateUserResponse>> HandleAsync(CreateUserRequest request, CancellationToken ct)
    {
        var company = await _context.Companies
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == request.CompanyId && !c.Removed);

        if (company is null)
            return Result<CreateUserResponse>.Fail(CommonErrors.NotFound);

        var userResult = User.Create(company.Id, request.Name, request.Email, request.Role);

        if (userResult.IsFailure)
            return Result<CreateUserResponse>.Fail(userResult.Error);

        var user = userResult.Data!;

        var isUnique = await _emailChecker.IsUniqueAsync(user.Email);
        if (!isUnique)
            return Result<CreateUserResponse>.Fail(CommonErrors.EmailAlreadyExists);

        var passwordResult = Password.Create(request.Password);
        if (passwordResult.IsFailure)
            return Result<CreateUserResponse>.Fail(passwordResult.Error);

        var passwordHash = _passwordHasher.HashPassword(passwordResult.Data!.Value);
        user.SetPasswordHash(passwordHash);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var response = new CreateUserResponse(
            user.Id,
            user.CompanyId,
            user.Name,
            user.Email.Value,
            user.Role,
            user.CreatedAt
        );

        return Result<CreateUserResponse>.Success(response);
    }
}

public record CreateUserRequest(
    Guid CompanyId,
    string Name,
    string Email,
    string Password,
    Role Role
);

public record CreateUserResponse(
    Guid Id,
    Guid CompanyId,
    string Name,
    string Email,
    Role Role,
    DateTimeOffset CreatedAt
);
