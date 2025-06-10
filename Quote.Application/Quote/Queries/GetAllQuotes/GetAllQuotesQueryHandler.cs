using App.Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using Quote.Application.Core.Abstractions.Data;
using Quote.Contracts.Common;
using Quote.Contracts.Responses.QuoteResponse;
using Quote.Domain.Core.Primitives.Maybe;

namespace Quote.Application.Quote.Queries.GetAllQuotes;

public class GetAllQuotesQueryHandler(IDbContext dbContext):IQueryHandler<GetAllQuotesQuery,Maybe<PagedList<QuoteResponse>>>
{
    public async Task<Maybe<PagedList<QuoteResponse>>> Handle(GetAllQuotesQuery request, CancellationToken cancellationToken)
    {
        var query = from quote in dbContext.Set<Domain.Entities.Quote>().AsNoTracking()
            where !string.IsNullOrWhiteSpace(request.Filter)
                ? EF.Functions.Like(quote.Author.Value.ToLower(), $"%{request.Filter.ToLowerInvariant()}%") ||
                  EF.Functions.Like(quote.Textt.Value.ToLower(), $"%{request.Filter.ToLowerInvariant()}%") ||
                  EF.Functions.Like(quote.Category.Value.ToLower(), $"%{request.Filter.ToLowerInvariant()}%")
                : true
            orderby quote.CreatedAt descending
            select new QuoteResponse(
                quote.Id,
                quote.Author.Value,
                quote.Textt.Value,
                quote.Category.Value);

        int totalCount = await query.CountAsync(cancellationToken);

        var responsesPage = await query
            .Skip(request.Page < 1 ? 0 : (request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToArrayAsync(cancellationToken);

        return Maybe<PagedList<QuoteResponse>>.From(
            new PagedList<QuoteResponse>(responsesPage,request.Page,request.PageSize,totalCount)
            );
    }
}