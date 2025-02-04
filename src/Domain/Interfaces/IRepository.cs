using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace PaymentGateway.Domain.Interfaces;
public interface IRepository<TEntity>
       where TEntity : class
{
    /// <summary>
    /// Gets the query.
    /// </summary>
    IQueryable<TEntity> Query { get; }

    /// <summary>
    /// <para>
    ///     Returns this object typed as <see cref="IAsyncEnumerable{T}" />.
    /// </para>
    /// <para>
    ///     This is a convenience method to help with disambiguation of extension methods in the same
    ///     namespace that extend both interfaces.
    /// </para>
    /// </summary>
    /// <returns> This object. </returns>
    IAsyncEnumerable<TEntity> AsAsyncEnumerable();

    /// <summary>
    /// <para>
    ///     Returns this object typed as <see cref="IQueryable{T}" />.
    /// </para>
    /// <para>
    ///     This is a convenience method to help with disambiguation of extension methods in the same
    ///     namespace that extend both interfaces.
    /// </para>
    /// </summary>
    /// <returns> This object. </returns>
    IQueryable<TEntity> AsQueryable();

    /// <summary>
    /// <para>
    ///     Gets an <see cref="LocalView{TEntity}" /> that represents a local view of all Added, Unchanged,
    ///     and Modified entities in this set.
    /// </para>
    /// <para>
    ///     This local view will stay in sync as entities are added or removed from the context. Likewise, entities
    ///     added to or removed from the local view will automatically be added to or removed
    ///     from the context.
    /// </para>
    /// <para>
    ///     This property can be used for data binding by populating the set with data, for example by using the
    ///     <see cref="EntityFrameworkQueryableExtensions.Load{TSource}" /> extension method,
    ///     and then binding to the local data through this property by calling
    ///     <see cref="LocalView{TEntity}.ToObservableCollection" /> for WPF binding, or
    ///     <see cref="LocalView{TEntity}.ToBindingList" /> for WinForms.
    /// </para>
    /// </summary>
    LocalView<TEntity> Local { get; }

    /// <summary>
    /// Creates a LINQ query based on a raw script query.
    /// </summary>
    /// <param name="script">The raw script query.</param>
    /// <param name="parameters">The values to be assigned to parameters.</param>
    /// <returns>An <see cref="IQueryable{T}" /> representing the raw script query.</returns>
    IQueryable<TEntity> FromScript(string script, params object[] parameters);

    /// <summary>
    /// Loads entity navigation property.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <param name="propertyExpression">A lambda expression representing the property to load.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    Task LoadAsync<TProperty>(
        TEntity entity,
        Expression<Func<TEntity, TProperty>> propertyExpression,
        CancellationToken cancellationToken = default)
            where TProperty : class;

    /// <summary>
    /// Loads entity navigation property.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <param name="propertyExpression">A lambda expression representing the property to load.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    Task LoadReferenceAsync<TProperty>(
        TEntity entity,
        Expression<Func<TEntity, TProperty>> propertyExpression,
        CancellationToken cancellationToken = default)
            where TProperty : class;

    /// <summary>
    /// Loads entity navigation property.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <param name="propertyExpression">A lambda expression representing the property to load.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    Task LoadCollectionAsync<TProperty>(
        TEntity entity,
        Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression,
        CancellationToken cancellationToken = default)
            where TProperty : class;

    /// <summary>
    /// Finds an entity with the given primary key values. If an entity with the given primary key values
    /// is being tracked by the context, then it is returned immediately without making a request to the
    /// database. Otherwise, a query is made to the database for an entity with the given primary key values
    /// and this entity, if found, is attached to the context and returned. If no entity is found, then
    /// null is returned.
    /// </summary>
    /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
    /// <returns>The entity found, or null.</returns>
    TEntity Find(params object[] keyValues);

    /// <summary>
    /// Finds an entity with the given primary key values. If an entity with the given primary key values
    /// is being tracked by the context, then it is returned immediately without making a request to the
    /// database. Otherwise, a query is made to the database for an entity with the given primary key values
    /// and this entity, if found, is attached to the context and returned. If no entity is found, then
    /// null is returned.
    /// </summary>
    /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
    /// <returns>The entity found, or null.</returns>
    ValueTask<TEntity> FindAsync(params object[] keyValues);

    /// <summary>
    /// Finds an entity with the given primary key values. If an entity with the given primary key values
    /// is being tracked by the context, then it is returned immediately without making a request to the
    /// database. Otherwise, a query is made to the database for an entity with the given primary key values
    /// and this entity, if found, is attached to the context and returned. If no entity is found, then
    /// null is returned.
    /// </summary>
    /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>The entity found, or null.</returns>
    ValueTask<TEntity> FindAsync(object[] keyValues, CancellationToken cancellationToken);

    /// <summary>
    /// <para>
    ///     Begins tracking the given entity, and any other reachable entities that are
    ///     not already being tracked, in the <see cref="EntityState.Added" /> state such that they will
    ///     be inserted into the database when <see cref="DbContext.SaveChanges()" /> is called.
    /// </para>
    /// <para>
    ///     Use <see cref="EntityEntry.State" /> to set the state of only a single entity.
    /// </para>
    /// </summary>
    /// <param name="entity"> The entity to add. </param>
    /// <returns>
    /// The <see cref="EntityEntry{TEntity}" /> for the entity. The entry provides
    /// access to change tracking information and operations for the entity.
    /// </returns>
    EntityEntry<TEntity> Add(TEntity entity);

    /// <summary>
    /// <para>
    ///     Begins tracking the given entity, and any other reachable entities that are
    ///     not already being tracked, in the <see cref="EntityState.Added" /> state such that they will
    ///     be inserted into the database when <see cref="DbContext.SaveChanges()" /> is called.
    /// </para>
    /// <para>
    ///     This method is async only to allow special value generators, such as the one used by
    ///     'Microsoft.EntityFrameworkCore.Metadata.SqlServerValueGenerationStrategy.SequenceHiLo',
    ///     to access the database asynchronously. For all other cases the non async method should be used.
    /// </para>
    /// <para>
    ///     Use <see cref="EntityEntry.State" /> to set the state of only a single entity.
    /// </para>
    /// </summary>
    /// <param name="entity"> The entity to add. </param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous Add operation. The task result contains the
    /// <see cref="EntityEntry{TEntity}" /> for the entity. The entry provides access to change tracking
    /// information and operations for the entity.
    /// </returns>
    ValueTask<EntityEntry<TEntity>> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// <para>
    ///     Begins tracking the given entity and entries reachable from the given entity using
    ///     the <see cref="EntityState.Unchanged" /> state by default, but see below for cases
    ///     when a different state will be used.
    /// </para>
    /// <para>
    ///     Generally, no database interaction will be performed until <see cref="DbContext.SaveChanges()" /> is called.
    /// </para>
    /// <para>
    ///     A recursive search of the navigation properties will be performed to find reachable entities
    ///     that are not already being tracked by the context. All entities found will be tracked
    ///     by the context.
    /// </para>
    /// <para>
    ///     For entity types with generated keys if an entity has its primary key value set
    ///     then it will be tracked in the <see cref="EntityState.Unchanged" /> state. If the primary key
    ///     value is not set then it will be tracked in the <see cref="EntityState.Added" /> state.
    ///     This helps ensure only new entities will be inserted.
    ///     An entity is considered to have its primary key value set if the primary key property is set
    ///     to anything other than the CLR default for the property type.
    /// </para>
    /// <para>
    ///     For entity types without generated keys, the state set is always <see cref="EntityState.Unchanged" />.
    /// </para>
    /// <para>
    ///     Use <see cref="EntityEntry.State" /> to set the state of only a single entity.
    /// </para>
    /// </summary>
    /// <param name="entity"> The entity to attach. </param>
    /// <returns>
    /// The <see cref="EntityEntry" /> for the entity. The entry provides
    /// access to change tracking information and operations for the entity.
    /// </returns>
    EntityEntry<TEntity> Attach(TEntity entity);

    /// <summary>
    /// Begins tracking the given entity in the <see cref="EntityState.Deleted" /> state such that it will
    /// be removed from the database when <see cref="DbContext.SaveChanges()" /> is called.
    /// </summary>
    /// <remarks>
    /// <para>
    ///     If the entity is already tracked in the <see cref="EntityState.Added" /> state then the context will
    ///     stop tracking the entity (rather than marking it as <see cref="EntityState.Deleted" />) since the
    ///     entity was previously added to the context and does not exist in the database.
    /// </para>
    /// <para>
    ///     Any other reachable entities that are not already being tracked will be tracked in the same way that
    ///     they would be if <see cref="Attach(TEntity)" /> was called before calling this method.
    ///     This allows any cascading actions to be applied when <see cref="DbContext.SaveChanges()" /> is called.
    /// </para>
    /// <para>
    ///     Use <see cref="EntityEntry.State" /> to set the state of only a single entity.
    /// </para>
    /// </remarks>
    /// <param name="entity"> The entity to remove. </param>
    /// <returns>
    /// The <see cref="EntityEntry{TEntity}" /> for the entity. The entry provides
    /// access to change tracking information and operations for the entity.
    /// </returns>
    EntityEntry<TEntity> Remove(TEntity entity);

    /// <summary>
    /// <para>
    ///     Begins tracking the given entity and entries reachable from the given entity using
    ///     the <see cref="EntityState.Modified" /> state by default, but see below for cases
    ///     when a different state will be used.
    /// </para>
    /// <para>
    ///     Generally, no database interaction will be performed until <see cref="DbContext.SaveChanges()" /> is called.
    /// </para>
    /// <para>
    ///     A recursive search of the navigation properties will be performed to find reachable entities
    ///     that are not already being tracked by the context. All entities found will be tracked
    ///     by the context.
    /// </para>
    /// <para>
    ///     For entity types with generated keys if an entity has its primary key value set
    ///     then it will be tracked in the <see cref="EntityState.Modified" /> state. If the primary key
    ///     value is not set then it will be tracked in the <see cref="EntityState.Added" /> state.
    ///     This helps ensure new entities will be inserted, while existing entities will be updated.
    ///     An entity is considered to have its primary key value set if the primary key property is set
    ///     to anything other than the CLR default for the property type.
    /// </para>
    /// <para>
    ///     For entity types without generated keys, the state set is always <see cref="EntityState.Modified" />.
    /// </para>
    /// <para>
    ///     Use <see cref="EntityEntry.State" /> to set the state of only a single entity.
    /// </para>
    /// </summary>
    /// <param name="entity"> The entity to update. </param>
    /// <returns>
    /// The <see cref="EntityEntry" /> for the entity. The entry provides
    /// access to change tracking information and operations for the entity.
    /// </returns>
    EntityEntry<TEntity> Update(TEntity entity);

    /// <summary>
    /// Begins tracking the given entities, and any other reachable entities that are
    /// not already being tracked, in the <see cref="EntityState.Added" /> state such that they will
    /// be inserted into the database when <see cref="DbContext.SaveChanges()" /> is called.
    /// </summary>
    /// <param name="entities"> The entities to add. </param>
    void AddRange(params TEntity[] entities);

    /// <summary>
    /// <para>
    ///     Begins tracking the given entities, and any other reachable entities that are
    ///     not already being tracked, in the <see cref="EntityState.Added" /> state such that they will
    ///     be inserted into the database when <see cref="DbContext.SaveChanges()" /> is called.
    /// </para>
    /// <para>
    ///     This method is async only to allow special value generators, such as the one used by
    ///     'Microsoft.EntityFrameworkCore.Metadata.SqlServerValueGenerationStrategy.SequenceHiLo',
    ///     to access the database asynchronously. For all other cases the non async method should be used.
    /// </para>
    /// </summary>
    /// <param name="entities"> The entities to add. </param>
    /// <returns> A task that represents the asynchronous operation. </returns>
    Task AddRangeAsync(params TEntity[] entities);

    /// <summary>
    /// <para>
    ///     Begins tracking the given entities and entries reachable from the given entities using
    ///     the <see cref="EntityState.Unchanged" /> state by default, but see below for cases
    ///     when a different state will be used.
    /// </para>
    /// <para>
    ///     Generally, no database interaction will be performed until <see cref="DbContext.SaveChanges()" /> is called.
    /// </para>
    /// <para>
    ///     A recursive search of the navigation properties will be performed to find reachable entities
    ///     that are not already being tracked by the context. All entities found will be tracked
    ///     by the context.
    /// </para>
    /// <para>
    ///     For entity types with generated keys if an entity has its primary key value set
    ///     then it will be tracked in the <see cref="EntityState.Unchanged" /> state. If the primary key
    ///     value is not set then it will be tracked in the <see cref="EntityState.Added" /> state.
    ///     This helps ensure only new entities will be inserted.
    ///     An entity is considered to have its primary key value set if the primary key property is set
    ///     to anything other than the CLR default for the property type.
    /// </para>
    /// <para>
    ///     For entity types without generated keys, the state set is always <see cref="EntityState.Unchanged" />.
    /// </para>
    /// <para>
    ///     Use <see cref="EntityEntry.State" /> to set the state of only a single entity.
    /// </para>
    /// </summary>
    /// <param name="entities"> The entities to attach. </param>
    void AttachRange(params TEntity[] entities);

    /// <summary>
    /// Begins tracking the given entities in the <see cref="EntityState.Deleted" /> state such that they will
    /// be removed from the database when <see cref="DbContext.SaveChanges()" /> is called.
    /// </summary>
    /// <remarks>
    /// <para>
    ///     If any of the entities are already tracked in the <see cref="EntityState.Added" /> state then the context will
    ///     stop tracking those entities (rather than marking them as <see cref="EntityState.Deleted" />) since those
    ///     entities were previously added to the context and do not exist in the database.
    /// </para>
    /// <para>
    ///     Any other reachable entities that are not already being tracked will be tracked in the same way that
    ///     they would be if <see cref="AttachRange(TEntity[])" /> was called before calling this method.
    ///     This allows any cascading actions to be applied when <see cref="DbContext.SaveChanges()" /> is called.
    /// </para>
    /// </remarks>
    /// <param name="entities"> The entities to remove. </param>
    void RemoveRange(params TEntity[] entities);

    /// <summary>
    /// <para>
    ///     Begins tracking the given entities and entries reachable from the given entities using
    ///     the <see cref="EntityState.Modified" /> state by default, but see below for cases
    ///     when a different state will be used.
    /// </para>
    /// <para>
    ///     Generally, no database interaction will be performed until <see cref="DbContext.SaveChanges()" /> is called.
    /// </para>
    /// <para>
    ///     A recursive search of the navigation properties will be performed to find reachable entities
    ///     that are not already being tracked by the context. All entities found will be tracked
    ///     by the context.
    /// </para>
    /// <para>
    ///     For entity types with generated keys if an entity has its primary key value set
    ///     then it will be tracked in the <see cref="EntityState.Modified" /> state. If the primary key
    ///     value is not set then it will be tracked in the <see cref="EntityState.Added" /> state.
    ///     This helps ensure new entities will be inserted, while existing entities will be updated.
    ///     An entity is considered to have its primary key value set if the primary key property is set
    ///     to anything other than the CLR default for the property type.
    /// </para>
    /// <para>
    ///     For entity types without generated keys, the state set is always <see cref="EntityState.Modified" />.
    /// </para>
    /// <para>
    ///     Use <see cref="EntityEntry.State" /> to set the state of only a single entity.
    /// </para>
    /// </summary>
    /// <param name="entities"> The entities to update. </param>
    void UpdateRange(params TEntity[] entities);

    /// <summary>
    /// Begins tracking the given entities, and any other reachable entities that are
    /// not already being tracked, in the <see cref="EntityState.Added" /> state such that they will
    /// be inserted into the database when <see cref="DbContext.SaveChanges()" /> is called.
    /// </summary>
    /// <param name="entities"> The entities to add. </param>
    void AddRange(IEnumerable<TEntity> entities);

    /// <summary>
    /// <para>
    ///     Begins tracking the given entities, and any other reachable entities that are
    ///     not already being tracked, in the <see cref="EntityState.Added" /> state such that they will
    ///     be inserted into the database when <see cref="DbContext.SaveChanges()" /> is called.
    /// </para>
    /// <para>
    ///     This method is async only to allow special value generators, such as the one used by
    ///     'Microsoft.EntityFrameworkCore.Metadata.SqlServerValueGenerationStrategy.SequenceHiLo',
    ///     to access the database asynchronously. For all other cases the non async method should be used.
    /// </para>
    /// </summary>
    /// <param name="entities"> The entities to add. </param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns> A task that represents the asynchronous operation. </returns>
    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// <para>
    ///     Begins tracking the given entities and entries reachable from the given entities using
    ///     the <see cref="EntityState.Unchanged" /> state by default, but see below for cases
    ///     when a different state will be used.
    /// </para>
    /// <para>
    ///     Generally, no database interaction will be performed until <see cref="DbContext.SaveChanges()" /> is called.
    /// </para>
    /// <para>
    ///     A recursive search of the navigation properties will be performed to find reachable entities
    ///     that are not already being tracked by the context. All entities found will be tracked
    ///     by the context.
    /// </para>
    /// <para>
    ///     For entity types with generated keys if an entity has its primary key value set
    ///     then it will be tracked in the <see cref="EntityState.Unchanged" /> state. If the primary key
    ///     value is not set then it will be tracked in the <see cref="EntityState.Added" /> state.
    ///     This helps ensure only new entities will be inserted.
    ///     An entity is considered to have its primary key value set if the primary key property is set
    ///     to anything other than the CLR default for the property type.
    /// </para>
    /// <para>
    ///     For entity types without generated keys, the state set is always <see cref="EntityState.Unchanged" />.
    /// </para>
    /// <para>
    ///     Use <see cref="EntityEntry.State" /> to set the state of only a single entity.
    /// </para>
    /// </summary>
    /// <param name="entities"> The entities to attach. </param>
    void AttachRange(IEnumerable<TEntity> entities);

    /// <summary>
    /// Begins tracking the given entities in the <see cref="EntityState.Deleted" /> state such that they will
    /// be removed from the database when <see cref="DbContext.SaveChanges()" /> is called.
    /// </summary>
    /// <remarks>
    /// <para>
    ///     If any of the entities are already tracked in the <see cref="EntityState.Added" /> state then the context will
    ///     stop tracking those entities (rather than marking them as <see cref="EntityState.Deleted" />) since those
    ///     entities were previously added to the context and do not exist in the database.
    /// </para>
    /// <para>
    ///     Any other reachable entities that are not already being tracked will be tracked in the same way that
    ///     they would be if <see cref="AttachRange(IEnumerable{TEntity})" /> was called before calling this method.
    ///     This allows any cascading actions to be applied when <see cref="DbContext.SaveChanges()" /> is called.
    /// </para>
    /// </remarks>
    /// <param name="entities"> The entities to remove. </param>
    void RemoveRange(IEnumerable<TEntity> entities);

    /// <summary>
    /// <para>
    ///     Begins tracking the given entities and entries reachable from the given entities using
    ///     the <see cref="EntityState.Modified" /> state by default, but see below for cases
    ///     when a different state will be used.
    /// </para>
    /// <para>
    ///     Generally, no database interaction will be performed until <see cref="DbContext.SaveChanges()" /> is called.
    /// </para>
    /// <para>
    ///     A recursive search of the navigation properties will be performed to find reachable entities
    ///     that are not already being tracked by the context. All entities found will be tracked
    ///     by the context.
    /// </para>
    /// <para>
    ///     For entity types with generated keys if an entity has its primary key value set
    ///     then it will be tracked in the <see cref="EntityState.Modified" /> state. If the primary key
    ///     value is not set then it will be tracked in the <see cref="EntityState.Added" /> state.
    ///     This helps ensure new entities will be inserted, while existing entities will be updated.
    ///     An entity is considered to have its primary key value set if the primary key property is set
    ///     to anything other than the CLR default for the property type.
    /// </para>
    /// <para>
    ///     For entity types without generated keys, the state set is always <see cref="EntityState.Modified" />.
    /// </para>
    /// <para>
    ///     Use <see cref="EntityEntry.State" /> to set the state of only a single entity.
    /// </para>
    /// </summary>
    /// <param name="entities"> The entities to update. </param>
    void UpdateRange(IEnumerable<TEntity> entities);
}
