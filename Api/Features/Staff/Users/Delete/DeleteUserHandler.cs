using Harmonix.Application.Common;
using Harmonix.Domain.Common.Errors;
using Harmonix.Domain.Common;
using Harmonix.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Harmonix.Api.Features.Staff.Users.Delete;

public class DeleteUserHandler : BaseHandler<Guid, bool>
{
    private readonly HarmonixDbContext _context;

    public DeleteUserHandler(HarmonixDbContext context)
    {
        _context = context;
    }
    protected override async Task<Result<bool>> HandleAsync(Guid id, CancellationToken ct)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(c => c.Id == id && !c.Removed, ct);

        if (user is null)
            return Result<bool>.Fail(CommonErrors.NotFound);

        user.Remove();

        await _context.SaveChangesAsync(ct);

        return Result<bool>.Success(true);
    }
}
