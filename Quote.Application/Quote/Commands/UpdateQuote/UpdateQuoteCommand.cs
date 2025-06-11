using App.Application.Abstractions.Messaging;
using Quote.Domain.Core.Primitives.Result;

namespace Quote.Application.Quote.Commands.UpdateQuote;

public record UpdateQuoteCommand(
    Guid QuoteId ,
    string Author,
    string Text,
    Guid CategoryId):ICommand<Result<Guid>>;