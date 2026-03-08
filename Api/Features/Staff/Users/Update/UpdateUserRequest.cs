using Harmonix.Domain.Users.Enums;

namespace Harmonix.Api.Features.Staff.Users.Update;

public record UpdateUserRequest
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
    public string? Email { get; init; }
    public Role? Role { get; init; }
};
