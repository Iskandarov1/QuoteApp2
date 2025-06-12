using Quote.Application.Core.Abstractions.Messaging;
using Quote.Contracts.Responses.QuotesResponse;
using Quote.Domain.Core.Primitives.Maybe;

namespace Quote.Application.Quote.Queries.GetByIdQuote;

public sealed record GetQuoteByIdQuery(Guid QuoteId): IQuery<Maybe<QuoteResponse>>;