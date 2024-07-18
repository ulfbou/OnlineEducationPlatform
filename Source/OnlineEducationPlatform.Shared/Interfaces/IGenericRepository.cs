using OnlineEducationPlatform.Shared.Entities;
using System.Linq.Expressions;

namespace OnlineEducationPlatform.Shared.Interfaces
{
    public interface IGenericRepository<TEntity> : IGenericRepository<TEntity, int>
        where TEntity : class, IEntity
    { }
    public interface IGenericRepository<TEntity, TIdentifier>
        where TEntity : class, IEntity<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
    {
        Task<bool> CreateEntityAsync(TEntity entity, CancellationToken cancellation = default);
        Task<TEntity?> ReadEntityAsync(TIdentifier id, CancellationToken cancellation = default);
        Task<TEntity?> FindEntityAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellation = default);
        Task<bool> UpdateEntityAsync(TEntity entity, CancellationToken cancellation = default);
        Task<bool> DeleteEntityAsync(TEntity entity, CancellationToken cancellation = default);
        Task<bool> DeleteEntityAsync(TIdentifier id, CancellationToken cancellation = default);
        Task<IEnumerable<TEntity>> ReadEntitiesAsync(CancellationToken cancellation = default);
        Task<IEnumerable<TEntity>> FindEntitiesAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellation = default);
        Task<int> SaveChangesAsync(CancellationToken cancellation = default);
    }
}