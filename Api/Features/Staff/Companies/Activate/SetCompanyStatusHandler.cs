using Harmonix.Application.Common;
using Harmonix.Application.Common.Errors;
using Harmonix.Application.Common.Results;
using Harmonix.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Harmonix.Api.Features.Staff.Companies.Activate;

public sealed class SetCompanyStatusHandler : BaseHandler<SetCompanyStatusRequest, bool>
{
    private readonly HarmonixDbContext _context;

    public SetCompanyStatusHandler(HarmonixDbContext context) : base()
    {
        _context = context;
    }

    protected override async Task<Result<bool>> HandleAsync(SetCompanyStatusRequest request, CancellationToken ct)
    {
        var company = await _context.Companies
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(c => c.Id == request.CompanyId && !c.Removed);

        if (company is null)
            return Result<bool>.Fail(CommonError.NotFound);

        if (request.IsActive) company.Activate();
        else company.Deactivate();

        await _context.SaveChangesAsync();

        return Result<bool>.Success(true);
    }
}

public record SetCompanyStatusRequest(Guid CompanyId, bool IsActive);
