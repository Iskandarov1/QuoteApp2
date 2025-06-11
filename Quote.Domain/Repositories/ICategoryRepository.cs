using Quote.Domain.Core.Primitives.Maybe;

namespace Quote.Domain.Repositories;

public interface ICategoryRepository
{
    Task<Maybe<Entities.Category>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Maybe<Entities.Category>> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<Maybe<IEnumerable<Entities.Category>>> GetBulkAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);

    void Insert(Entities.Category item);
    void Update(Entities.Category item);
    void Remove(Entities.Category item);
}