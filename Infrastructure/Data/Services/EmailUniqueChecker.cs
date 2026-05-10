using Harmonix.Domain.Common.Services;
using Harmonix.Domain.Common.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Harmonix.Infrastructure.Data.Services;

public sealed class EmailUniqueChecker : IEmailUniqueChecker
{
    private readonly HarmonixDbContext _context;
    public EmailUniqueChecker(HarmonixDbContext context)
    {
        _context = context;
    }
    public async Task<bool> IsUniqueAsync(Email email)
        => !await _context.Users
            .AsNoTracking()
            .AnyAsync(u => u.Email.Value == email.Value);
}
