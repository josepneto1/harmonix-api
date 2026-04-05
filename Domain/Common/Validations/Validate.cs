namespace Harmonix.Domain.Common.Validations;

public static class Validate
{
    public static bool IsValidText(string text, int? minLength = null, int? maxLength = null)
    {
        if (string.IsNullOrWhiteSpace(text))
            return false;

        if (minLength is < 0 || maxLength is < 0)
            return false;

        if (minLength is not null && maxLength is not null && minLength > maxLength)
            return false;

        if (minLength is not null && text.Length < minLength)
            return false;

        if (maxLength is not null && text.Length > maxLength)
            return false;

        return true;
    }
}
