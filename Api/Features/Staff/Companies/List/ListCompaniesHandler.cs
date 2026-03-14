using Harmonix.Application.Common;
using Harmonix.Application.Common.Results;
using Harmonix.Domain.Companies;
using Harmonix.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Harmonix.Api.Features.Staff.Companies.List;

public class ListCompaniesHandler : BaseHandler<ListCompaniesRequest, ListCompaniesResponse>
{
    private readonly HarmonixDbContext _context;

    public ListCompaniesHandler(HarmonixDbContext context)
    {
        _context = context;
    }

    protected override async Task<Result<ListCompaniesResponse>> HandleAsync(ListCompaniesRequest request, CancellationToken ct)
    {
        var page = request.NormalizedPage;
        var pageSize = request.NormalizedPageSize;

        var query = _context.Companies.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim().ToLower();

            query = query.Where(c => c.Name.ToLower().Contains(search));
        }

        query = ApplySorting(query, request.SortBy, request.SortDirection);

        var totalCount = await query.CountAsync(ct);

        var companies = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new CompanyView(
                c.Id,
                c.Name,
                c.Alias.Value,
                c.CreatedAt,
                c.ExpirationDate
            ))
            .ToListAsync(ct);

        var response = new ListCompaniesResponse(
            companies,
            page,
            pageSize,
            totalCount);

        return Result<ListCompaniesResponse>.Success(response);
    }

    private static IQueryable<Company> ApplySorting(IQueryable<Company> query, string? sortBy, string? sortDirection)
    {
        var isDesc = string.Equals(sortDirection, "desc", StringComparison.OrdinalIgnoreCase);

        return (sortBy ?? string.Empty).Trim().ToLowerInvariant() switch
        {
            "name" => isDesc
                ? query.OrderByDescending(c => c.Name)
                : query.OrderBy(c => c.Name),

            "alias" => isDesc
                ? query.OrderByDescending(c => c.Alias)
                : query.OrderBy(c => c.Alias),

            "createdat" => isDesc
                ? query.OrderByDescending(c => c.CreatedAt)
                : query.OrderBy(c => c.CreatedAt),

            "expirationdate" => isDesc
                ? query.OrderByDescending(c => c.ExpirationDate)
                : query.OrderBy(c => c.ExpirationDate),

            _ => isDesc
                ? query.OrderByDescending(c => c.CreatedAt)
                : query.OrderBy(c => c.CreatedAt)
        };
    }
}

public record ListCompaniesRequest(
    int Page,
    int PageSize,
    string? SortBy,
    string? SortDirection,
    string? Search = null)
{
    public int NormalizedPage => Page <= 0 ? 1 : Page;
    public int NormalizedPageSize => Math.Clamp(PageSize <= 0 ? 25 : PageSize, 1, 100);
}

public record ListCompaniesResponse(
    List<CompanyView> Data,
    int Page,
    int PageSize,
    int TotalCount)
{
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}

public record CompanyView(
    Guid Id,
    string Name,
    string Alias,
    DateTimeOffset CreatedAt,
    DateTimeOffset ExpirationDate
);
