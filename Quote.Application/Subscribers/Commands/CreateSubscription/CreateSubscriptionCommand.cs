using Quote.Application.Core.Abstractions.Messaging;
using Quote.Domain.Core.Primitives.Result;

namespace Quote.Application.Subscribers.Commands.CreateSubscription;

public sealed record CreateSubscriptionCommand(
    string? FirstName,
    string? LastName,
    string? Email,
    long? TelegramUser):ICommand<Result<Guid>>;