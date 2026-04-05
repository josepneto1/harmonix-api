using Harmonix.Domain.Common.Errors.Enums;

namespace Harmonix.Domain.Common.Errors;

public static class AuthErrors
{
    public static Error InvalidCredentials => new("auth.invalid-credentials", "Email ou senha inválido", ErrorType.Unauthorized);
    public static Error InvalidRefreshToken => new("auth.invalid-refresh-token", "Refresh token inválido ou expirado", ErrorType.Unauthorized);
}
