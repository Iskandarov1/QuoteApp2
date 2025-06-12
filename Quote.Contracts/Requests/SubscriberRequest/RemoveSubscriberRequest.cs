using System.Text.Json.Serialization;

namespace Quote.Contracts.Requests.SubscriberRequest;

public record RemoveSubscriberRequest
{
    
    [property:JsonPropertyName("email")]
    public string Email { get; set; }

    [property: JsonPropertyName("telegram_user")]
    public long? TelegramUser { get; set; }
}