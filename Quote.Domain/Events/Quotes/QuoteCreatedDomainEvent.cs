using Quote.Domain.Core.Events;

namespace Quote.Domain.Events.Quotes;

public record QuoteCreatedDomainEvent(Guid QuoteId) : IDomainEvent;
