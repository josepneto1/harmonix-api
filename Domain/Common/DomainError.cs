namespace Harmonix.Domain.Common;

public sealed record DomainError(string Code, string Message, ErrorType Type = ErrorType.Failure)
{
    public static readonly DomainError None = new(string.Empty, string.Empty, ErrorType.None);
}
