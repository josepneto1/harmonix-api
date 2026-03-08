using Harmonix.Application.Common.Errors.Enums;
using Harmonix.Domain.Common;

namespace Harmonix.Application.Common.Errors;

public class ErrorMapper
{
    public static Error FromDomain(DomainError domainError)
    {
        var status = domainError.Type switch
        {
            ErrorType.Validation => ErrorStatus.BadRequest,
            ErrorType.Conflict => ErrorStatus.Conflict,
            ErrorType.NotFound => ErrorStatus.NotFound,
            _ => ErrorStatus.InternalError
        };

        return new Error(
            domainError.Code,
            domainError.Message,
            status
        );
    }
}
