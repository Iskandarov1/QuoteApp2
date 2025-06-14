using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Quote.Contracts.Requests.QuotesRequest;

public record UpdateQuoteRequest
( 
    [property:JsonPropertyName("quote_id")]
    [Required] Guid Id ,
    
    [property:JsonPropertyName("author")]
    [Required]  string Author ,
    
    [property:JsonPropertyName("text")]
    [Required, MaxLength(400) ]  string Text ,
    
    [property:JsonPropertyName("category")]
    [Required] Guid CategoryId 
);