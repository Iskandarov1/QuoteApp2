using App.Application.Abstractions.Messaging;
using Quote.Application.Core.Abstractions.Data;
using Quote.Domain.Core.Errors;
using Quote.Domain.Core.Primitives.Result;
using Quote.Domain.Repositories;
using Quote.Domain.ValueObjects;

namespace Quote.Application.Quote.Commands.UpdateQuote;

public sealed class UpdateQuoteCommandHandler(IQuoteRepository quoteRepository, IUnitOfWork unitOfWork):ICommandHandler<UpdateQuoteCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(UpdateQuoteCommand request, CancellationToken cancellationToken)
    {
        var maybeQuote = await quoteRepository.GetByIdAsync(request.QuoteId, cancellationToken);

        if (maybeQuote.HasNoValue)
        {
            return Result.Failure<Guid>(DomainErrors.Quote.NotFound);
        }
        var author = Author.Create(request.Author);
        var text = Textt.Create(request.Text);
        var category = Category.Create(request.Category);
        
        var validation = Result.FirstFailureOrSuccess(author, text, category);
        if (validation.IsFailure)
            return Result.Failure<Guid>(validation.Error);
        
        maybeQuote.Value.Update(
            author.Value,
            text.Value,
            category.Value);
        
        quoteRepository.Update(maybeQuote.Value);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(maybeQuote.Value.Id);
    }
}