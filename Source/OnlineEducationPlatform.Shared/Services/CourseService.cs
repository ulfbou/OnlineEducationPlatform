using AutoMapper;
using Microsoft.Extensions.Logging;
using OnlineEducationPlatform.Shared.Entities;
using OnlineEducationPlatform.Shared.Interfaces;

namespace OnlineEducationPlatform.Shared.Services
{
    public class CourseService(
        IGenericRepository<Course> repository,
        ILogger<GenericService<Course>> logger,
        IMapper mapper)
        : GenericService<Course>(repository, logger, mapper), IGenericService<Course>
    { }
}