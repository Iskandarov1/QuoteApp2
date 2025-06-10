using System.Text.Json.Serialization;

namespace Quote.Contracts.Responses.QuoteResponse;

public sealed record QuoteResponse(
    [property:JsonPropertyName("quote_id")] Guid QuoteId,
    [property:JsonPropertyName("author")] string Author,
    [property:JsonPropertyName("text")] string Text,
    [property:JsonPropertyName("category")] string Category);