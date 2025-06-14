using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Quote.Contracts.Requests.QuotesRequest;

public record CreateQuoteRequest
( 
    [property:JsonPropertyName("author")]
    [Required]  string Author ,
    
    [property:JsonPropertyName("text")]
    [Required, MaxLength(400) ]
     string Text ,
    
    [property:JsonPropertyName("category")]
    [Required]  Guid CategoryId 
);