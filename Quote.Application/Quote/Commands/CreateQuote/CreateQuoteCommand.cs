using App.Application.Abstractions.Messaging;
using Quote.Domain.Core.Primitives.Result;

namespace Quote.Application.Quote.Commands.CreateQuote;

public sealed record CreateQuoteCommand(

    string Author,
    string Text,
    Guid CategoryId) : ICommand<Result<Guid>>;
