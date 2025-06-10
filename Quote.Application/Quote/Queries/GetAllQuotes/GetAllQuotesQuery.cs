using App.Application.Abstractions.Messaging;
using Quote.Contracts.Common;
using Quote.Contracts.Responses.QuoteResponse;
using Quote.Domain.Core.Primitives.Maybe;

namespace Quote.Application.Quote.Queries.GetAllQuotes;

public sealed record GetAllQuotesQuery(
    string? Filter,
    int Page,
    int PageSize) :IQuery<Maybe<PagedList<QuoteResponse>>>;