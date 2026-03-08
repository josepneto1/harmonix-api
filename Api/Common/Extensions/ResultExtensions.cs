using Harmonix.Application.Common.Results;
using Microsoft.AspNetCore.Mvc;

namespace Harmonix.Api.Common.Extensions;

public static class ResultExtensions
{
    public static IActionResult GetResult<T>(this ControllerBase controller, Result<T> result)
    {
        if (result.IsSuccess)
        {
            var method = controller.HttpContext.Request.Method;

            return method switch
            {
                "POST" => controller.StatusCode(StatusCodes.Status201Created, result.Value),
                "DELETE" => controller.StatusCode(StatusCodes.Status204NoContent),
                "PUT" or "PATCH" => controller.StatusCode(StatusCodes.Status200OK, result.Value),
                "GET" => controller.StatusCode(StatusCodes.Status200OK, result.Value),
                _ => controller.StatusCode(StatusCodes.Status200OK, result.Value)
            };
        }

        var error = result.Error;
        return controller.StatusCode(
            (int)error.Status,
            new
            {
                error = error.Code,
                message = error.Message,
                details = error.Details
            });
    }

    public static IActionResult GetResult(this ControllerBase controller, Result result)
    {
        if (result.IsSuccess)
            return controller.NoContent();

        var error = result.Error;
        return controller.StatusCode(
            (int)error.Status,
            new
            {
                error = error.Code,
                message = error.Message,
                details = error.Details
            });
    }

    //public static IActionResult GetCreatedResult<T>(this ControllerBase controller, Result<T> result)
    //{
    //    if (result.IsSuccess)
    //        return controller.StatusCode(StatusCodes.Status201Created, result.Value);

    //    var error = result.Error;
    //    return controller.StatusCode(
    //        (int)error.Status,
    //        new
    //        {
    //            error = error.Code,
    //            message = error.Message,
    //            details = error.Details
    //        });
    //}
}
