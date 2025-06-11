using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Quote.Contracts.Requests.QuotesRequest;

public record UpdateQuoteRequest
{
    [property:JsonPropertyName("quote_id")]
    [Required] Guid QuoteId { get; init; }
    
    [property:JsonPropertyName("author")]
    [Required] public string Author { get; init; }
    
    [property:JsonPropertyName("text")]
    [Required, MaxLength(400) ] public string Text { get; init; }
    
    [property:JsonPropertyName("category")]
    [Required]public Guid CategoryId { get; init; }
}