using FluentValidation.Results;
using Harmonix.Domain.Common.Errors;
using Harmonix.Domain.Common.Errors.Enums;

namespace Harmonix.Common;

public static class ValidationExtensions
{
    public static Error ToValidationError(this ValidationResult validation)
    {
        var errors = validation.Errors
            .Where(e => e is not null)
            .Select(e => $"{e.PropertyName}: {e.ErrorMessage}")
            .Distinct()
            .ToArray();

        var message = errors.Length == 0 ? "Dados inválidos" : string.Join("; ", errors);

        return new Error("validation.failed", message, ErrorType.Validation);
    }
}
