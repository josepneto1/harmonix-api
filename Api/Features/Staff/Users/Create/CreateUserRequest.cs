using Harmonix.Domain.Users.Enums;

namespace Harmonix.Api.Features.Staff.Users.Create;

public record CreateUserRequest(
    Guid CompanyId,
    string Name,
    string Email,
    string Password,
    Role Role
);
