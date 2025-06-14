using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Quote.Contracts.Requests.CategoryRequest;

public sealed record CreateCategoryRequest
(
    [property:JsonPropertyName("name")]
    [Required, MaxLength(100)]
     string Name 
);