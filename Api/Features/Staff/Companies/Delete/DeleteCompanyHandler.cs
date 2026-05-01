using Harmonix.Domain.Common.Errors;
using Harmonix.Domain.Common;
using Harmonix.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Harmonix.Common;

namespace Harmonix.Api.Features.Staff.Companies.Delete;

public class DeleteCompanyHandler : BaseHandler<Guid, bool>
{
    private readonly HarmonixDbContext _context;

    public DeleteCompanyHandler(HarmonixDbContext context)
    {
        _context = context;
    }

    protected override async Task<Result<bool>> HandleAsync(Guid id, CancellationToken ct)
    {
        var company = await _context.Companies
            .FirstOrDefaultAsync(c => c.Id == id && !c.Removed);

        if (company is null)
            return Result<bool>.Fail(CommonErrors.NotFound);

        company.Deactivate();
        company.Remove();

        var users = await _context.Users
            .Where(u => u.CompanyId == company.Id && !u.Removed)
            .ToListAsync();

        foreach (var user in users)
        {
            user.Remove();
        }

        await _context.SaveChangesAsync();

        return Result<bool>.Success(true);
    }
}
