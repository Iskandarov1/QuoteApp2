using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Quote.Contracts.Requests.QuoteRequest;

public class UpdateQuoteRequest
{
    [property:JsonPropertyName("quote_id")]
    [Required] Guid QuoteId { get; init; }
    
    [property:JsonPropertyName("author")]
    [Required] public string Author { get; init; }
    
    [property:JsonPropertyName("text")]
    [Required, MaxLength(400) ] public string Text { get; init; }
    
    [property:JsonPropertyName("category")]
    [Required]public string Category { get; init; }
}