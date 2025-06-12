using Microsoft.EntityFrameworkCore;
using Quote.Application.Core.Abstractions.Data;
using Quote.Application.Core.Abstractions.Messaging;
using Quote.Contracts.Responses.CategoriesResponse;
using Quote.Domain.Core.Primitives.Maybe;

namespace Quote.Application.Category.Queries.GetByIdCategory;

public class GetCategoryByIdQueryHandler(IDbContext dbContext) : IQueryHandler<GetCategoryByIdQuery, Maybe<CategoryResponse>>
{
    public async Task<Maybe<CategoryResponse>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var response = await (from category in dbContext.Set<Domain.Entities.Category>().AsNoTracking()
                where category.Id == request.CategoryId
                    select new CategoryResponse(
                        category.Id,
                        category.Name)).FirstOrDefaultAsync(cancellationToken);
        
        if (response is null)
            return Maybe<CategoryResponse>.None;
                
        return response;
        
    }
}