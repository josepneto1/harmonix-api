using Harmonix.Domain.Common.Errors.Enums;

namespace Harmonix.Domain.Common.Errors;

public static class CommonErrors
{
    public static Error InvalidName => new("common.name.invalid", "Nome inválido");
    public static Error InvalidEmail => new("common.email.invalid", "Email inválido");
    public static Error InvalidRole => new("common.role.invalid", "Papel inválido");
    public static Error InvalidParameters => new("common.parameters.invalid", "Dados inválidos");
    public static Error NotFound => new("common.not-found", "Não encontrado", ErrorType.NotFound);
    public static Error InternalError => new("internal.error", "Erro interno", ErrorType.InternalError);
    public static Error BadRequest(string message) => new("bad.request", message, ErrorType.BadRequest);
    public static Error EmailAlreadyExists => new("common.email-exists", "Este email já existe", ErrorType.Conflict);

}
