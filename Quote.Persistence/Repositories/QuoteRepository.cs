using Quote.Application.Core.Abstractions.Data;
using Quote.Domain.Entities;
using Quote.Domain.Repositories;
using Quote.Domain.Core.Primitives.Maybe;

namespace Quote.Persistence.Repositories;

/// <summary>
/// Represents the quote repository.
/// </summary>
internal sealed class QuoteRepository(IDbContext dbContext) : GenericRepository<Domain.Entities.Quote>(dbContext), IQuoteRepository
{
}