using System.ComponentModel;
using Quote.Application.Core.Abstractions.Messaging;
using Quote.Contracts.Common;
using Quote.Contracts.Responses.QuotesResponse;
using Quote.Domain.Core.Primitives.Maybe;

namespace Quote.Application.Quote.Queries.GetQuotes;

public sealed record GetQuotesQuery(
    string? Author,
    //[property: Required]
    [property: DefaultValue(0)] int Page = 0, 
    //[property: Required]
    [property: DefaultValue(10)] int PageSize = 10) : IQuery<Maybe<PagedList<QuoteResponse>>>;