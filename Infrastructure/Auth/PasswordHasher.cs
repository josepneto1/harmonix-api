namespace Harmonix.Infrastructure.Auth;

public class PasswordHasher
{
    private readonly string _fakeHash;

    public PasswordHasher()
    {
        _fakeHash = HashPassword("f@k3paSSw0rd123!");
    }

    public string FakeHash => _fakeHash;
    public string HashPassword(string password)
    {
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
        return passwordHash;
    }

    public bool VerifyPassword(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}
