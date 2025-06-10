using Quote.Application.Core.Abstractions.Data;
using Quote.Domain.Entities;
using Quote.Domain.Repositories;
using Quote.Domain.Core.Primitives.Maybe;

namespace Quote.Persistence.Repositories;

/// <summary>
/// Represents the quote repository.
/// </summary>
internal sealed class QuoteRepository : GenericRepository<Domain.Entities.Quote>, IQuoteRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="QuoteRepository"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public QuoteRepository(IDbContext dbContext)
        : base(dbContext)
    {
    }
    
}