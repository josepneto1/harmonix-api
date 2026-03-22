using Harmonix.Application.Common;
using Harmonix.Application.Common.Errors;
using Harmonix.Application.Common.Results;
using Harmonix.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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
            return Result<bool>.Fail(CommonError.NotFound);

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
