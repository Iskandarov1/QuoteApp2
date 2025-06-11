using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Quote.Application.Resources;
using Quote.Domain.Core.Localizations;

namespace Quote.Application;

public static class DependencyInjection
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.TryAddTransient<ISharedViewLocalizer, ApplicationSharedViewLocalizer>();

        return services;
        
    }
}