using Quote.Application.Core.Abstractions.Messaging;
using Quote.Contracts.Responses.CategoriesResponse;
using Quote.Domain.Core.Primitives.Maybe;

namespace Quote.Application.Category.Queries.GetByIdCategory;

public sealed record GetCategoryByIdQuery(Guid CategoryId) : IQuery<Maybe<CategoryResponse>>;