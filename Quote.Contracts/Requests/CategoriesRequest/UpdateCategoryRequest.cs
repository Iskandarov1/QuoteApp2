using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Quote.Contracts.Requests.CategoriesRequest;

public record UpdateCategoryRequest
{
    [property:JsonPropertyName("category_id")]
    [Required]
    public Guid CategoryId { get; init; }
    
    [property:JsonPropertyName("name")]
    [Required, MaxLength(100)]
    public string Name { get; init; }
    
}