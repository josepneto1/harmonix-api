namespace Harmonix.Api.Features.Staff.Companies.Update;

public record UpdateCompanyRequest
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
    public string? Alias { get; init; }
    public DateTimeOffset? ExpirationDate { get; init; }
};
