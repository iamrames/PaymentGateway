using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Infrastructure.Data;
using PaymentGateway.Domain.Helpers;

namespace PaymentGateway.Infrastructure.Repositories;
public class EntityFrameworkRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
{
    private readonly DbContext _context;

    private DbSet<TEntity> Set => _context.Set<TEntity>();

    /// <inheritdoc />
    public IQueryable<TEntity> Query => Set;

    public EntityFrameworkRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public IAsyncEnumerable<TEntity> AsAsyncEnumerable() => Set.AsAsyncEnumerable();

    /// <inheritdoc />
    public IQueryable<TEntity> AsQueryable() => Set;

    /// <inheritdoc />
    public LocalView<TEntity> Local => Set.Local;

    public IQueryable<TEntity> FromScript(string script, params object[] parameters)
    {
        var result = Set.FromSqlRaw(script, parameters);

        return result;
    }

    /// <inheritdoc />
    public async Task LoadAsync<TProperty>(
        TEntity entity,
        Expression<Func<TEntity, TProperty>> propertyExpression,
        CancellationToken cancellationToken = default)
            where TProperty : class
    {
        var type = typeof(TEntity);
        var propertyName = propertyExpression.GetMemberName();
        var property = type.GetProperty(propertyName);
        var isEnumerable = property!.PropertyType
            .GetInterfaces()
            .Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>));

        var entry = _context.Entry(entity);

        if (!isEnumerable)
        {
            var reference = entry.References.Single(r => r.Metadata.Name == propertyName);
            if (!reference.IsLoaded)
            {
                await reference.LoadAsync(cancellationToken).ConfigureAwait(false);
            }
        }
        else
        {
            var collection = entry.Collections.Single(c => c.Metadata.Name == propertyName);
            if (!collection.IsLoaded)
            {
                await collection.LoadAsync(cancellationToken).ConfigureAwait(false);
            }
        }
    }

    /// <inheritdoc />
    public async Task LoadReferenceAsync<TProperty>(
        TEntity entity,
        Expression<Func<TEntity, TProperty>> propertyExpression,
        CancellationToken cancellationToken = default)
            where TProperty : class
    {
        var reference = _context.Entry(entity).Reference(propertyExpression!);
        if (!reference.IsLoaded)
        {
            await reference.LoadAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    /// <inheritdoc />
    public async Task LoadCollectionAsync<TProperty>(
        TEntity entity,
        Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression,
        CancellationToken cancellationToken = default)
            where TProperty : class
    {
        var collection = _context.Entry(entity).Collection(propertyExpression);
        if (!collection.IsLoaded)
        {
            await collection.LoadAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    /// <inheritdoc />
    public TEntity Find(params object[] keyValues) => Set.Find(keyValues)!;

    /// <inheritdoc />
    public async ValueTask<TEntity> FindAsync(params object[] keyValues)
        => (await Set.FindAsync(keyValues).ConfigureAwait(false))!;

    /// <inheritdoc />
    public async ValueTask<TEntity> FindAsync(object[] keyValues, CancellationToken cancellationToken)
        => (await Set.FindAsync(keyValues, cancellationToken).ConfigureAwait(false))!;

    /// <inheritdoc />
    public EntityEntry<TEntity> Add(TEntity entity) => Set.Add(entity);

    /// <inheritdoc />
    public async ValueTask<EntityEntry<TEntity>> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        => await Set.AddAsync(entity, cancellationToken).ConfigureAwait(false);

    /// <inheritdoc />
    public EntityEntry<TEntity> Attach(TEntity entity) => Set.Attach(entity);

    /// <inheritdoc />
    public EntityEntry<TEntity> Remove(TEntity entity) => Set.Remove(entity);

    /// <inheritdoc />
    public EntityEntry<TEntity> Update(TEntity entity) => Set.Update(entity);

    /// <inheritdoc />
    public void AddRange(params TEntity[] entities) => Set.AddRange(entities);

    /// <inheritdoc />
    public async Task AddRangeAsync(params TEntity[] entities) => await Set.AddRangeAsync(entities).ConfigureAwait(false);

    /// <inheritdoc />
    public void AttachRange(params TEntity[] entities) => Set.AttachRange(entities);

    /// <inheritdoc />
    public void RemoveRange(params TEntity[] entities) => Set.RemoveRange(entities);

    /// <inheritdoc />
    public void UpdateRange(params TEntity[] entities) => Set.UpdateRange(entities);

    /// <inheritdoc />
    public void AddRange(IEnumerable<TEntity> entities) => Set.AddRange(entities);

    /// <inheritdoc />
    public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        => await Set.AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);

    /// <inheritdoc />
    public void AttachRange(IEnumerable<TEntity> entities) => Set.AttachRange(entities);

    /// <inheritdoc />
    public void RemoveRange(IEnumerable<TEntity> entities) => Set.RemoveRange(entities);

    /// <inheritdoc />
    public void UpdateRange(IEnumerable<TEntity> entities) => Set.UpdateRange(entities);

    ///// <inheritdoc />
    //IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator() => ((IEnumerable<TEntity>)Set).GetEnumerator();

    ///// <inheritdoc />
    //IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Set).GetEnumerator();

    ///// <inheritdoc />
    //IAsyncEnumerator<TEntity> IAsyncEnumerable<TEntity>.GetAsyncEnumerator(CancellationToken cancellationToken)
    //    => ((IAsyncEnumerable<TEntity>)Set).GetAsyncEnumerator(cancellationToken);

    ///// <inheritdoc />
    //Type IQueryable.ElementType => ((IQueryable)Set).ElementType;

    ///// <inheritdoc />
    //Expression IQueryable.Expression => ((IQueryable)Set).Expression;

    ///// <inheritdoc />
    //IQueryProvider IQueryable.Provider => ((IQueryable)Set).Provider;
}
