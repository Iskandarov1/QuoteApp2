using Quote.Domain.Core.Primitives;

namespace Quote.Domain.Enumerations;

public sealed class NotificationPreference : Enumeration<NotificationPreference>
{
    public static readonly NotificationPreference Email = new NotificationPreference(1, "Email");
    public static readonly NotificationPreference Telegram = new NotificationPreference(2, "Telegram");
    
    private NotificationPreference(short value, string name)
        : base(value, name)
    {
    }

 
}