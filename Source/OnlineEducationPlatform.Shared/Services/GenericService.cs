using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineEducationPlatform.Shared.Entities;
using OnlineEducationPlatform.Shared.Interfaces;

namespace OnlineEducationPlatform.Shared.Services
{
    public class GenericService<TEntity>(
        IGenericRepository<TEntity, int> repository,
        ILogger<GenericService<TEntity, int>> logger,
        IMapper mapper)
        : GenericService<TEntity, int>(repository, logger, mapper)
        where TEntity : Entity<int>, IEntity<int>
    { }
    public class GenericService<TEntity, TIdentifier>(
        IGenericRepository<TEntity, TIdentifier> repository,
        ILogger<GenericService<TEntity, TIdentifier>> logger,
        IMapper mapper)
        : BaseService, IGenericService<TEntity, TIdentifier>
        where TEntity : class, IEntity<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
    {
        protected IGenericRepository<TEntity, TIdentifier> _repository = repository
            ?? throw new ArgumentNullException(nameof(repository));
        protected ILogger<GenericService<TEntity, TIdentifier>> _logger = logger
            ?? throw new ArgumentNullException(nameof(logger));
        protected IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        protected string _entityName = typeof(TEntity).Name;

        public async Task<IActionResult> CreateEntityAsync<TCreateEntityDTO>(TCreateEntityDTO dto)
        {
            ArgumentNullException.ThrowIfNull(dto, nameof(dto));

            try
            {
                TEntity entity = _mapper.Map<TEntity>(dto);

                if (await _repository.CreateEntityAsync(entity))
                {
                    return NoContent();
                }

                return BadRequest(
                    code: "CreateEntityFailed",
                    description: $"Failed to create {_entityName}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failure to map {_entityName} from {dto.GetType().Name}.");
                return InternalServerError();
            }
        }

        public async Task<IActionResult> ReadEntityAsync<TReadEntityDTO>(TIdentifier id)
        {
            try
            {
                var entity = await _repository.ReadEntityAsync(id);

                if (entity == null)
                {
                    return NotFound(
                        code: "EntityNotFound",
                        description: $"{_entityName} with id {id} not found.");
                }

                return Ok(_mapper.Map<TReadEntityDTO>(entity));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failure to read {_entityName} with id {id}.");
                return InternalServerError();
            }
        }

        public async Task<IActionResult> UpdateEntityAsync<TUpdateEntityDTO, TUpdatedEntityDTO>(TUpdateEntityDTO dto)
        {
            TEntity? entity = null;

            try
            {
                entity = _mapper.Map<TEntity>(dto);

                if (await _repository.UpdateEntityAsync(entity))
                {
                    return NoContent();
                }

                return BadRequest(
                    code: "UpdateEntityFailed",
                    description: $"Failed to update {_entityName}.");
            }
            catch (Exception ex)
            {
                if (entity is null)
                {
                    _logger.LogError(ex, $"Failure to update {_entityName} with unknown id.");
                }
                else
                {
                    _logger.LogError(ex, $"Failure to update {_entityName} with id {entity.Id}.");
                }
                return InternalServerError();
            }
        }

        public async Task<IActionResult> DeleteEntityAsync(TEntity entity)
            => await DeleteEntityAsync(entity.Id);

        public async Task<IActionResult> DeleteEntityAsync(TIdentifier id)
        {
            try
            {
                if (await _repository.DeleteEntityAsync(id))
                {
                    return NoContent();
                }

                return BadRequest(
                    code: "DeleteEntityFailed",
                    description: $"Failed to delete {_entityName} with id {id}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failure to delete {_entityName} with id {id}.");
                return InternalServerError();
            }
        }

        public async Task<IActionResult> ReadEntitiesAsync<TReadEntityDTO>()
        {
            try
            {
                IEnumerable<TEntity> entities = await _repository.ReadEntitiesAsync();

                return Ok(_mapper.Map<IEnumerable<TReadEntityDTO>>(entities));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failure to read {_entityName}s.");
                return InternalServerError();
            }
        }
    }
}