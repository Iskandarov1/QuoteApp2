using System.Text.Json.Serialization;
using Quote.Contracts.Responses.QuotesResponse;

namespace Quote.Contracts.Responses.CategoriesResponse;

public sealed record CategoryResponse(
    [property : JsonPropertyName("id")]
    Guid Id,
    
    [property : JsonPropertyName("name")]
    string Name) : BaseResponse();