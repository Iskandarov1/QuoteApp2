using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quote.Application.Quote.Queries.GetRandomQuote;
using Quote.Application.Subscribers.Commands.CreateSubscription;
using Quote.Application.Subscribers.Commands.RemoveSubscriptionCommand;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Quote.Services.BackgroundTasks.Services;

public class TelegramBotService : BackgroundService
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<TelegramBotService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public TelegramBotService(IConfiguration configuration, ILogger<TelegramBotService> logger,
        IServiceProvider serviceProvider)
    {
        var botToken = configuration["Telegram:BotToken"];

        if (string.IsNullOrEmpty(botToken))
        {
            throw new InvalidOperationException("Telegram bot token is not configured");
        }

        _botClient = new TelegramBotClient(botToken);
        _logger = logger;
        _serviceProvider = serviceProvider;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var reveiverOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };
        
        _botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                errorHandler: HandlePollingErrorAsync,
                receiverOptions: reveiverOptions,
                cancellationToken: stoppingToken
            );
           
        
        var me = await _botClient.GetMe(stoppingToken);
        _logger.LogInformation("Telegram bot {BotName} started",me.Username);

        await Task.Delay(Timeout.Infinite, stoppingToken);

    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        if (update.Message is not { } message || message.Text is not {} messageText)
                return;

        var chatId = message.Chat.Id;
        _logger.LogInformation("Received message from {ChatId}: {MessageText}",chatId,messageText);

        using var scope = _serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        try
        {
            await ProcessCommand(chatId, messageText, mediator, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Error processing Telegram command from {ChatId}",chatId);
            await botClient.SendMessage(
                chatId: chatId,
                text: "Sorry , something went wrong , try again",
                cancellationToken: cancellationToken);
        }
    }

    private async Task ProcessCommand(long chatId, string messageText, IMediator mediator,
        CancellationToken cancellationToken)
    {
        switch (messageText.ToLower())
        {
            case "/start":
                await _botClient.SendMessage(
                    chatId:chatId,
                    text:" Welcome to Daily Quotes Bot!\n\n" +
                          "Commands:\n" +
                          "/subscribe - Subscribe to daily quotes\n" +
                          "/unsubscribe - Unsubscribe from quotes\n" +
                          "/quote - Get a random quote\n" +
                          "/help - Show this help message",
                    cancellationToken:cancellationToken);
                break;
            case "/subscribe":
                await HandleSubscribe(chatId, mediator, cancellationToken);
                break;
            case "/unsubscribe":
                await HandleUnsubscribe(chatId, mediator, cancellationToken);
                break;
            case "/quote":
                await HandleGetQuote(chatId, mediator, cancellationToken);
                break;
            case "/help":
                await _botClient.SendMessage(
                    chatId: chatId,
                    text: "📋 Available commands:\n\n" +
                          "/subscribe - Subscribe to daily quotes\n" +
                          "/unsubscribe - Unsubscribe from quotes\n" +
                          "/quote - Get a random quote\n" +
                          "/help - Show this help message", 
                    cancellationToken: cancellationToken);
                break;
            
            default:
                await _botClient.SendMessage(
                    chatId:chatId,
                    text: "I don't understand that command. Type /help to see available commands.",
                    cancellationToken:cancellationToken);
                break;
        }
    }

    private async Task HandleSubscribe(long chatId, IMediator mediator, CancellationToken cancellationToken)
    {
        var command = new CreateSubscriptionCommand(null,null,null,chatId);
        var result = await mediator.Send(command, cancellationToken);

        if (result.IsSuccess)
        {
            await _botClient.SendMessage(
                chatId:chatId,
                text:"Subscribed successfully",
                cancellationToken:cancellationToken
            );
        }
        else
        {
            await _botClient.SendMessage(
                chatId: chatId,
                text: $"Subscribtion failed",
                cancellationToken:cancellationToken);
        }
    }
    private async Task HandleUnsubscribe(long chatId, IMediator mediator, CancellationToken cancellationToken)
    {
        var command = new RemoveSubscriptionCommand(null,chatId);
        var result = await mediator.Send(command, cancellationToken);

        if (result.IsSuccess)
        {
            await _botClient.SendMessage(
                chatId: chatId,
                text: " You've been successfully unsubscribed from daily quotes.",
                cancellationToken: cancellationToken);
        }
        else
        {
            await _botClient.SendMessage(
                chatId: chatId,
                text: $" Unsubscribe failed:",
                cancellationToken: cancellationToken);
        }
    }
    private async Task HandleGetQuote(long chatId, IMediator mediator, CancellationToken cancellationToken)
    {
        var query = new GetRandomQuoteQuery();
        var result = await mediator.Send(query, cancellationToken);

        if (result.HasValue)
        {
            var quote = result.Value;
            var message = $"💭 \"{quote.Text}\"\n\n— {quote.Author}\n\n📚";
            
            await _botClient.SendMessage(
                chatId: chatId,
                text: message,
                cancellationToken: cancellationToken);
        }
        else
        {
            await _botClient.SendMessage(
                chatId: chatId,
                text: "Sorry, no quotes are available at the moment.",
                cancellationToken: cancellationToken);
        }
    }
    private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        _logger.LogError(exception, "Telegram polling error: {ErrorMessage}", errorMessage);
        return Task.CompletedTask;
    }
}