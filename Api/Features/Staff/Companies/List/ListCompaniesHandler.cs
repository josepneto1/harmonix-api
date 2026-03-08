using Harmonix.Application.Common;
using Harmonix.Application.Common.Results;
using Harmonix.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Harmonix.Api.Features.Staff.Companies.List;

public class ListCompaniesHandler : BaseHandler<List<ListCompaniesResponse>>
{
    private readonly HarmonixDbContext _context;

    public ListCompaniesHandler(HarmonixDbContext context)
    {
        _context = context;
    }

    public override async Task<Result<List<ListCompaniesResponse>>> ExecuteAsync(CancellationToken ct)
    {
        var companies = await _context.Companies
            .AsNoTracking()
            .Where(c => !c.Removed)
            .Select(c => new ListCompaniesResponse(
                c.Id,
                c.Name,
                c.Alias.Value,
                c.CreatedAt,
                c.ExpirationDate
            ))
            .ToListAsync(ct);

        return Result<List<ListCompaniesResponse>>.Success(companies);
    }
}

public record ListCompaniesResponse(
    Guid Id,
    string Name,
    string Alias,
    DateTimeOffset CreatedAt,
    DateTimeOffset ExpirationDate
);
