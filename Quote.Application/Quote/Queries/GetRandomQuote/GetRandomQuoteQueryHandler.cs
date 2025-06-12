using Microsoft.EntityFrameworkCore;
using Quote.Application.Core.Abstractions.Data;
using Quote.Application.Core.Abstractions.Messaging;
using Quote.Contracts.Responses.QuotesResponse;
using Quote.Domain.Core.Primitives.Maybe;

namespace Quote.Application.Quote.Queries.GetRandomQuote;

public class GetRandomQuoteQueryHandler(IDbContext dbContext) : IQueryHandler<GetRandomQuoteQuery,Maybe<QuoteResponse>>
{
    public async Task<Maybe<QuoteResponse>> Handle(GetRandomQuoteQuery request, CancellationToken cancellationToken)
    {
        var response = await (from quote in dbContext.Set<Domain.Entities.Quote>().AsNoTracking()
            join Category in dbContext.Set<Domain.Entities.Category>() on quote.CategoryId equals Category.Id 
            orderby quote.CreatedAt descending
            select new QuoteResponse(
                quote.Id,
                quote.Author,
                quote.Textt,
                quote.CategoryId,
                Category.Name)).ToListAsync();
        
        if (!response.Any())
            return Maybe<QuoteResponse>.None;

        var randomIndex = new Random().Next(response.Count);
        var q = response[randomIndex];

       

        return q;


    }
}