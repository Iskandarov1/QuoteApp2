using System.Text.Json.Serialization;

namespace Quote.Contracts.Requests.SubscriberRequest;

public record RemoveSubscriberRequest
( 
    [property:JsonPropertyName("email")]
     string Email ,

    [property: JsonPropertyName("telegram_user")]
     long? TelegramUser 
);