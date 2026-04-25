using Harmonix.Domain.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Harmonix.Infrastructure.Data.DbConfig;

public class RefreshTokenDbConfig : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("refresh_tokens");

        builder.HasKey(rt => rt.Id);

        builder.Property(rt => rt.Id).HasColumnName("id").HasColumnType("uniqueidentifier");
        builder.Property(rt => rt.UserId).HasColumnName("user_id").HasColumnType("uniqueidentifier");
        builder.Property(rt => rt.CompanyId).HasColumnName("company_id").HasColumnType("uniqueidentifier");
        builder.Property(rt => rt.Token).HasColumnName("token").IsRequired().HasMaxLength(256);
        builder.Property(rt => rt.ExpiresAt).HasColumnName("expires_at").HasColumnType("datetimeoffset").IsRequired();
        builder.Property(rt => rt.RevokedAt).HasColumnName("revoked_at").HasColumnType("datetimeoffset").IsRequired(false);
        builder.Property(rt => rt.CreatedAt).HasColumnName("created_at").HasColumnType("datetimeoffset").IsRequired();

        builder.HasIndex(rt => new { rt.Token, rt.CompanyId })
            .IsUnique()
            .HasDatabaseName("idx_refresh_tokens_token_company");
        builder.HasIndex(rt => rt.UserId).HasDatabaseName("idx_refresh_tokens_user_id");
        builder.HasIndex(rt => rt.CompanyId).HasDatabaseName("idx_refresh_tokens_company_id");
        builder.HasIndex(rt => rt.ExpiresAt).HasDatabaseName("idx_refresh_tokens_expires_at");

        builder.HasOne(rt => rt.User).WithMany(u => u.RefreshTokens).HasForeignKey(rt => rt.UserId);

        builder.HasQueryFilter(rt => !rt.Removed && rt.RevokedAt == null && rt.ExpiresAt > DateTimeOffset.UtcNow);
    }
}
