using Harmonix.Domain.Companies.ValueObjects;

namespace Harmonix.Domain.Companies.Services;

public interface IAliasUniqueChecker
{
    Task<bool> IsUniqueAsync(Alias alias);
}
