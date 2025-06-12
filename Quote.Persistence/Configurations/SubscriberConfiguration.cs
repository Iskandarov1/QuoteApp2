using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quote.Domain.Entities;

namespace Quote.Persistence.Configurations;

internal sealed class SubscriberConfiguration : IEntityTypeConfiguration<Subscriber>
{
    public void Configure(EntityTypeBuilder<Subscriber> builder)
    {
        builder.Property(item => item.Id)
            .IsRequired();
        builder.HasKey(item => item.Id);

        builder.Property(item => item.FirstName)
            .HasMaxLength(50);

        builder.Property(item => item.LastName)
            .HasMaxLength(50);

        builder.Property(item => item.Email)
            .HasMaxLength(255);

        builder.Property(item => item.AttachedFilePath)
            .HasMaxLength(500);

        builder.Property(item => item.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(item => item.TelegramUser);

        builder.Property(item => item.PreferredNotificationMethod)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(item => item.CreatedAt)
            .IsRequired();

        builder.Property(item => item.UpdatedAt);

        builder.HasIndex(item => item.Email)
            .IsUnique()
            .HasFilter("email IS NOT NULL");

        builder.HasIndex(item => item.TelegramUser)
            .IsUnique()
            .HasFilter("telegram_user IS NOT NULL");

        builder.HasIndex(item => item.CreatedAt);
    }
}