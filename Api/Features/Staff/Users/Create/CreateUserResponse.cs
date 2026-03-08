using Harmonix.Domain.Users.Enums;

namespace Harmonix.Api.Features.Staff.Users.Create;

public record CreateUserResponse(
    Guid Id,
    Guid CompanyId,
    string Name,
    string Email,
    Role Role,
    DateTimeOffset CreatedAt
);
