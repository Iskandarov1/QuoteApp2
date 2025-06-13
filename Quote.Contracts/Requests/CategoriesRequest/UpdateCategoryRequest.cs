using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Quote.Contracts.Requests.CategoriesRequest;

public record UpdateCategoryRequest(
    [property: JsonPropertyName("id")]
    Guid Id,
    
    [property: JsonPropertyName("name")]
    string Name
);