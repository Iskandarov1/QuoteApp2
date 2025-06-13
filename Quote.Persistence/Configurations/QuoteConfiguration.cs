
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Quote.Persistence.Configurations;

/// <summary>
/// Represents the configuration for the <see cref="Domain.Entities.Quote"/> entity.
/// </summary>
internal sealed class QuoteConfiguration : IEntityTypeConfiguration<Domain.Entities.Quote>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Quote> builder)
    {
        builder.Property(quote => quote.Id)
            .IsRequired();

        builder.HasKey(quote => quote.Id);

        builder.Property(item => item.Author)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(item => item.Textt)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(item => item.CategoryId)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasOne(q => q.Category)
            .WithMany()
            .HasForeignKey(q => q.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(quote => quote.CreatedAt)
            .IsRequired();

        builder.HasIndex(quote => quote.CreatedAt);
        
        builder.Property(q => q.IsDelete).HasDefaultValue(false);
        builder.HasIndex(q => q.IsDelete);
        
        builder.HasQueryFilter(q => !q.IsDelete);
    }
}