using Harmonix.Domain.Common;

namespace Harmonix.Domain.Companies;

public static class CompanyErrors
{
    public static DomainError InvalidName => new("company.name.invalid", "Nome inválido");
    public static DomainError InvalidExpirationDate => new("company.expiration-date.invalid", "Data de expiração inválida");
    public static DomainError Expired => new("company.expired", "Acesso expirado");
    public static DomainError AliasAlreadyExists => new("company.alias-exists", "Empresa já existe", ErrorType.Conflict);
    public static DomainError Inactive => new("company.is-inactive", "Empresa inativa");
    public static DomainError InvalidAlias => new("company.alias.invalid", "Alias inválido");
}
