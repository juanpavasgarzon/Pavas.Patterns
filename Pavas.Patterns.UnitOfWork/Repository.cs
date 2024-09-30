using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Pavas.Patterns.UnitOfWork.Contracts;
using Pavas.Patterns.UnitOfWork.Exceptions;

namespace Pavas.Patterns.UnitOfWork;

/// <summary>
/// Defines a generic repository pattern for handling data access operations on entities
/// </summary>
/// <typeparam name="TEntity">The type of the entity managed by the repository.</typeparam>
internal class Repository<TEntity>(DbContext context) : IRepository<TEntity> where TEntity : class
{
    /// <summary>
    /// Asynchronously retrieves an entity from the context based on its primary key.
    /// </summary>
    /// <typeparam name="TKey">The type of the key used to identify the entity.</typeparam>
    /// <param name="key">The key of the entity to retrieve.</param>
    /// <param name="token">Asynchronous cancellation token</param>
    /// <returns>
    /// A <see cref="Task{TEntity}"/> that represents the asynchronous operation. The task result contains the entity if found, or null if no entity with the given key is found.
    /// </returns>
    public async Task<TEntity?> GetByKeyAsync<TKey>(TKey key, CancellationToken token = new()) where TKey : notnull
    {
        return await context.Set<TEntity>().FindAsync([key], token);
    }

    /// <summary>
    /// Retrieves a single entity by its primary key.
    /// </summary>
    /// <typeparam name="TKey">The type of the primary key. Must be not null.</typeparam>
    /// <param name="key">The primary key of the entity to retrieve.</param>
    /// <returns>The entity if found; otherwise, <c>null</c>.</returns>
    public TEntity? GetByKey<TKey>(TKey key) where TKey : notnull
    {
        return context.Set<TEntity>().Find([key]);
    }

    /// <summary>
    /// Asynchronously retrieves a single record based on the provided filter.
    /// </summary>
    /// <param name="filter">The expression to filter the entity.</param>
    /// <param name="token">Asynchronous cancellation token</param>
    /// <returns>A task that represents the asynchronous operation, containing the entity if found, or null.</returns>
    public Task<TEntity?> GetOneAsync(Expression<Func<TEntity, bool>> filter, CancellationToken token = new())
    {
        return context.Set<TEntity>().Where(filter).FirstOrDefaultAsync(token);
    }

    /// <summary>
    /// Retrieves a single entity that matches the given filter expression.
    /// </summary>
    /// <param name="filter">An expression that defines the condition to filter the entity.</param>
    /// <returns>The entity if found; otherwise, <c>null</c>.</returns>
    public TEntity? GetOne(Expression<Func<TEntity, bool>> filter)
    {
        return context.Set<TEntity>().Where(filter).FirstOrDefault();
    }

    /// <summary>
    /// Asynchronously retrieves all records that match the provided filter.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation, containing a collection of matching entities.</returns>
    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken token = new())
    {
        return await context.Set<TEntity>().ToListAsync(token);
    }

    /// <summary>
    /// Retrieves all entities in the database table.
    /// </summary>
    /// <returns>An <see cref="IEnumerable{TEntity}"/> containing all the entities.</returns>
    public IEnumerable<TEntity> GetAll()
    {
        return context.Set<TEntity>().ToList();
    }

    /// <summary>
    /// Asynchronously retrieves an IQueryable to allow further query operations on the entities.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation, containing an IQueryable of the entities.</returns>
    public async Task<IQueryable<TEntity>> GetQueryAsync(CancellationToken token = new())
    {
        return await Task.Run(() => context.Set<TEntity>().AsQueryable(), token);
    }

    /// <summary>
    /// Retrieves an <see cref="IQueryable{TEntity}"/> to perform further query operations on the entities.
    /// </summary>
    /// <returns>An <see cref="IQueryable{TEntity}"/> for the entities.</returns>
    public IQueryable<TEntity> GetQuery()
    {
        return context.Set<TEntity>().AsQueryable();
    }

    /// <summary>
    /// Asynchronously adds a new entity to the context.
    /// </summary>
    /// <param name="entry">The entity to add.</param>
    /// <param name="token">Asynchronous cancellation token</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task<EntityEntry<TEntity>> AddAsync(TEntity entry, CancellationToken token = new())
    {
        return await context.Set<TEntity>().AddAsync(entry, token);
    }

    /// <summary>
    /// Adds a new entity to the context.
    /// </summary>
    /// <param name="entry">The entity to add.</param>
    /// <returns>An <see cref="EntityEntry{TEntity}"/> for the added entity.</returns>
    /// <remarks>
    /// This method tracks the entity in the context, so it will be persisted to the database on save.
    /// </remarks>
    public EntityEntry<TEntity> Add(TEntity entry)
    {
        return context.Set<TEntity>().Add(entry);
    }

    /// <summary>
    /// Asynchronously adds multiple new entities to the context.
    /// </summary>
    /// <param name="entries">The collection of entities to add.</param>
    /// <param name="token">Asynchronous cancellation token</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task AddManyAsync(IEnumerable<TEntity> entries, CancellationToken token = new())
    {
        await context.Set<TEntity>().AddRangeAsync(entries, token);
    }

    /// <summary>
    /// Adds multiple new entities to the context.
    /// </summary>
    /// <param name="entries">A collection of entities to add.</param>
    /// <remarks>
    /// This method tracks the entities in the context, so they will be persisted to the database on save.
    /// </remarks>
    public void AddMany(IEnumerable<TEntity> entries)
    {
        context.Set<TEntity>().AddRange(entries);
    }

    /// <summary>
    /// Asynchronously updates an existing entity in the context.
    /// </summary>
    /// <param name="entry">The entity to update.</param>
    /// <param name="token">Asynchronous cancellation token</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task<EntityEntry<TEntity>> UpdateAsync(TEntity entry, CancellationToken token = new())
    {
        return await Task.Run(() => context.Set<TEntity>().Update(entry), token);
    }

    /// <summary>
    /// Updates an existing entity in the context.
    /// </summary>
    /// <param name="entry">The entity to update.</param>
    /// <returns>An <see cref="EntityEntry{TEntity}"/> for the updated entity.</returns>
    /// <remarks>
    /// The entity must already be tracked by the context or attached to it before calling this method.
    /// </remarks>
    public EntityEntry<TEntity> Update(TEntity entry)
    {
        return context.Set<TEntity>().Update(entry);
    }

    /// <summary>
    /// Asynchronously removes an entity from the context based on its primary key.
    /// </summary>
    /// <typeparam name="TKey">The type of the key used to identify the entity.</typeparam>
    /// <param name="key">The key of the entity to be removed.</param>
    /// <param name="token">Asynchronous cancellation token</param>
    /// <returns>
    /// A <see cref="Task"/> that represents the asynchronous operation. The task will complete when the entity has been removed.
    /// </returns>
    public async Task<EntityEntry<TEntity>> RemoveByKeyAsync<TKey>(TKey key, CancellationToken token = new())
        where TKey : notnull
    {
        var entity = await context.Set<TEntity>().FindAsync([key], token);
        if (entity is null)
        {
            throw new NotFoundException($"Entity with key {key.ToString()} not found.");
        }

        return await Task.Run(() => context.Set<TEntity>().Remove(entity), token);
    }

    /// <summary>
    /// Removes an entity from the context by its primary key.
    /// </summary>
    /// <typeparam name="TKey">The type of the primary key. Must be not null.</typeparam>
    /// <param name="key">The primary key of the entity to remove.</param>
    /// <returns>An <see cref="EntityEntry{TEntity}"/> for the removed entity.</returns>
    /// <remarks>
    /// This method assumes that the entity is attached to the context. If the entity is not attached, a new entity will be created with the specified key and then removed.
    /// </remarks>
    public EntityEntry<TEntity> RemoveByKey<TKey>(TKey key) where TKey : notnull
    {
        var entity = context.Set<TEntity>().Find([key]);
        if (entity is null)
        {
            throw new NotFoundException($"Entity with key {key.ToString()} not found.");
        }

        return context.Set<TEntity>().Remove(entity);
    }

    /// <summary>
    /// Asynchronously removes an existing entity from the context.
    /// </summary>
    /// <param name="entry">The entity to remove.</param>
    /// <param name="token">Asynchronous cancellation token</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task<EntityEntry<TEntity>> RemoveAsync(TEntity entry, CancellationToken token = new())
    {
        return await Task.Run(() => context.Set<TEntity>().Remove(entry), token);
    }

    /// <summary>
    /// Removes an entity from the context.
    /// </summary>
    /// <param name="entry">The entity to remove.</param>
    /// <returns>An <see cref="EntityEntry{TEntity}"/> for the removed entity.</returns>
    /// <remarks>
    /// The entity must be tracked by the context to remove it. If the entity is not attached, it needs to be attached first.
    /// </remarks>
    public EntityEntry<TEntity> Remove(TEntity entry)
    {
        return context.Set<TEntity>().Remove(entry);
    }

    /// <summary>
    /// Asynchronously removes multiple entities from the context.
    /// </summary>
    /// <param name="entries">The collection of entities to remove.</param>
    /// <param name="token">Asynchronous cancellation token</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task RemoveManyAsync(IEnumerable<TEntity> entries, CancellationToken token = new())
    {
        await Task.Run(() => context.Set<TEntity>().RemoveRange(entries), token);
    }

    /// <summary>
    /// Removes multiple entities from the context.
    /// </summary>
    /// <param name="entries">A collection of entities to remove.</param>
    /// <remarks>
    /// This method removes entities from the context. These changes will be reflected in the database upon saving the context.
    /// </remarks>
    public void RemoveMany(IEnumerable<TEntity> entries)
    {
        context.Set<TEntity>().RemoveRange(entries);
    }
}