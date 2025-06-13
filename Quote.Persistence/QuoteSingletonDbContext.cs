using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Quote.Application.Core.Abstractions.Common;
using Quote.Domain.Core.Abstractions;
using Quote.Persistence.Extensions;

namespace Quote.Persistence;

public sealed class QuoteSingletonDbContext : DbContext
{
    private readonly IDateTime dateTime;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public QuoteSingletonDbContext(
        DbContextOptions<QuoteSingletonDbContext> options,
        IDateTime dateTime) : base(options)
    {
        this.dateTime = dateTime;
    }

    /// <summary>
    /// Thread-safe method to execute delete operations
    /// </summary>
    public async Task<int> ExecuteDeleteAsync<TEntity>(
        IQueryable<TEntity> query,
        CancellationToken cancellationToken = default)
        where TEntity : class
    {
        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            return await query.ExecuteDeleteAsync(cancellationToken);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    /// <summary>
    /// Saves all of the pending changes in the unit of work.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The number of entities that have been saved.</returns>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            var utcNow = this.dateTime.UtcNow;
            this.UpdateAuditableEntities(utcNow);
            return await base.SaveChangesAsync(cancellationToken);
        }
        finally
        {
            _semaphore.Release();
        }
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
    

    public override async ValueTask DisposeAsync()
    {
        _semaphore?.Dispose();
        await base.DisposeAsync();
    }
}