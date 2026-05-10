using Harmonix.Common;
using Harmonix.Domain.Common;
using Harmonix.Domain.Users;
using Harmonix.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Harmonix.Api.Features.Staff.Users.List;

public class ListUsersHandler : BaseHandler<ListUsersRequest, ListUsersResponse>
{
    private readonly HarmonixDbContext _context;

    public ListUsersHandler(HarmonixDbContext context)
    {
        _context = context;
    }

    protected override async Task<Result<ListUsersResponse>> HandleAsync(ListUsersRequest request, CancellationToken ct)
    {
        var page = request.NormalizedPage;
        var pageSize = request.NormalizedPageSize;

        var query = _context.Users.AsNoTracking().IgnoreQueryFilters();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim().ToLower();

            query = query.Where(u => 
                u.Name.ToLower().Contains(search) ||
                u.Email.Value.Contains(search));
        }

        query = ApplySorting(query, request.SortBy, request.SortDirection);

        var totalCount = await query.CountAsync(ct);

        var users = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new UserView(
                u.Id,
                u.Company.Name,
                u.Name,
                u.Email.Value,
                u.Role.ToString(),
                u.CreatedAt
            ))
            .ToListAsync(ct);

        var response = new ListUsersResponse(
            users,
            page,
            pageSize,
            totalCount);

        return Result<ListUsersResponse>.Success(response);
    }

    private static IQueryable<User> ApplySorting(IQueryable<User> query, string? sortBy, string? sortDirection)
    {
        var isDesc = string.Equals(sortDirection, "desc", StringComparison.OrdinalIgnoreCase);

        sortBy = (sortBy ?? string.Empty).Trim().ToLowerInvariant();

        return sortBy switch
        {
            "companyName" => isDesc
                ? query.OrderByDescending(c => c.Company.Name)
                : query.OrderBy(c => c.Company.Name),

            "name" => isDesc
                ? query.OrderByDescending(c => c.Name)
                : query.OrderBy(c => c.Name),

            "email" => isDesc
                ? query.OrderByDescending(c => c.Email.Value)
                : query.OrderBy(c => c.Email.Value),

            "role" => isDesc
                ? query.OrderByDescending(c => c.Role)
                : query.OrderBy(c => c.Role),

            "createdAt" => isDesc
                ? query.OrderByDescending(c => c.CreatedAt)
                : query.OrderBy(c => c.CreatedAt),

            _ => isDesc
                ? query.OrderByDescending(c => c.CreatedAt)
                : query.OrderBy(c => c.CreatedAt)
        };
    }
}

public record ListUsersRequest(
    int Page,
    int PageSize,
    string? SortBy,
    string? SortDirection,
    string? Search = null)
{
    public int NormalizedPage => Page <= 0 ? 1 : Page;
    public int NormalizedPageSize => Math.Clamp(PageSize <= 0 ? 25 : PageSize, 1, 100);
}

public record ListUsersResponse(
    List<UserView> Data,
    int Page,
    int PageSize,
    int TotalCount
);

public record UserView(
    Guid Id,
    string CompanyName,
    string Name,
    string Email,
    string Role,
    DateTimeOffset CreatedAt
);
