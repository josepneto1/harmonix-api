using FluentValidation;
using Harmonix.Application.Common;
using Harmonix.Application.Common.Errors;
using Harmonix.Application.Common.Results;
using Harmonix.Domain.Common.Services;
using Harmonix.Domain.Common.ValueObjects;
using Harmonix.Domain.Users;
using Harmonix.Domain.Users.ValueObjects;
using Harmonix.Infrastructure.Auth;
using Harmonix.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Harmonix.Api.Features.Staff.Users.Create;

public class CreateUserHandler : BaseHandler<CreateUserRequest, CreateUserResponse>
{
    private readonly HarmonixDbContext _context;
    private readonly IEmailUniqueChecker _emailChecker;
    private readonly PasswordHasher _passwordHasher;

    public CreateUserHandler(
        HarmonixDbContext context,
        IEmailUniqueChecker emailChecker,
        PasswordHasher passwordHasher,
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
            .FirstOrDefaultAsync(c => c.Id == request.CompanyId && !c.Removed, ct);

        if (company is null)
            return Result<CreateUserResponse>.Fail(CommonError.NotFound);

        var email = Email.Create(request.Email);

        var isUnique = await _emailChecker.IsUniqueAsync(email, ct);

        if (!isUnique)
            return Result<CreateUserResponse>.Fail(CommonError.EmailAlreadyExists);

        var password = Password.Create(request.Password);
        var passwordHash = _passwordHasher.HashPassword(password.Value);

        var user = new User(
            request.CompanyId,
            request.Name,
            request.Email,
            passwordHash,
            request.Role
        );

        _context.Users.Add(user);
        await _context.SaveChangesAsync(ct);

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
