using App.Application.Abstractions.Messaging;
using Quote.Contracts.Responses.QuoteResponse;
using Quote.Domain.Core.Primitives.Maybe;

namespace Quote.Application.Quote.Queries.GetByIdQuote;

public sealed record GetQuoteByIdQuery(Guid QuoteId): IQuery<Maybe<QuoteResponse>>;