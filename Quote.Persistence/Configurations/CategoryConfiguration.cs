using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quote.Domain.Entities;

namespace Quote.Persistence.Configurations;

internal sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.Property(c => c.Id).IsRequired();
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .HasMaxLength(100)
            .IsRequired();
        builder.HasIndex(c => c.Name).IsUnique();

        builder.Property(c => c.CreatedAt).IsRequired();
        builder.HasIndex(c => c.CreatedAt);
    }
}