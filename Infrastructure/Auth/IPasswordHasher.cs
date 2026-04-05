namespace Harmonix.Infrastructure.Auth;

public interface IPasswordHasher
{
    string FakeHash { get; }
    string HashPassword(string password);
    bool VerifyPassword(string password, string passwordHash);
}
