using FluentValidation;
using Harmonix.Application.Common;
using Harmonix.Application.Common.Errors;
using Harmonix.Application.Common.Results;
using Harmonix.Domain.Common.Services;
using Harmonix.Domain.Common.ValueObjects;
using Harmonix.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Harmonix.Api.Features.Staff.Users.Update;

public class UpdateUserHandler : BaseHandler<UpdateUserRequest, UpdateUserResponse>
{
    private readonly HarmonixDbContext _context;
    private readonly IEmailUniqueChecker _emailChecker;

    public UpdateUserHandler(
        HarmonixDbContext context,
        IEmailUniqueChecker emailChecker,
        IValidator<UpdateUserRequest> validator)
        : base(validator)
    {
        _context = context;
        _emailChecker = emailChecker;
    }

    protected override async Task<Result<UpdateUserResponse>> HandleAsync(UpdateUserRequest request, CancellationToken ct)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.Id && !u.Removed, ct);

        if (user is null)
            return Result<UpdateUserResponse>.Fail(CommonError.NotFound);

        if (request.Email is not null)
        {
            var email = Email.Create(request.Email);

            var isUnique = await _emailChecker
                .IsUniqueAsync(email);

            if (!isUnique)
                return Result<UpdateUserResponse>.Fail(CommonError.EmailAlreadyExists);
        }

        user.Update(request.Name, request.Email, request.Role);

        await _context.SaveChangesAsync(ct);

        var response = new UpdateUserResponse(user.Id, user.Name);

        return Result<UpdateUserResponse>.Success(response);
    }
}
