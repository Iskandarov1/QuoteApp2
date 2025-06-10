using App.Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using Quote.Application.Core.Abstractions.Data;
using Quote.Contracts.Responses.QuoteResponse;
using Quote.Domain.Core.Primitives.Maybe;

namespace Quote.Application.Quote.Queries.GetByIdQuote;

public class GetQuoteByIdQueryHandler(IDbContext dbContext):IQueryHandler<GetQuoteByIdQuery,Maybe<QuoteResponse>>
{
    public async Task<Maybe<QuoteResponse>> Handle(GetQuoteByIdQuery request, CancellationToken cancellationToken)
    {
        var response = await (from quote in dbContext.Set<Domain.Entities.Quote>().AsNoTracking()
            where quote.Id == request.QuoteId
            select new QuoteResponse(
                quote.Id,
                quote.Author.Value,
                quote.Textt.Value,
                quote.Category.Value))
            .FirstOrDefaultAsync(cancellationToken);

        return response is not null ? Maybe<QuoteResponse>.From(response) : Maybe<QuoteResponse>.None;
    }
}