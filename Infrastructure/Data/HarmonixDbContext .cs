using Harmonix.Domain.Auth;
using Harmonix.Domain.Common;
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

        modelBuilder.Entity<Company>().HasQueryFilter(c => c.IsActive);
        
        modelBuilder.Entity<User>().HasQueryFilter(u => !u.Company.Removed);

        base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(ct);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
                entry.Entity.SetCreated();

            if (entry.State == EntityState.Modified)
                entry.Entity.SetUpdated();
        }
    }
}
