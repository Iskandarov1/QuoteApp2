using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Quote.Application.Core.Abstractions.Data;
using Quote.Domain.Repositories;
using Quote.Persistence.Infrastructure;
using Quote.Persistence.Repositories;

namespace Quote.Persistence;

public static class DependencyInjection
{
  
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString(ConnectionString.SettingsKey)
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddSingleton(new ConnectionString(connectionString));

        services.AddDbContext<QuoteContext>(options =>
        {
            options.UseNpgsql(connectionString);
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
        });
        

        services.AddSingleton<QuoteSingletonDbContext>(serviceProvider =>
        {
            var optionsBuilder = new DbContextOptionsBuilder<QuoteSingletonDbContext>();
            optionsBuilder.UseNpgsql(connectionString);
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.EnableDetailedErrors();
            
            var dateTime = new SystemDateTime();
            
            return new QuoteSingletonDbContext(optionsBuilder.Options, dateTime);
        });

        services.AddScoped<IDbContext>(serviceProvider => serviceProvider.GetRequiredService<QuoteContext>());
        services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<QuoteContext>());
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.TryAddScoped<IQuoteRepository, QuoteRepository>();
        services.AddScoped<ISubscriberRepository, SubscriberRepository>();

        return services;
    }
}