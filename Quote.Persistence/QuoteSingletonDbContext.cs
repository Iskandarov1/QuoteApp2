using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Quote.Application.Core.Abstractions.Common;
using Quote.Domain.Core.Abstractions;
using Quote.Persistence.Extensions;

namespace Quote.Persistence;

public sealed class QuoteSingletonDbContext : DbContext
{
    private readonly IDateTime dateTime;

    public QuoteSingletonDbContext(
        DbContextOptions<QuoteSingletonDbContext> options,
        IDateTime dateTime) : base(options)
    {
        this.dateTime = dateTime;
    }

    /// <summary>
    /// Saves all of the pending changes in the unit of work.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The number of entities that have been saved.</returns>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var utcNow = this.dateTime.UtcNow;

        this.UpdateAuditableEntities(utcNow);

        return await base.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());

        modelBuilder.ApplyUtcDateTimeConverter();
        base.OnModelCreating(modelBuilder);
    }

    private void UpdateAuditableEntities(DateTime utcNow)
    {
        foreach (EntityEntry<IAuditableEntity> entityEntry in ChangeTracker.Entries<IAuditableEntity>())
        {
            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Property(nameof(IAuditableEntity.CreatedAt)).CurrentValue = utcNow;
            }

            if (entityEntry.State == EntityState.Modified)
            {
                entityEntry.Property(nameof(IAuditableEntity.UpdatedAt)).CurrentValue = utcNow;
            }
        }
    }
}