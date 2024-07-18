using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineEducationPlatform.Persistence.Repository;
using OnlineEducationPlatform.Shared.DTOs;
using OnlineEducationPlatform.Shared.Entities;
using OnlineEducationPlatform.Shared.Interfaces;
using OnlineEducationPlatform.Shared.Services;

namespace OnlineEducationPlatform.Controllers
{
    [Route("api/[controller]")]
    public class GenericController<TEntity>(
        IGenericService<TEntity> service,
        ILogger<GenericController<TEntity>> logger) : Controller
        where TEntity : Entity, IEntity
    {
        protected readonly IGenericService<TEntity> _service = service
            ?? throw new ArgumentNullException(nameof(service));
        protected readonly ILogger _logger = logger
            ?? throw new ArgumentNullException(nameof(logger));

        [HttpPost]
        public virtual async Task<IActionResult> CreateEntity<TCreateEntityDTO>(TCreateEntityDTO dto)
        {
            ArgumentNullException.ThrowIfNull(dto, nameof(dto));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return await _service.CreateEntityAsync<TCreateEntityDTO>(dto);
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> ReadEntity<TReadDTO>(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await _service.ReadEntityAsync<TReadDTO>(id);
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> UpdateEntity<TUpdateEntityDTO, TUpdatedEntityDTO>(int id, [FromBody] TUpdateEntityDTO dto)
        {
            ArgumentNullException.ThrowIfNull(dto, nameof(dto));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await _service.UpdateEntityAsync<TUpdateEntityDTO, TUpdatedEntityDTO>(dto);
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> DeleteEntity(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await _service.DeleteEntityAsync(id);
        }

        [HttpGet]
        public virtual async Task<IActionResult> ReadEntities<TReadEntityDTO>()
        {
            return await _service.ReadEntitiesAsync<TReadEntityDTO>();
        }

        protected async Task<TDTO?> ReadRequestBodyAsync<TDTO>() where TDTO : class
        {
            // Enable buffering to allow multiple reads
            HttpContext.Request.EnableBuffering();

            // Read the request body
            var body = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            _logger.LogDebug($"Raw Request Body: {body}");

            // Rewind the stream for model binding or manual deserialization
            HttpContext.Request.Body.Position = 0;

            // Manually deserialize the request body to CreateCourseDTO
            TDTO? dto;
            try
            {
                dto = System.Text.Json.JsonSerializer.Deserialize<TDTO>(body);
            }
            catch (System.Text.Json.JsonException ex)
            {
                _logger.LogError($"JSON Deserialization error: {ex.Message}");
                return null;
            }

            // Log the deserialized object (optional)
            _logger.LogDebug($"Deserialized DTO: {System.Text.Json.JsonSerializer.Serialize(dto)}");
            return dto;
        }
    }
}