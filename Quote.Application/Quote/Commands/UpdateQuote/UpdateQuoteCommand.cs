using Quote.Application.Core.Abstractions.Messaging;
using Quote.Domain.Core.Primitives.Result;

namespace Quote.Application.Quote.Commands.UpdateQuote;

public record UpdateQuoteCommand(
    Guid Id ,
    string Author,
    string Text,
    Guid CategoryId):ICommand<Result<Guid>>;