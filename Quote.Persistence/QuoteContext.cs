using System.Linq.Expressions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;
using Quote.Application.Core.Abstractions.Common;
using Quote.Application.Core.Abstractions.Data;
using Quote.Domain.Core.Abstractions;
using Quote.Domain.Core.Primitives;
using Quote.Domain.Core.Primitives.Maybe;
using Quote.Persistence.Extensions;

namespace Quote.Persistence;

/// <summary>
/// Represents the applications database context.
/// </summary>
public sealed class QuoteContext(
    DbContextOptions<QuoteContext> options,
    IDateTime dateTime,
    IMediator mediator) : DbContext(options), IDbContext, IUnitOfWork
{
    /// <inheritdoc />
    public new DbSet<TEntity> Set<TEntity>()
        where TEntity : Entity
        => base.Set<TEntity>();

    /// <inheritdoc />
    public async Task<Maybe<TEntity>> GetBydIdAsync<TEntity>(Guid id, CancellationToken cancellationToken = default)
        where TEntity : Entity
        => id == Guid.Empty ?
            Maybe<TEntity>.None :
            Maybe<TEntity>.From(await Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id, cancellationToken));

    /// <inheritdoc />
    public async Task<Maybe<IEnumerable<TEntity>>> GetBulkAsync<TEntity>(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
        where TEntity : Entity
        => Maybe<IEnumerable<TEntity>>.From(await Set<TEntity>().Where(e => ids.Contains(e.Id)).ToArrayAsync(cancellationToken));

    /// <inheritdoc />
    public void Insert<TEntity>(TEntity entity)
        where TEntity : Entity
        => Set<TEntity>().Add(entity);

    /// <inheritdoc />
    public void InsertRange<TEntity>(IEnumerable<TEntity> entities)
        where TEntity : Entity
        => Set<TEntity>().AddRange(entities);

    /// <inheritdoc />
    public void UpdateRange<TEntity>(IEnumerable<TEntity> entities)
        where TEntity : Entity
        => Set<TEntity>().UpdateRange(entities);

    /// <inheritdoc />
    public new void Remove<TEntity>(TEntity entity)
        where TEntity : Entity
        => Set<TEntity>().Remove(entity);

    public void RemoveRange<TEntity>(IEnumerable<TEntity> entities)
        where TEntity : Entity
        => Set<TEntity>().RemoveRange(entities);

    /// <inheritdoc />
    public Maybe<IQueryable<TEntity>> Where<TEntity>(Expression<Func<TEntity, bool>> predicate)
        where TEntity : Entity
        => Maybe<IQueryable<TEntity>>.From(Set<TEntity>().Where(predicate));


    /// <summary>
    /// Saves all of the pending changes in the unit of work.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The number of entities that have been saved.</returns>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var utcNow = dateTime.UtcNow;

        UpdateAuditableEntities(utcNow);
        
        return await base.SaveChangesAsync(cancellationToken);
    }



    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());
        modelBuilder.ApplyUtcDateTimeConverter();
        base.OnModelCreating(modelBuilder);
    }

    /// <summary>
    /// Updates the entities implementing <see cref="IAuditableEntity"/> interface.
    /// </summary>
    /// <param name="utcNow">The current date and time in UTC format.</param>
    private async Task UpdateAuditableEntities(DateTime utcNow, CancellationToken cancellationToken = default)
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