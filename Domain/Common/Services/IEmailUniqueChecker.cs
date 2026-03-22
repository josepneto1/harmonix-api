using Harmonix.Domain.Common.ValueObjects;

namespace Harmonix.Domain.Common.Services;

public interface IEmailUniqueChecker
{
    Task<bool> IsUniqueAsync(Email email);
}