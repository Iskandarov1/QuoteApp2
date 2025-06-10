
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

        builder.OwnsOne(quote => quote.Author, authorBuilder =>
        {
            authorBuilder.Property(author => author.Value)
                .HasColumnName("author")
                .HasMaxLength(100)
                .IsRequired();
        });

        builder.OwnsOne(quote => quote.Textt, textBuilder =>
        {
            textBuilder.Property(text => text.Value)
                .HasColumnName("quote_text")
                .HasMaxLength(400)
                .IsRequired();
        });

        builder.OwnsOne(quote => quote.Category, categoryBuilder =>
        {
            categoryBuilder.Property(category => category.Value)
                .HasColumnName("category")
                .HasMaxLength(100)
                .IsRequired();
        });

        builder.Property(quote => quote.CreatedAt)
            .IsRequired();
        
        builder.HasIndex(quote => quote.CreatedAt);
    }
}