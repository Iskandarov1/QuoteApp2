using Quote.Application.Core.Abstractions.Data;
using Quote.Application.Core.Abstractions.Messaging;
using Quote.Domain.Core.Errors;
using Quote.Domain.Core.Primitives.Result;
using Quote.Domain.Entities;
using Quote.Domain.Repositories;

namespace Quote.Application.Subscribers.Commands.RemoveSubscriptionCommand;

public class RemoveSubscriptionCommandHandler(
    ISubscriberRepository subscriberRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<RemoveSubscriptionCommand,Result>
{
    public async Task<Result> Handle(RemoveSubscriptionCommand request, CancellationToken cancellationToken)
    {
        
        Subscriber? subscriber = null;

        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            subscriber = await subscriberRepository.GetByEmailAsync(request.Email, cancellationToken);
        }
        else if (request.TelegramUser.HasValue)
        {
            subscriber = await subscriberRepository.GetByTelegramChatIdAsync(request.TelegramUser.Value, cancellationToken);
        }

        if (subscriber == null)
            return Result.Failure(DomainErrors.Subscriber.NotFound);

        subscriber.Deactivate();
         subscriberRepository.Update(subscriber);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();

    }
}