using System.ComponentModel;
using App.Application.Abstractions.Messaging;
using Quote.Contracts.Common;
using Quote.Contracts.Responses.CategoriesResponse;
using Quote.Domain.Core.Primitives.Maybe;

namespace Quote.Application.Category.Queries.GetAllCategories;

public sealed record GetAllCategoriesQuery (
    string? Filter,
    //[property: Required]
    [property: DefaultValue(0)] int Page = 0, 
    //[property: Required]
    [property: DefaultValue(10)] int PageSize = 10) : IQuery<Maybe<PagedList<CategoryResponse>>>;