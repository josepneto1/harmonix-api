namespace Harmonix.Infrastructure.Auth;

public class BcryptPasswordHasher : IPasswordHasher
{
    private readonly string _fakeHash;

    public BcryptPasswordHasher()
    {
        _fakeHash = HashPassword("f@k3paSSw0rd123!");
    }

    public string FakeHash => _fakeHash;

    public string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);

    public bool VerifyPassword(string password, string passwordHash) => BCrypt.Net.BCrypt.Verify(password, passwordHash);
}
