using App.Application.Abstractions.Messaging;
using Quote.Domain.Core.Primitives.Result;

namespace Quote.Application.Quote.Commands.CreateQuote;

public sealed record CreateQuoteCommand(

    string Author,
    string Text,
    string Category) : ICommand<Result<Guid>>;
