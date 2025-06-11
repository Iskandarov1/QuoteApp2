using App.Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using Quote.Application.Core.Abstractions.Data;
using Quote.Contracts.Responses.QuotesResponse;
using Quote.Domain.Core.Primitives.Maybe;

namespace Quote.Application.Quote.Queries.GetByIdQuote;

public class GetQuoteByIdQueryHandler(IDbContext dbContext):IQueryHandler<GetQuoteByIdQuery,Maybe<QuoteResponse>>
{
    public async Task<Maybe<QuoteResponse>> Handle(GetQuoteByIdQuery request, CancellationToken cancellationToken)
    {
        var response = await (from quote in dbContext.Set<Domain.Entities.Quote>().AsNoTracking()
            join category in dbContext.Set<Domain.Entities.Category>() on quote.CategoryId equals category.Id 
            where quote.Id == request.QuoteId
            select new QuoteResponse(
                quote.Id,
                quote.Author,
                quote.Textt,
                quote.CategoryId,
                category.Name))
            .FirstOrDefaultAsync(cancellationToken);
        if (response is null)
        {
            return Maybe<QuoteResponse>.None;
        }
        return response;
    }
}