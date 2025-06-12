using System.Text.Json.Serialization;

namespace Quote.Contracts.Requests.SubscriberRequest;

public record CreateSubscriberRequest
{
    [property:JsonPropertyName("first_name")]
    public string FirstName { get; set; }
    
    [property:JsonPropertyName("last_name")]
    public string LastName { get; set; }
    
    [property:JsonPropertyName("email")]
    public string Email { get; set; }
    
    [property:JsonPropertyName("telegram_user")]
    
    public long? TelegramUser { get; set; }
}