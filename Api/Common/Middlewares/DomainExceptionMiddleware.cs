//using Harmonix.Application.Common.Errors;
//using Harmonix.Application.Common.Errors.Enums;
//using Harmonix.Domain.Exceptions;

//namespace Harmonix.Api.Common.Middlewares;

//public class DomainExceptionMiddleware
//{
//    private readonly RequestDelegate _next;

//    public DomainExceptionMiddleware(RequestDelegate next)
//    {
//        _next = next;
//    }

//    public async Task InvokeAsync(HttpContext context)
//    {
//        try
//        {
//            await _next(context);
//        }
//        catch (DomainException ex)
//        {
//            context.Response.StatusCode = StatusCodes.Status400BadRequest;
//            context.Response.ContentType = "application/json";

//            var error = new Error(ex.Code, ex.Message, ErrorStatus.BadRequest);

//            await context.Response.WriteAsJsonAsync(new
//            {
//                error = error.Code,
//                message = error.Message
//            });
//        }
//    }
//}
