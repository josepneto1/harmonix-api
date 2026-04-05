using Harmonix.Domain.Common.Errors;
using Harmonix.Domain.Common.Errors.Enums;

namespace Harmonix.Domain.Companies;

public static class CompanyErrors
{
    #region Failure 
    public static Error InvalidExpirationDate => new("company.expiration-date.invalid", "Data de expiração inválida");
    public static Error InvalidAlias => new("company.alias.invalid", "Alias inválido");
    #endregion

    #region BadRequest
    public static Error Expired => new("company.expired", "Acesso expirado", ErrorType.BadRequest);
    public static Error Inactive => new("company.is-inactive", "Empresa inativa", ErrorType.BadRequest);
    #endregion

    #region Conflict
    public static Error AliasAlreadyExists => new("company.alias-exists", "Empresa já existe", ErrorType.Conflict);
    #endregion
}
