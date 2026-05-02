using Harmonix.Domain.Companies;
using Harmonix.Domain.Users;
using Harmonix.Domain.Users.Enums;
using Harmonix.Infrastructure.Auth;
using Microsoft.EntityFrameworkCore;

namespace Harmonix.Infrastructure.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<HarmonixDbContext>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

        var companyExists = await dbContext.Companies
            .IgnoreQueryFilters()
            .AnyAsync(c => c.Alias.Value == "sysadmin");

        if (companyExists)
            return;

        var companyResult = Company.Create("SysAdmin", "sysadmin", DateTimeOffset.MaxValue);
        if (companyResult.IsFailure)
            return;

        var company = companyResult.Data;

        var userResult = User.Create(company.Id, "SysAdmin", "admin@admin", Role.SysAdmin);
        if (userResult.IsFailure)
            return;

        var user = userResult.Data;
        user.SetPasswordHash(passwordHasher.HashPassword("admin"));

        dbContext.Companies.Add(company);
        dbContext.Users.Add(user);

        await dbContext.SaveChangesAsync();
    }
}
