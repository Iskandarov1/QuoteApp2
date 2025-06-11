using System.Text.Json.Serialization;
using Quote.Contracts.Responses.QuotesResponse;

namespace Quote.Contracts.Responses.CategoriesResponse;

public sealed record CategoryResponse(
    [property : JsonPropertyName("category_id")]
    Guid CategoryId,
    
    [property : JsonPropertyName("name")]
    string Name) : BaseResponse();