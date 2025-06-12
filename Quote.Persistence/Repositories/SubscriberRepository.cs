using Microsoft.EntityFrameworkCore;
using Quote.Application.Core.Abstractions.Data;
using Quote.Domain.Core.Primitives.Maybe;
using Quote.Domain.Entities;
using Quote.Domain.Repositories;

namespace Quote.Persistence.Repositories;

internal sealed class SubscriberRepository(IDbContext dbContext) : GenericRepository<Domain.Entities.Subscriber>(dbContext),ISubscriberRepository
{
    public async Task<Maybe<Subscriber>> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var getEmail = await DbContext.Set<Domain.Entities.Subscriber>().AsNoTracking().FirstOrDefaultAsync(
            s => s.Email == email, cancellationToken);
        
        return getEmail != null ? Maybe<Subscriber>.From(getEmail) : Maybe<Subscriber>.None;
    }

    public async Task<Maybe<Subscriber>> GetByTelegramChatIdAsync(long? chatId, CancellationToken cancellationToken = default)
    {
        var getBot = await DbContext.Set<Domain.Entities.Subscriber>().AsNoTracking().FirstOrDefaultAsync(
            s => s.TelegramUser == chatId, cancellationToken);
        
        return getBot != null ? Maybe<Subscriber>.From(getBot) : Maybe<Subscriber>.None;
    }

    public async Task<Maybe<IEnumerable<Subscriber>>> GetActiveSubscribersAsync(CancellationToken cancellationToken = default) =>
       Maybe<IEnumerable<Subscriber>>.From(await DbContext.Set<Domain.Entities.Subscriber>().Where(
            s => s.IsActive).ToArrayAsync(cancellationToken));
}