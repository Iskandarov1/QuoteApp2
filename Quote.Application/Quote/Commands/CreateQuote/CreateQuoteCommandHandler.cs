using App.Application.Abstractions.Messaging;
using Quote.Application.Core.Abstractions.Data;
using Quote.Domain.Core.Primitives.Result;
using Quote.Domain.Repositories;
using Quote.Domain.ValueObjects;
using QuoteEntity = Quote.Domain.Entities.Quote;
namespace Quote.Application.Quote.Commands.CreateQuote;

public class CreateQuoteCommandHandler(
    IUnitOfWork unitOfWork,
    IQuoteRepository quoteRepository
) : ICommandHandler<CreateQuoteCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateQuoteCommand request, CancellationToken cancellationToken)
    {
        var author = Author.Create(request.Author);
        var text = Textt.Create(request.Text);
        var category = Category.Create(request.Category);

        var validation = Result.FirstFailureOrSuccess(author, text, category);
        if (validation.IsFailure)
            return Result.Failure<Guid>(validation.Error);

        var quote = new QuoteEntity(
            author.Value, 
            text.Value, 
            category.Value);

        quoteRepository.Insert(quote);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(quote.Id);

    }
}