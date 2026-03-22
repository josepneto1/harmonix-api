using Harmonix.Domain.Common.Services;
using Harmonix.Domain.Common.ValueObjects;
using Harmonix.Infrastructure.Data;
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
    {
        return !await _context.Users
            .AsNoTracking()
            .AnyAsync(u => u.Email == email);
    }
}
