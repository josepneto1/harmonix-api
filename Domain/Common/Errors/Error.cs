using Harmonix.Domain.Common.Errors.Enums;

namespace Harmonix.Domain.Common.Errors;

public sealed record Error(string Code, string Message, ErrorType Type = ErrorType.Failure)
{
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.None);
}
