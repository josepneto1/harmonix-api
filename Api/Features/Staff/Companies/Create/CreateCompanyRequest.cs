namespace Harmonix.Api.Features.Staff.Companies.Create;

public record CreateCompanyRequest(
    string Name,
    string Alias,
    DateTimeOffset ExpirationDate
);
