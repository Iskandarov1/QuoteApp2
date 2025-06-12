namespace Quote.Application.Core.Abstractions.Services;

public interface ITelegramService
{
    Task SendMessageAsync(long chatId, string message, CancellationToken cancellationToken = default);
    Task<bool> IsUserSubscribedAsync(long chatId, CancellationToken cancellationToken = default);
}