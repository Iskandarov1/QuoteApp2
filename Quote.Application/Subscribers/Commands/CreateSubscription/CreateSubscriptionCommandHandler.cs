using Microsoft.Extensions.Logging;
using Quote.Application.Core.Abstractions.Data;
using Quote.Application.Core.Abstractions.Messaging;
using Quote.Contracts.Requests.SubscriberRequest;
using Quote.Domain.Core.Abstractions;
using Quote.Domain.Core.Localizations;
using Quote.Domain.Core.Primitives;
using Quote.Domain.Core.Primitives.Result;
using Quote.Domain.Repositories;
using Quote.Domain.ValueObjects;
using Quote.Domain.Entities;

namespace Quote.Application.Subscribers.Commands.CreateSubscription;

public class CreateSubscriptionCommandHandler(
    ISubscriberRepository subscriberRepository,
    ISharedViewLocalizer sharedViewLocalizer,
    IUnitOfWork unitOfWork,
    IQuoteEmailService quoteEmailService,
    ILogger<CreateSubscriptionCommandHandler> logger) : ICommandHandler<CreateSubscriptionCommand,Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken)
    {
        // Handle Email subscription
        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            var emailResult = Email.Create(request.Email, CaseConverter.PascalToSnakeCase(nameof(CreateSubscriberRequest.Email)), sharedViewLocalizer);
            if (emailResult.IsFailure)
                return Result.Failure<Guid>(emailResult.Error);
            
            var firstName = FirstName.Create(request.FirstName, CaseConverter.PascalToSnakeCase(nameof(CreateSubscriberRequest.FirstName)), sharedViewLocalizer);
            if (firstName.IsFailure)
                return Result.Failure<Guid>(firstName.Error);
            
            var lastName = LastName.Create(request.LastName, CaseConverter.PascalToSnakeCase(nameof(CreateSubscriberRequest.LastName)), sharedViewLocalizer);
            if (lastName.IsFailure)
                return Result.Failure<Guid>(lastName.Error);

            var existingEmailSubscriber = await subscriberRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (existingEmailSubscriber.HasValue)
            {
                var subscriber = existingEmailSubscriber.Value;
                if (!subscriber.IsActive)
                {
                    subscriber.Activate();
                    subscriberRepository.Update(subscriber);
                    await unitOfWork.SaveChangesAsync(cancellationToken);
                    
                    try
                    {
                        await quoteEmailService.SendWelcomeEmailAsync(request.Email, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Failed to send welcome email to {Email}", request.Email);
                    }
                }

                return Result.Success(subscriber.Id);
            }

            var emailSubscriber = Subscriber.CreateWithEmail(emailResult.Value, firstName.Value, lastName.Value, request.AttachedFilePath);
            subscriberRepository.Insert(emailSubscriber);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            
            try
            {
                await quoteEmailService.SendWelcomeEmailAsync(request.Email, cancellationToken);
                logger.LogInformation("Welcome email sent to new subscriber: {Email}", request.Email);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failed to send welcome email to {Email}", request.Email);
            }
            
            return Result.Success(emailSubscriber.Id);
        }
        
        // Handle Telegram subscription
        if (request.TelegramUser.HasValue)
        {
            var existingTelegramSubscriber = await subscriberRepository.GetByTelegramChatIdAsync(request.TelegramUser.Value, cancellationToken);
            
            if (existingTelegramSubscriber.HasValue)
            {
                var subscriber = existingTelegramSubscriber.Value;
                if (!subscriber.IsActive)
                {
                    subscriber.Activate();
                    subscriberRepository.Update(subscriber);
                    await unitOfWork.SaveChangesAsync(cancellationToken);
                }
                return Result.Success(subscriber.Id);
            }
            
            var telegramSubscriber = Subscriber.CreateWithTelegram(request.TelegramUser.Value);
            subscriberRepository.Insert(telegramSubscriber);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success(telegramSubscriber.Id);
        }

        return Result.Failure<Guid>(
            new Error("subscriber.contact_required",
                sharedViewLocalizer["You must provide either Email or Telegram user."]));
    }
}