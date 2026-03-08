using Harmonix.Application.Common.Errors.Enums;

namespace Harmonix.Application.Common.Errors;

public sealed record Error(string Code, string Message, ErrorStatus Status, object? Details = null)
{
    public static readonly Error None = new(string.Empty, string.Empty, ErrorStatus.None);
}
