using App.Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using Quote.Application.Core.Abstractions.Data;
using Quote.Contracts.Common;
using Quote.Contracts.Responses.CategoriesResponse;
using Quote.Domain.Core.Primitives.Maybe;

using Quote.Domain.Core.Primitives.Result;

namespace Quote.Application.Category.Queries.GetAllCategories;

public class GetAllCategoriesQueryhandler(IDbContext dbContext) : IQueryHandler<GetAllCategoriesQuery, Maybe<PagedList<CategoryResponse>>>
{
    public async Task<Maybe<PagedList<CategoryResponse>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = from category in dbContext.Set<Domain.Entities.Category>().AsNoTracking()
            orderby category.Name
            select new CategoryResponse(
                category.Id,
                category.Name);
        if (!categories.Any())
            return Maybe<PagedList<CategoryResponse>>.None;
        
        
        int totalCount = await categories.CountAsync(cancellationToken);
        var responsesPage = await categories
            .Skip(request.Page < 1 ? 0 : (request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToArrayAsync(cancellationToken);
        
        return new PagedList<CategoryResponse>(responsesPage,request.Page,request.PageSize,totalCount);

        
    }
}