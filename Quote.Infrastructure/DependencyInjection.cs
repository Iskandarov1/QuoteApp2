using Quote.Application.Core.Abstractions.Common;
using Quote.Infrastructure.Common;
using Microsoft.Extensions.DependencyInjection;
using Quote.Application.Core.Abstractions.Services;
using Quote.Domain.Core.Abstractions;
using Quote.Infrastructure.Services;

namespace Quote.Infrastructure;

public static class DependencyInjection
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IDateTime, MachineDateTime>();

        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IQuoteEmailService, QuoteEmailService>();
        services.AddScoped<ITelegramService, TelegramService>();

        return services;
    }
}