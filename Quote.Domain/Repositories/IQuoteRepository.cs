using Quote.Domain.Core.Primitives.Maybe;
using Quote.Domain.Entities;
namespace Quote.Domain.Repositories;

public interface IQuoteRepository
{
 Task<Maybe<Entities.Quote>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

 Task<Maybe<IEnumerable<Entities.Quote>>> GetBulkAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);

 void Insert(Entities.Quote item);
 void Update(Entities.Quote item);
 void Remove(Entities.Quote item);


}