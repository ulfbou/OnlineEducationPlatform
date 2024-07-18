using Microsoft.AspNetCore.Mvc;
using OnlineEducationPlatform.Shared.Entities;

namespace OnlineEducationPlatform.Shared.Interfaces
{
    public interface IGenericService<TEntity> : IGenericService<TEntity, int>
        where TEntity : class, IEntity
    { }
    public interface IGenericService<TEntity, TIdentifier>
        where TEntity : class, IEntity<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
    {
        Task<IActionResult> CreateEntityAsync<TCreateEntityDTO>(TCreateEntityDTO dto);
        Task<IActionResult> ReadEntityAsync<TReadEntityDTO>(TIdentifier id);
        Task<IActionResult> UpdateEntityAsync<TUpdateEntityDTO, TUpdatedEntityDTO>(TUpdateEntityDTO dto);
        Task<IActionResult> DeleteEntityAsync(TIdentifier id);
        Task<IActionResult> DeleteEntityAsync(TEntity entity);
        Task<IActionResult> ReadEntitiesAsync<TReadEntityDTO>();
    }
}