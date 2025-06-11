using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Quote.Contracts.Requests.CategoriesRequest;

public sealed record CreateCategoryRequest
{
    [property:JsonPropertyName("name")]
    [Required, MaxLength(100)]
    public string Name { get; init; }
}