using Quote.Domain.Core.Primitives.Maybe;
using Quote.Domain.Entities;
namespace Quote.Domain.Repositories;

public interface ISubscriberRepository
{
    Task<Maybe<Entities.Subscriber>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Maybe<Entities.Subscriber>> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<Maybe<Entities.Subscriber>> GetByTelegramChatIdAsync(long? chatId, CancellationToken cancellationToken = default);
    Task<Maybe<IEnumerable<Entities.Subscriber>>> GetActiveSubscribersAsync(CancellationToken cancellationToken = default);
    void Insert(Subscriber subscriber);
    void Update(Subscriber subscriber);
    void Remove(Subscriber subscriber);
}   