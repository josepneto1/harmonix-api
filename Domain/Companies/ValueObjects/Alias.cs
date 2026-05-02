using Harmonix.Domain.Common;

namespace Harmonix.Domain.Companies.ValueObjects;

public sealed record Alias
{
    private const int MinLength = 3;
    private const int MaxLength = 30;
    public string Value { get; }

    private Alias(string value) => Value = value;

    public static Result<Alias> Create(string alias)
    {
        if (string.IsNullOrWhiteSpace(alias))
            return Result<Alias>.Fail(CompanyErrors.InvalidAlias);

        var normalized = NormalizeAlias(alias);

        if (normalized.Length is < MinLength or > MaxLength)
            return Result<Alias>.Fail(CompanyErrors.InvalidAlias);

        return Result<Alias>.Success(new Alias(normalized));
    }

    private static string NormalizeAlias(string alias)
    {
        return alias
            .Trim()
            .Replace(" ", "")
            .ToLower();
    }
}
