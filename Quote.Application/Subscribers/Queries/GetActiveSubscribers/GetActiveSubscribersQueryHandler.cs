using Microsoft.EntityFrameworkCore;
using Quote.Application.Core.Abstractions.Data;
using Quote.Application.Core.Abstractions.Messaging;
using Quote.Contracts.Common;
using Quote.Domain.Core.Primitives.Maybe;
using Quote.Domain.Entities;

namespace Quote.Application.Subscribers.Queries.GetActiveSubscribers;

public class GetActiveSubscribersQueryHandler(IDbContext dbContext) : IQueryHandler<GetActiveSubscribersQuery,Maybe<PagedList<Subscriber>>>
{
    public async Task<Maybe<PagedList<Subscriber>>> Handle(GetActiveSubscribersQuery request, CancellationToken cancellationToken)
    {
        var maybeSubscriber = dbContext.Set<Domain.Entities.Subscriber>().AsNoTracking()
            .Where(s => s.IsActive); 
        
        int totalCount = await maybeSubscriber.CountAsync(cancellationToken);
        
        var responsesPage = await maybeSubscriber
            .Skip(request.Page < 1 ? 0 : (request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);
        
        return new PagedList<Subscriber>(responsesPage,request.Page,request.PageSize,totalCount);


      
    }
}