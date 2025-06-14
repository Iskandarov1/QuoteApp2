using System.Text.Json.Serialization;

namespace Quote.Contracts.Requests.CategoryRequest;

public record UpdateCategoryRequest(
    [property: JsonPropertyName("id")]
    Guid Id,
    
    [property: JsonPropertyName("name")]
    string Name
);