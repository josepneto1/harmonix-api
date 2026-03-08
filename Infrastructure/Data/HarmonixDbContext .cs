using Harmonix.Domain.Auth;
using Harmonix.Domain.Companies;
using Harmonix.Domain.Users;
using Harmonix.Infrastructure.Data.DbConfig;
using Microsoft.EntityFrameworkCore;

namespace Harmonix.Infrastructure.Data;

public class HarmonixDbContext : DbContext
{
    public HarmonixDbContext(DbContextOptions<HarmonixDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserDbConfig());
        modelBuilder.ApplyConfiguration(new CompanyDbConfig());
        modelBuilder.ApplyConfiguration(new RefreshTokenDbConfig());

        modelBuilder.Entity<Company>()
            .HasQueryFilter(c => c.IsActive);

        base.OnModelCreating(modelBuilder);
    }
}
