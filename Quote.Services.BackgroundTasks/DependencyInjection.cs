using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quote.Services.BackgroundTasks.Services;

namespace Quote.Services.BackgroundTasks;

public static class DependencyInjection
{
    public static IServiceCollection AddBackgroundTasks(
        this IServiceCollection services,
        IConfiguration configuration)
    {

        services.AddHostedService<DailyQuoteNotificationWorker>();
        services.AddHostedService<TelegramBotService>();


        return services;
    }

}