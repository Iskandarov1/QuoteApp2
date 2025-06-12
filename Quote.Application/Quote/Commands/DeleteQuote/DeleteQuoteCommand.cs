using Quote.Application.Core.Abstractions.Messaging;
using Quote.Domain.Core.Primitives.Result;

namespace Quote.Application.Quote.Commands.DeleteQuote;

public sealed record DeleteQuoteCommand(Guid QuoteId):ICommand<Result>;