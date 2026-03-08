using System.Net.Mail;

namespace Harmonix.Domain.Common.ValueObjects;

public sealed record Email
{
    public const int MaxLength = 255;
    public string Value { get; }

    public Email(string value)
    {
        Value = value;
    }

    public static Email Create(string email)
    {
        //if (string.IsNullOrWhiteSpace(email))
            //throw UserDomainException.InvalidEmail();

        email = email.Trim();

        //if (email.Length > MaxLength || !IsValid(email))
            //throw UserDomainException.InvalidEmail();

        return new Email(email);
    }

    private static bool IsValid(string email)
    {
        try
        {
            var emailAddress = new MailAddress(email);
            return emailAddress.Address == email;
        }
        catch
        {
            return false;
        }
    }
}
