using Microsoft.EntityFrameworkCore;
using OnlineEducationPlatform.Persistence.Data;
using OnlineEducationPlatform.Shared.Entities;
using OnlineEducationPlatform.Shared.Interfaces;
using System;
using System.Linq.Expressions;

namespace OnlineEducationPlatform.Persistence.Repository
{
    public class GenericRepository<TEntity>(ApplicationDbContext context)
        : GenericRepository<TEntity, int>(context), IGenericRepository<TEntity>
        where TEntity : class, IEntity
    { }
    public class GenericRepository<TEntity, TIdentifier>(ApplicationDbContext context)
        : IGenericRepository<TEntity, TIdentifier>
        where TEntity : class, IEntity<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
    {
        private readonly ApplicationDbContext _context = context ?? throw new InvalidOperationException(nameof(context));
        private readonly DbSet<TEntity> _entities = context?.Set<TEntity>()
            ?? throw new InvalidOperationException("db set is null");

        public async Task<bool> CreateEntityAsync(TEntity entity, CancellationToken cancellation = default)
        {
            try
            {
                cancellation.ThrowIfCancellationRequested();
                await _entities.AddAsync(entity);
                return (await _context.SaveChangesAsync(cancellation)) > 0;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<TEntity?> ReadEntityAsync(TIdentifier id, CancellationToken cancellation = default)
        {
            try
            {
                cancellation.ThrowIfCancellationRequested();
                return await _entities.FindAsync(new object[] { id }, cancellation);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> UpdateEntityAsync(TEntity entity, CancellationToken cancellation = default)
        {
            try
            {
                cancellation.ThrowIfCancellationRequested();
                _entities.Update(entity);
                return (await _context.SaveChangesAsync(cancellation)) > 0;

            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteEntityAsync(TIdentifier identifier, CancellationToken cancellation = default)
        {
            try
            {
                var entity = await _entities.FirstOrDefaultAsync(e => e.Id.Equals(identifier), cancellation);
                if (entity is null) return false;
                return await DeleteEntityAsync(entity, cancellation);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteEntityAsync(TEntity entity, CancellationToken cancellation = default)
        {
            ArgumentNullException.ThrowIfNull(entity);
            bool result;
            try
            {
                _context.Remove(entity);
                return await _context.SaveChangesAsync(cancellation) > 0;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        public async Task<IEnumerable<TEntity>> ReadEntitiesAsync(CancellationToken cancellation = default)
        {
            try
            {
                cancellation.ThrowIfCancellationRequested();
                return await _entities.ToListAsync(cancellation);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception)
            {
                return new List<TEntity>();
            }
        }

        public async Task<TEntity?> FindEntityAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellation = default)
        {
            ArgumentNullException.ThrowIfNull(predicate);
            try
            {
                cancellation.ThrowIfCancellationRequested();
                return await _context.Set<TEntity>().FirstOrDefaultAsync(predicate, cancellation);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<TEntity>> FindEntitiesAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellation = default)
        {
            ArgumentNullException.ThrowIfNull(predicate);
            try
            {
                cancellation.ThrowIfCancellationRequested();
                return await _context.Set<TEntity>().Where(predicate).ToListAsync(cancellation);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch
            {
                return Enumerable.Empty<TEntity>();
            }
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellation = default)
        {
            try
            {
                cancellation.ThrowIfCancellationRequested();
                return await _context.SaveChangesAsync(cancellation);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}