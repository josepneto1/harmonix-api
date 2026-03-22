using Harmonix.Domain.Common;
using Harmonix.Domain.Companies.ValueObjects;
using Harmonix.Domain.Users;

namespace Harmonix.Domain.Companies;

public class Company : BaseEntity
{
    public string Name { get; private set; } = null!;
    public Alias Alias { get; private set; } = null!;
    public DateTimeOffset ExpirationDate { get; private set; }
    public bool IsActive { get; private set; }

    public List<User> Users { get; private set; } = new List<User>();

    protected Company() { }

    private Company(string name, Alias alias, DateTimeOffset expirationDate)
    {
        Id = GenerateNewId();
        Name = name;
        Alias = alias;
        ExpirationDate = expirationDate;
        IsActive = true;
    }

    public static Result<Company> Create(string name, string alias, DateTimeOffset expirationDate)
    {
        name = name.Trim();

        if (!IsValidName(name))
            return Result<Company>.Fail(CompanyErrors.InvalidName);

        if (!IsValidExpirationDate(expirationDate))
            return Result<Company>.Fail(CompanyErrors.InvalidExpirationDate);

        var aliasResult = Alias.Create(alias);
        if (aliasResult.IsFailure)
            return Result<Company>.Fail(aliasResult.Error);

        var company = new Company(name, aliasResult.Value!, expirationDate);
        return Result<Company>.Success(company);
    }

    public Result Update(string? name, Alias? alias, DateTimeOffset? expirationDate)
    {
        name = name?.Trim();

        if (name is not null)
        {
            if (!IsValidName(name))
                return Result.Fail(CompanyErrors.InvalidName);

            Name = name;
        }

        if (alias is not null)
            Alias = alias;

        if (expirationDate is not null)
        {
            if (!IsValidExpirationDate(expirationDate))
                return Result.Fail(CompanyErrors.InvalidExpirationDate);

            ExpirationDate = expirationDate.Value;
        }

        return Result.Success();
    }

    public void Deactivate() => IsActive = false;

    public void Activate() => IsActive = true;

    private static bool IsValidName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return false;

        return name.Length is >= 3 and <= 100;
    }

    private static bool IsValidExpirationDate(DateTimeOffset? expirationDate) => expirationDate > DateTimeOffset.UtcNow;
}
