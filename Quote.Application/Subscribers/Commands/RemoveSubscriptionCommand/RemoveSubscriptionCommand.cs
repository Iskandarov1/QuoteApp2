using Quote.Application.Core.Abstractions.Messaging;
using Quote.Domain.Core.Primitives.Result;

namespace Quote.Application.Subscribers.Commands.RemoveSubscriptionCommand;

public record RemoveSubscriptionCommand(
    string? FirstName,
    string? LastName,
    string? Email,
    long? TelegramUser): ICommand<Result<bool>>;