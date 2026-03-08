using Harmonix.Application.Common;
using Harmonix.Application.Common.Results;
using Harmonix.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Harmonix.Api.Features.Staff.Users.List;

public class ListUsersHandler : BaseHandler<List<ListUsersResponse>>
{
    private readonly HarmonixDbContext _context;

    public ListUsersHandler(HarmonixDbContext context)
    {
        _context = context;
    }

    public override async Task<Result<List<ListUsersResponse>>> ExecuteAsync(CancellationToken ct)
    {
        var data = await _context.Users
            .AsNoTracking()
            .Where(u => !u.Removed)
            .Select(u => new ListUsersResponse(
                u.Id,
                u.Company.Name,
                u.Name,
                u.Email.Value
            ))
            .ToListAsync(ct);

        return Result<List<ListUsersResponse>>.Success(data);
    }
}

public record ListUsersResponse(
    Guid Id,
    string CompanyName,
    string Name,
    string Email
);
