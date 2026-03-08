using FluentValidation.Results;
using Harmonix.Application.Common.Errors;

namespace Harmonix.Application.Common.Extensions;

public static class ValidationExtensions
{
    public static Error ToValidationError(this ValidationResult validationResult)
    {
        var details = validationResult.Errors
            .Select(failure => new ValidationError(failure.PropertyName, failure.ErrorMessage))
            .ToList();

        return ValidationError.Validation(details);
    }
}
