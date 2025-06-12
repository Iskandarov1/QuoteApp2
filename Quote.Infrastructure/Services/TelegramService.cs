using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quote.Application.Core.Abstractions.Services;
using Telegram.Bot;
using Telegram.Bot.Exceptions;

namespace Quote.Infrastructure.Services;

public class TelegramService :ITelegramService
{
    private readonly TelegramBotClient _botClient;
    private readonly ILogger<TelegramService> _logger;


    public TelegramService(IConfiguration configuration, ILogger<TelegramService> logger)
    {
        var botToken = configuration["Telegram:BotToken"];

        if (string.IsNullOrEmpty(botToken))
        {
            throw new InvalidOperationException("Telegram bot token is not configured");
        }

        _botClient = new TelegramBotClient(botToken);
        _logger = logger;

    }
    
    
    public async Task SendMessageAsync(long chatId, string message, CancellationToken cancellationToken = default)
    {
        try
        {
            await _botClient.SendMessage(
                chatId: chatId,
                text: message,
                cancellationToken: cancellationToken);
            _logger.LogInformation("Quote sent to telegram user {ChatId} : {Message}",chatId,message);
        }
        catch (ApiRequestException  ex)
        {
            _logger.LogError(ex,"Failed to send Telegram message to {ChatId}",chatId);
            throw;
        }
    }

    public async Task<bool> IsUserSubscribedAsync(long chatId, CancellationToken cancellationToken = default)
    {
        try
        {
            var chat = await _botClient.GetChat(chatId, cancellationToken);
            return true;
        }
        catch (Exception e)
        {
            _logger.LogWarning(e,"Could not verify Telegram user {ChatId}", chatId);
            return false;
        }
    }
}