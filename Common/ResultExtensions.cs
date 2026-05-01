using Harmonix.Domain.Common;
using Harmonix.Domain.Common.Errors.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Harmonix.Common;

public static class ResultExtensions
{
    public static IActionResult GetResult<T>(this ControllerBase controller, Result<T> result)
    {
        if (result.IsSuccess)
        {
            var method = controller.HttpContext.Request.Method;
            return method switch
            {
                "POST" => controller.StatusCode(StatusCodes.Status201Created, result.Data),
                "DELETE" => controller.StatusCode(StatusCodes.Status204NoContent),
                "PUT" or "PATCH" => controller.StatusCode(StatusCodes.Status200OK, result.Data),
                "GET" => controller.StatusCode(StatusCodes.Status200OK, result.Data),
                _ => controller.StatusCode(StatusCodes.Status200OK, result.Data)
            };
        }

        var error = result.Error;
        return controller.StatusCode(
            MapToHttpStatusCode(error.Type),
            new
            {
                error = error.Code,
                message = error.Message,
            });
    }

    public static IActionResult GetResult(this ControllerBase controller, Result result)
    {
        if (result.IsSuccess)
            return controller.NoContent();

        var error = result.Error;
        return controller.StatusCode(
            MapToHttpStatusCode(error.Type),
            new
            {
                error = error.Code,
                message = error.Message,
            });
    }

    private static int MapToHttpStatusCode(ErrorType errorType) => errorType switch
    {
        ErrorType.Failure => StatusCodes.Status400BadRequest,
        ErrorType.Validation => StatusCodes.Status400BadRequest,
        ErrorType.BadRequest => StatusCodes.Status400BadRequest,
        ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
        ErrorType.Forbidden => StatusCodes.Status403Forbidden,
        ErrorType.NotFound => StatusCodes.Status404NotFound,
        ErrorType.Conflict => StatusCodes.Status409Conflict,
        ErrorType.InternalError => StatusCodes.Status500InternalServerError,
        _ => StatusCodes.Status500InternalServerError
    };
}
