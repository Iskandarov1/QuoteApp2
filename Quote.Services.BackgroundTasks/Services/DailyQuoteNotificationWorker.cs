using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quote.Application.Core.Abstractions.Services;
using Quote.Application.Quote.Queries.GetRandomQuote;
using Quote.Domain.Core.Abstractions;
using Quote.Domain.Entities;
using Quote.Domain.Repositories;
using Quote.Domain.ValueObjects;
using Quote.Persistence;
using Category = Quote.Domain.Entities.Category;
using Quote.Domain.Enumerations;

namespace Quote.Services.BackgroundTasks.Services;

public class DailyQuoteNotificationWorker(
    ILogger<DailyQuoteNotificationWorker> logger,
    IServiceProvider serviceProvider,
    QuoteSingletonDbContext singletonContext
    ) : BackgroundService
{
    private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(1);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                logger.LogInformation("🔄 Quote notification cycle starting at {Time}", DateTimeOffset.UtcNow);
                await SendDailyQuotesToSubscribers(stoppingToken);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error occurred during quote notification");
            }

            await Task.Delay(_checkInterval, stoppingToken);
        }
    }

    private async Task SendDailyQuotesToSubscribers(CancellationToken cancellationToken)
    {
        logger.LogInformation("Sending quotes to all subscribers for demo at {Time}", DateTimeOffset.UtcNow);

        using var scope = serviceProvider.CreateScope();
        var quoteEmailService = scope.ServiceProvider.GetRequiredService<IQuoteEmailService>();
        var telegramService = scope.ServiceProvider.GetRequiredService<ITelegramService>();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        try
        {
            var subscribers = await singletonContext.Set<Subscriber>()
                .Where(s => s.IsActive)
                .ToListAsync(cancellationToken);
            
            if (!subscribers.Any())
            {
                logger.LogInformation("No active subscribers found");
                return;
            }

            var randomQuoteResult = await mediator.Send(new GetRandomQuoteQuery(), cancellationToken);
            
            if (randomQuoteResult.HasNoValue)
            {
                logger.LogWarning("No quotes available for notification");
                return;
            }

            var quote = randomQuoteResult.Value;
            
            var telegramContent = FormatTelegramContent(quote.Author, quote.Text, quote.CategoryName);

            var successCount = 0;

            foreach (var subscriber in subscribers)
            {
                try
                {
                    if (subscriber.PreferredNotificationMethod == NotificationPreference.Email &&
                        !string.IsNullOrEmpty(subscriber.Email))
                    {

                        await quoteEmailService.SendDailyQuoteEmailAsync(
                            subscriber.Email,
                            quote.Author,
                            quote.Text);
                        logger.LogInformation("📧 Quote sent to {Email}: \"{Quote}\" by {Author}",
                            subscriber.Email, quote.Text, quote.Author);
                        break;
                    }

                    else if (subscriber.PreferredNotificationMethod == NotificationPreference.Telegram && subscriber.TelegramUser.HasValue)
                    {
                        await telegramService.SendMessageAsync(
                            subscriber.TelegramUser.Value,
                            telegramContent,
                            cancellationToken);
                        logger.LogInformation("📱 Quote sent to Telegram {UserId}: \"{Quote}\" by {Author}", 
                            subscriber.TelegramUser.Value, quote.Text, quote.Author);
                        break;
                    }
                    successCount++;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to send notification to subscriber {SubscriberId}", subscriber.Id);
                }
            }

            await singletonContext.SaveChangesAsync(cancellationToken);
            logger.LogInformation("Successfully sent quotes to {Count} subscribers", successCount);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send quote notifications");
            throw;
        }
    }
    

    private string FormatTelegramContent(string author, string text, string category)
    {
        return $"💭 \"{text}\"\n\n— {author}\n\n📚 Category: {category}";
    }
}