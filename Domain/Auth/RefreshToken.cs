using Harmonix.Domain.Common;
using Harmonix.Domain.Users;

namespace Harmonix.Domain.Auth;

public class RefreshToken : BaseEntity
{
    public Guid UserId { get; private set; }
    public Guid CompanyId { get; set; }
    public string Token { get; private set; } = null!;
    public DateTime ExpiresAt { get; private set; }
    public DateTime? RevokedAt { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    public User User { get; private set; } = null!;

    protected RefreshToken() { }

    public RefreshToken(Guid userId, Guid companyId, string token, DateTime expiresAt, string? deviceInfo = null)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        CompanyId = companyId;
        Token = token;
        ExpiresAt = expiresAt;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public bool IsExpired => DateTime.UtcNow > ExpiresAt;
    public bool IsRevoked => RevokedAt.HasValue;
    public bool IsValid => !IsExpired && !IsRevoked;

    public void Revoke()
    {
        RevokedAt = DateTime.UtcNow;
        Remove();
    }
}