using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quote.Persistence;

namespace Quote.Services.BackgroundTasks.Services;

public class CleanUpOldQuotesService(
    ILogger<CleanUpOldQuotesService> logger,
    QuoteSingletonDbContext dbContext) : BackgroundService
{
    private readonly TimeSpan _interval = TimeSpan.FromMinutes(5);
    private readonly TimeSpan _retentionPeriod = TimeSpan.FromHours(24);
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
             
            try
            {
                await CleanUpQuotes(stoppingToken);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error occurred during the clean up");
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }

    private async Task CleanUpQuotes(CancellationToken stoppingToken)
    {
        logger.LogInformation("Starting quote cleanup at {Time}", DateTimeOffset.UtcNow);
        
        try
        {
            var cutoffDate = DateTime.UtcNow - _retentionPeriod;
            var quotesToDelete =  dbContext.Set<Domain.Entities.Quote>()
                .Where(q => q.CreatedAt < cutoffDate);
            
            var deletedCount = await dbContext.ExecuteDeleteAsync(quotesToDelete, stoppingToken);
            
            if (deletedCount > 0)
            {
                logger.LogInformation("Successfully deleted {Count} quotes older than {Hours} hours",
                    quotesToDelete, _retentionPeriod.TotalHours);
            }
            else
            {
                logger.LogInformation("No quotes found older than {Hours} hours",
                    _retentionPeriod.TotalHours);
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to cleanup old quotes");

        }
    }
}