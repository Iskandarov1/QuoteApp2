using Quote.Application.Core.Abstractions.Data;
using Quote.Application.Core.Abstractions.Messaging;
using Quote.Domain.Core.Errors;
using Quote.Domain.Core.Primitives.Result;
using Quote.Domain.Repositories;

namespace Quote.Application.Quote.Commands.DeleteQuote;

public class DeleteQuoteCommandHandler(IQuoteRepository quoteRepository,IUnitOfWork unitOfWork):ICommandHandler<DeleteQuoteCommand,Result>
{
    public async Task<Result> Handle(DeleteQuoteCommand request, CancellationToken cancellationToken)
    {
        var maybeQuote = await quoteRepository.GetByIdAsync(request.QuoteId, cancellationToken);

        if (maybeQuote.HasNoValue)
        {
            return Result.Failure(DomainErrors.Quote.NotFound);
        }
        
        quoteRepository.Remove(maybeQuote.Value);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}