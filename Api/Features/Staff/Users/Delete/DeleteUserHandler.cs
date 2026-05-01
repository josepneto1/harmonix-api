using Harmonix.Domain.Common.Errors;
using Harmonix.Domain.Common;
using Harmonix.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Harmonix.Common;

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
            .FirstOrDefaultAsync(c => c.Id == id && !c.Removed);

        if (user is null)
            return Result<bool>.Fail(CommonErrors.NotFound);

        user.Remove();

        await _context.SaveChangesAsync();

        return Result<bool>.Success(true);
    }
}
