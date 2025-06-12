using Quote.Application.Core.Abstractions.Data;
using Quote.Application.Core.Abstractions.Messaging;
using Quote.Contracts.Requests.QuotesRequest;
using Quote.Domain.Core.Errors;
using Quote.Domain.Core.Localizations;
using Quote.Domain.Core.Primitives;
using Quote.Domain.Core.Primitives.Result;
using Quote.Domain.Repositories;
using Quote.Domain.ValueObjects;
using QuoteEntity = Quote.Domain.Entities.Quote;
namespace Quote.Application.Quote.Commands.CreateQuote;

public class CreateQuoteCommandHandler(
    ISharedViewLocalizer sharedViewLocalizer,
    IUnitOfWork unitOfWork,
    IQuoteRepository quoteRepository,
    ICategoryRepository categoryRepository
) : ICommandHandler<CreateQuoteCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateQuoteCommand request, CancellationToken cancellationToken)
    {
        var maybeCategory = await categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
        if (maybeCategory.HasNoValue)
            return Result.Failure<Guid>(DomainErrors.Category.NotFound);
        
        var author = Author.Create(request.Author,CaseConverter.PascalToSnakeCase(nameof(CreateQuoteRequest.Author)), sharedViewLocalizer);
        if (author.IsFailure)
            return Result.Failure<Guid>(author.Error);
        
        var text = Textt.Create(request.Text,CaseConverter.PascalToSnakeCase(nameof(CreateQuoteRequest.Text)),sharedViewLocalizer);
        if (author.IsFailure)
            return Result.Failure<Guid>(text.Error);
        
        var quote = new QuoteEntity(
            author.Value, 
            text.Value, 
            request.CategoryId);
        
        quoteRepository.Insert(quote);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(quote.Id);

    }
}