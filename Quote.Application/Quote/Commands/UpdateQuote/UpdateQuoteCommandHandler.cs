using App.Application.Abstractions.Messaging;
using Quote.Application.Core.Abstractions.Data;
using Quote.Contracts.Requests.QuotesRequest;
using Quote.Domain.Core.Errors;
using Quote.Domain.Core.Localizations;
using Quote.Domain.Core.Primitives;
using Quote.Domain.Core.Primitives.Result;
using Quote.Domain.Repositories;
using Quote.Domain.ValueObjects;

namespace Quote.Application.Quote.Commands.UpdateQuote;

public sealed class UpdateQuoteCommandHandler(
    IQuoteRepository quoteRepository,
    IUnitOfWork unitOfWork,
    ISharedViewLocalizer sharedViewLocalizer,
    ICategoryRepository categoryRepository):ICommandHandler<UpdateQuoteCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(UpdateQuoteCommand request, CancellationToken cancellationToken)
    {
        var maybeQuote = await quoteRepository.GetByIdAsync(request.QuoteId, cancellationToken);
        
        var maybeCategory = await categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
        if (maybeCategory.HasNoValue)
            return Result.Failure<Guid>(DomainErrors.Category.NotFound);
        
        if (maybeQuote.HasNoValue)
        {
            return Result.Failure<Guid>(DomainErrors.Quote.NotFound);
        }
        var author = Author.Create(request.Author,CaseConverter.PascalToSnakeCase(nameof(UpdateQuoteRequest.Author)), sharedViewLocalizer);
        if (author.IsFailure)
            return Result.Failure<Guid>(author.Error);
        
        var text = Textt.Create(request.Text,CaseConverter.PascalToSnakeCase(nameof(UpdateQuoteRequest.Text)), sharedViewLocalizer);
        if (text.IsFailure)
            return Result.Failure<Guid>(text.Error);
        
        
        
        quoteRepository.Update(
            maybeQuote.Value.Update(
                author.Value,
                text.Value,
                request.CategoryId));

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(maybeQuote.Value.Id);
    }
}