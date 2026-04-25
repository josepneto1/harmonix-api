using Harmonix.Domain.Common.Errors;
using Harmonix.Domain.Common.Validations;
using System.Net.Mail;

namespace Harmonix.Domain.Common.ValueObjects;

public sealed record Email : IValueObject<Email, string>
{
    private const int MaxLength = 255;
    public string Value { get; }

    private Email(string value) => Value = value;

    public static Result<Email> Create(string email)
    {
        email = email.Trim();

        if (!Validate.IsValidText(email, minLength: null, MaxLength)|| !IsValid(email))
            return Result<Email>.Fail(CommonErrors.InvalidEmail);

        return Result<Email>.Success(new Email(email));
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

    public static Email FromDbConfig(string value) => new(value);
}
