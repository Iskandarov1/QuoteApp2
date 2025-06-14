using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Quote.Application.Core.Abstractions.Messaging;
using Quote.Contracts.Common;
using Quote.Domain.Core.Primitives.Maybe;
using Quote.Domain.Entities;

namespace Quote.Application.Subscribers.Queries.GetActiveSubscribers;

public record GetActiveSubscribersQuery(
    string? Filter,
    [property: Required]
    [property: DefaultValue(0)] int Page = 0, 
    [property: Required]
    [property: DefaultValue(10)] int PageSize = 10) : IQuery<Maybe<PagedList<Subscriber>>>;