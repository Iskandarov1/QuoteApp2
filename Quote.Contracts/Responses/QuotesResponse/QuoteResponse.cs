using System.Text.Json.Serialization;

namespace Quote.Contracts.Responses.QuotesResponse;

public sealed record QuoteResponse(
    [property:JsonPropertyName("quote_id")] Guid Id,
    [property:JsonPropertyName("author")] string Author,
    [property:JsonPropertyName("text")] string Text,
    [property:JsonPropertyName("category_id")] Guid CategoryId,
    [property: JsonPropertyName("category_name")] string CategoryName) : BaseResponse();