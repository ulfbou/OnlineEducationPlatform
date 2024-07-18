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
    public class CoursesController(
        IGenericService<Course> service,
        ILogger<CoursesController> logger) : Controller
    {
        protected readonly IGenericService<Course> _service = service
            ?? throw new ArgumentNullException(nameof(service));
        protected readonly ILogger _logger = logger
            ?? throw new ArgumentNullException(nameof(logger));

        [HttpPost]
        public virtual async Task<IActionResult> CreateCourse([FromBody] CreateCourseDTO dto)
        {
            ArgumentNullException.ThrowIfNull(dto, nameof(dto));

            if (!ModelState.IsValid)
            {
                // Log the complete details of the invalid model state
                _logger.LogWarning($"Invalid model state for the {dto}.");
                foreach (var data in ModelState)
                {
                    _logger.LogWarning($"Key: {data.Key}, Value: {data.Value}");
                }
                return BadRequest(ModelState);
            }
            return await _service.CreateEntityAsync<CreateCourseDTO>(dto);
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> ReadCourse(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await _service.ReadEntityAsync<ReadCourseDTO>(id);
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> UpdateCourse(int id, [FromBody] UpdateCourseDTO dto)
        {
            ArgumentNullException.ThrowIfNull(dto, nameof(dto));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await _service.UpdateEntityAsync<UpdateCourseDTO, UpdatedCourseDTO>(dto);
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> DeleteCourse(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return await _service.DeleteEntityAsync(id);
        }

        [HttpGet]
        public async Task<IActionResult> ReadCourses()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await _service.ReadEntitiesAsync<ReadCourseDTO>();
        }
    }
}