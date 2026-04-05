using Harmonix.Domain.Common.Services;
using Harmonix.Domain.Companies.Services;
using Harmonix.Infrastructure.Auth;
using Harmonix.Infrastructure.Data.Services;

namespace Harmonix.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddShared(this IServiceCollection services)
    {
        services.AddScoped<IEmailUniqueChecker, EmailUniqueChecker>();
        services.AddScoped<IAliasUniqueChecker, AliasUniqueChecker>();

        services.AddSingleton<IPasswordHasher, BcryptPasswordHasher>();
        services.AddScoped<JwtTokenProvider>();

        return services;
    }
}
