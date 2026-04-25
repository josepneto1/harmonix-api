using Harmonix.Domain.Common.ValueObjects;
using Harmonix.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Harmonix.Infrastructure.Data.DbConfig;

public class UserDbConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id).HasColumnName("id").HasColumnType("uniqueidentifier");
        builder.Property(u => u.CompanyId).HasColumnName("company_id").HasColumnType("uniqueidentifier");
        builder.Property(u => u.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
        builder.Property(u => u.Email)
            .HasColumnName("email")
            .HasConversion(email => email.Value, value => Email.FromDbConfig(value))
            .HasMaxLength(255)
            .IsRequired();
        builder.Property(u => u.PasswordHash).HasColumnName("password_hash").HasMaxLength(500).IsRequired();
        builder.Property(u => u.Role).HasColumnName("role").HasMaxLength(50).IsRequired().HasConversion<string>();
        builder.Property(u => u.CreatedAt).HasColumnName("created_at").HasColumnType("datetimeoffset").IsRequired();
        builder.Property(u => u.UpdatedAt).HasColumnName("updated_at").HasColumnType("datetimeoffset");
        builder.Property(u => u.Removed).HasColumnName("removed").HasColumnType("bit").HasDefaultValue(false);

        builder.HasIndex(u => u.Email).IsUnique().HasDatabaseName("idx_users_email");

        builder.HasOne(u => u.Company).WithMany(c => c.Users).HasForeignKey(u => u.CompanyId);

        builder.HasMany(u => u.RefreshTokens).WithOne(rt => rt.User).HasForeignKey(rt => rt.UserId);
    }
}
