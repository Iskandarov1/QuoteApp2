using Quote.Domain.Core.Events;

namespace Quote.Domain.Events.Quotes;

public record QuoteUpdatedDomainEvent(Guid QuoteId) : IDomainEvent;
