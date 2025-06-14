using Microsoft.EntityFrameworkCore;
using Quote.Application.Core.Abstractions.Data;
using Quote.Application.Core.Abstractions.Messaging;
using Quote.Contracts.Common;
using Quote.Contracts.Responses.CategoriesResponse;
using Quote.Domain.Core.Primitives.Maybe;

namespace Quote.Application.Category.Queries.GetCategories;

public class GetCategoriesQueryhandler(IDbContext dbContext) : IQueryHandler<GetCategoriesQuery, Maybe<PagedList<CategoryResponse>>>
{
    public async Task<Maybe<PagedList<CategoryResponse>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var query = from category in  dbContext.Set<Domain.Entities.Category>().AsNoTracking()
            where request.CategoryId.HasValue ? category.Id == request.CategoryId.Value : true
                where !string.IsNullOrWhiteSpace(request.Text) ? category.Name.Contains(request.Text) : true
                orderby category.Name
      
            select new  CategoryResponse(
                category.Id,
                category.Name)
            {
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt
            };
                
        if (!await query.AnyAsync(cancellationToken))
            return Maybe<PagedList<CategoryResponse>>.None;
        
        int totalCount = await query.CountAsync(cancellationToken);
        var responsesPage = await query
            .Skip(request.Page < 1 ? 0 : (request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToArrayAsync(cancellationToken);
        
        return new PagedList<CategoryResponse>(responsesPage, request.Page, request.PageSize, totalCount);
    }
}