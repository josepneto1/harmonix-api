using FluentValidation;
using Harmonix.Application.Common;
using Harmonix.Domain.Common.Errors;
using Harmonix.Domain.Common;
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
            .FirstOrDefaultAsync(u => u.Id == request.Id && !u.Removed);

        if (user is null)
            return Result<UpdateUserResponse>.Fail(CommonErrors.NotFound);

        if (request.Email is not null)
        {
            var emailResult = Email.Create(request.Email);

            if (emailResult.IsFailure)
                return Result<UpdateUserResponse>.Fail(emailResult.Error);

            if (!user.Email.Value.Equals(emailResult.Data.Value)) 
            {
                var isUnique = await _emailChecker.IsUniqueAsync(emailResult.Data);

                if (!isUnique)
                    return Result<UpdateUserResponse>.Fail(CommonErrors.EmailAlreadyExists);
            }
        }

        user.Update(request.Name, request.Email, request.Role);

        await _context.SaveChangesAsync();

        var response = new UpdateUserResponse(user.Id, user.Name);

        return Result<UpdateUserResponse>.Success(response);
    }
}
