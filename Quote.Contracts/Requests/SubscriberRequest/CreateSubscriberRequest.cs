using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace Quote.Contracts.Requests.SubscriberRequest;

public record CreateSubscriberRequest
( 
    [property: JsonPropertyName("first_name")]
     string FirstName ,

    [property: JsonPropertyName("last_name")]
     string LastName ,

    [property: JsonPropertyName("email")]
     string Email ,

    [property: JsonPropertyName("telegram_user")]
     long? TelegramUser ,
    
     IFormFile? AttachedFile 
);