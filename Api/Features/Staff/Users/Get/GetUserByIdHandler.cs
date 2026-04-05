using Harmonix.Application.Common;
using Harmonix.Domain.Common.Errors;
using Harmonix.Domain.Common;
using Harmonix.Domain.Users.Enums;
using Harmonix.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Harmonix.Api.Features.Staff.Users.Get;

public class GetUserByIdHandler : BaseHandler<Guid, GetUserByIdResponse>
{
    private readonly HarmonixDbContext _context;

    public GetUserByIdHandler(HarmonixDbContext context)
    {
        _context = context;
    }

    protected override async Task<Result<GetUserByIdResponse>> HandleAsync(Guid id, CancellationToken ct)
    {
        var user = await _context.Users
            .AsNoTracking()
            .Where(u => u.Id == id)
            .Select(u => new GetUserByIdResponse(
                u.Id,
                u.Company.Name,
                u.Name,
                u.Email.Value,
                u.Role,
                u.CreatedAt
            ))
            .FirstOrDefaultAsync(ct);

        if (user is null)
            return Result<GetUserByIdResponse>.Fail(CommonErrors.NotFound);

        return Result<GetUserByIdResponse>.Success(user);
    }
}

public record GetUserByIdResponse(
    Guid Id,
    string CompanyName,
    string Name,
    string Email,
    Role Role,
    DateTimeOffset CreatedAt
);
