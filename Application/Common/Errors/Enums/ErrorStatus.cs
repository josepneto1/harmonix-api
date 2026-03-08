namespace Harmonix.Application.Common.Errors.Enums;

public enum ErrorStatus
{
    None = 0,
    BadRequest = 400,
    Unauthorized = 401,
    Forbidden = 403,
    NotFound = 404,
    Conflict = 409,
    InternalError = 500
}
