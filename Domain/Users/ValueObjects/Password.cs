using Harmonix.Domain.Common;
using Harmonix.Domain.Common.Errors;
using Harmonix.Domain.Common.Validations;

namespace Harmonix.Domain.Users.ValueObjects;

public sealed record Password : IValueObject<Password, string>
{
    public const int MinLength = 8;
    public const int MaxLength = 20;
    public string Value { get; }

    public Password(string value)
    {
        Value = value;
    }

    public static Result<Password> Create(string password)
    {
        if (!Validate.IsValidText(password, MinLength, MaxLength))
            return Result<Password>.Fail(CommonErrors.InvalidParameters);

        return Result<Password>.Success(new Password(password));
    }

    public static Password FromDbConfig(string value) => new(value);
}
