using Harmonix.Application.Common;
using Harmonix.Domain.Common.Errors;
using Harmonix.Domain.Common;
using Harmonix.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Harmonix.Api.Features.Staff.Companies.Get;

public class GetCompanyByIdHandler : BaseHandler<Guid, GetCompanyByIdResponse>
{
    private readonly HarmonixDbContext _context;

    public GetCompanyByIdHandler(HarmonixDbContext context)
    {
        _context = context;
    }

    protected override async Task<Result<GetCompanyByIdResponse>> HandleAsync(Guid id, CancellationToken ct)
    {
        var company = await _context.Companies
            .AsNoTracking()
            .IgnoreQueryFilters()
            .Where(c => c.Id == id)
            .Select(c => new GetCompanyByIdResponse(
                c.Id,
                c.Name,
                c.Alias.Value,
                c.IsActive,
                c.CreatedAt,
                c.ExpirationDate
            ))
            .FirstOrDefaultAsync(ct);

        if (company is null)
            return Result<GetCompanyByIdResponse>.Fail(CommonErrors.NotFound);

        return Result<GetCompanyByIdResponse>.Success(company);
    }
}
public record GetCompanyByIdResponse(
    Guid Id,
    string Name,
    string Alias,
    bool IsActive,
    DateTimeOffset CreatedAt,
    DateTimeOffset ExpirationDate
);
