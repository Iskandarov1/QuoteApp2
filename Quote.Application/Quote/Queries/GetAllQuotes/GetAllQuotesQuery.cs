using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Application.Abstractions.Messaging;
using Quote.Contracts.Common;
using Quote.Contracts.Responses.QuotesResponse;
using Quote.Domain.Core.Primitives.Maybe;

namespace Quote.Application.Quote.Queries.GetAllQuotes;

public sealed record GetAllQuotesQuery(
    string? Filter,
    //[property: Required]
    [property: DefaultValue(0)] int Page = 0, 
    //[property: Required]
    [property: DefaultValue(10)] int PageSize = 10) : IQuery<Maybe<PagedList<QuoteResponse>>>;