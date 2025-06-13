using Microsoft.EntityFrameworkCore;
using Quote.Application.Core.Abstractions.Data;
using Quote.Application.Core.Abstractions.Messaging;
using Quote.Contracts.Common;
using Quote.Contracts.Responses.QuotesResponse;
using Quote.Domain.Core.Primitives.Maybe;

namespace Quote.Application.Quote.Queries.GetAllQuotes;

public class GetAllQuotesQueryHandler(IDbContext dbContext):IQueryHandler<GetAllQuotesQuery,Maybe<PagedList<QuoteResponse>>>
{
    public async Task<Maybe<PagedList<QuoteResponse>>> Handle(GetAllQuotesQuery request, CancellationToken cancellationToken)
    {
        var query = from quote in dbContext.Set<Domain.Entities.Quote>().AsNoTracking()
            join category in dbContext.Set<Domain.Entities.Category>().AsNoTracking()
                on quote.CategoryId equals category.Id 
            where !string.IsNullOrWhiteSpace(request.Author)
                ? quote.Author.Contains(request.Author) ||
                  quote.Textt.Contains(request.Author)
                : true
            orderby quote.CreatedAt descending
            select new QuoteResponse(
                quote.Id,
                quote.Author,
                quote.Textt,
                quote.CategoryId,
                category.Name);

        int totalCount = await query.CountAsync(cancellationToken);

        var responsesPage = await query
            .Skip(request.Page < 1 ? 0 : (request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToArrayAsync(cancellationToken);

        return new PagedList<QuoteResponse>(responsesPage,request.Page,request.PageSize,totalCount);
    }
}