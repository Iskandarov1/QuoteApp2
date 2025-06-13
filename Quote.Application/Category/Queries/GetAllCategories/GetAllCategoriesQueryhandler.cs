using Microsoft.EntityFrameworkCore;
using Quote.Application.Core.Abstractions.Data;
using Quote.Application.Core.Abstractions.Messaging;
using Quote.Contracts.Common;
using Quote.Contracts.Responses.CategoriesResponse;
using Quote.Domain.Core.Primitives.Maybe;
using Quote.Domain.Core.Primitives.Result;

namespace Quote.Application.Category.Queries.GetAllCategories;

public class GetAllCategoriesQueryhandler(IDbContext dbContext) : IQueryHandler<GetAllCategoriesQuery, Maybe<PagedList<CategoryResponse>>>
{
    public async Task<Maybe<PagedList<CategoryResponse>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.Set<Domain.Entities.Category>().AsNoTracking();

        if (request.CategoryId.HasValue)
        {
            query = query.Where(c => c.Id == request.CategoryId.Value);
        }
        
        if (!string.IsNullOrWhiteSpace(request.Text))
        {
            query = query.Where(c => c.Name.Contains(request.Text));
        }
        
        var categories = query
            .OrderBy(c => c.Name)
            .Select(category => new CategoryResponse(
                category.Id,
                category.Name)
            {
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt
            });
                
        if (!await categories.AnyAsync(cancellationToken))
            return Maybe<PagedList<CategoryResponse>>.None;
        
        int totalCount = await categories.CountAsync(cancellationToken);
        var responsesPage = await categories
            .Skip(request.Page < 1 ? 0 : (request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToArrayAsync(cancellationToken);
        
        return new PagedList<CategoryResponse>(responsesPage, request.Page, request.PageSize, totalCount);
    }
}