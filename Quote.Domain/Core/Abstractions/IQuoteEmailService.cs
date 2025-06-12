namespace Quote.Domain.Core.Abstractions;

public interface IQuoteEmailService
{
    Task SendDailyQuoteEmailAsync(string email, string author, string text, CancellationToken cancellationToken = default);
    Task SendWelcomeEmailAsync(string email, CancellationToken cancellationToken = default);
}
