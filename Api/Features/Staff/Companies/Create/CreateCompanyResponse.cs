namespace Harmonix.Api.Features.Staff.Companies.Create;

public record CreateCompanyResponse(
    Guid Id,
    string Name,
    string Alias,
    DateTimeOffset CreatedAt,
    DateTimeOffset ExpirationDate
);