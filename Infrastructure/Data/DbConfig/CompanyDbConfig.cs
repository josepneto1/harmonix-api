using Harmonix.Domain.Companies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Harmonix.Infrastructure.Data.DbConfig;

public class CompanyDbConfig : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("companies");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasColumnName("id").HasColumnType("uniqueidentifier");
        builder.Property(c => c.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
        builder.Property(c => c.CreatedAt).HasColumnName("created_at").HasColumnType("datetimeoffset").IsRequired();
        builder.Property(c => c.ExpirationDate).HasColumnName("expiration_date").HasColumnType("datetimeoffset").IsRequired();
        builder.Property(c => c.UpdatedAt).HasColumnName("updated_at").HasColumnType("datetimeoffset");
        builder.Property(c => c.Removed).HasColumnName("removed").HasColumnType("bit").HasDefaultValue(false);
        builder.Property(c => c.IsActive).HasColumnName("is_active").HasColumnType("bit").HasDefaultValue(true);

        builder.OwnsOne(c => c.Alias, a =>
        {
            a.Property(x => x.Value).HasColumnName("alias").HasMaxLength(30).IsRequired();
            a.HasIndex(x => x.Value).IsUnique().HasDatabaseName("idx_companies_alias");
        });

        builder.HasMany(c => c.Users).WithOne(u => u.Company).HasForeignKey(u => u.CompanyId);
    }
}
