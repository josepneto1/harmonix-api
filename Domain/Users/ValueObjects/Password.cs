namespace Harmonix.Domain.Users.ValueObjects;

public sealed record Password
{
    public const int MinLength = 8;
    public const int MaxLength = 20;
    public string Value { get; }

    public Password(string value)
    {
        Value = value;
    }

    public static Password Create(string password)
    {
        //if (string.IsNullOrEmpty(password))
        //    throw UserDomainException.InvalidPassword();

        password = password.Trim();

        //if (password.Length is < MinLength or > MaxLength)
        //    throw UserDomainException.InvalidPassword();

        return new Password(password);
    }
}
