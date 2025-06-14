using System.ComponentModel.DataAnnotations.Schema;
using Quote.Domain.Core.Primitives;
using Quote.Domain.Enumerations;
using Quote.Domain.ValueObjects;

namespace Quote.Domain.Entities;

public class Subscriber : AggregateRoot
{
    private Subscriber(){}
    
    public Subscriber(string? firstName, string? lastName, string? email, long? telegramUser, string? attachedFilePath = null) : base()
    {
       this.FirstName = firstName;
        this.LastName = lastName;
        this.Email = email;
        this.TelegramUser = telegramUser;
        this.AttachedFilePath = attachedFilePath;
        IsActive = true;
    }
    
    public void Activate()
    {
        IsActive = true;
    }
    public void Deactivate() => IsActive = false;

    [Column("first_name")] public string? FirstName { get; private set; }
    [Column("last_name")] public string? LastName { get; private set; }
    [Column("email")] public string? Email { get; private set; }
    [Column("is_active")] public bool IsActive { get; private set; }
    [Column("telegram_user")] public long? TelegramUser { get; private set; }
    [Column("attached_file_path")] public string? AttachedFilePath { get; private set; }
    public NotificationPreference PreferredNotificationMethod { get; private set; }
    

    public static Subscriber CreateWithEmail(string? email, string? firstName, string? lastName, string? attachedFilePath = null)
    {
        var subscriber = new Subscriber(firstName, lastName, email, null, attachedFilePath);
        subscriber.PreferredNotificationMethod = NotificationPreference.Email;
        return subscriber;
    }

    public static Subscriber CreateWithTelegram(long? telegramUser)
    {
        var subscriber = new Subscriber(null, null, null, telegramUser);
        subscriber.PreferredNotificationMethod = NotificationPreference.Telegram;
        return subscriber;
    }
    public Subscriber Update(string firstName, string lastName, string email)
    {
        this.FirstName = firstName;
        this.LastName = lastName;
        this.Email = email;
        return this;
    }
}