using Microsoft.EntityFrameworkCore;
using Quote.Application.Core.Abstractions.Data;
using Quote.Domain.Core.Primitives.Maybe;
using Quote.Domain.Entities;
using Quote.Domain.Repositories;

namespace Quote.Persistence.Repositories;

internal sealed class CategoryRepository(IDbContext dbContext) : GenericRepository<Domain.Entities.Category>(dbContext),ICategoryRepository
{
    public Task<Maybe<Category>> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        => FirstOrDefaultAsync(c => c.Name == name, cancellationToken);
}