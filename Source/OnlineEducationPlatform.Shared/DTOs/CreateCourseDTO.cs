using System.ComponentModel.DataAnnotations;

namespace OnlineEducationPlatform.Shared.DTOs
{
    public class CreateCourseDTO
    {
        [Required, StringLength(50, MinimumLength = 3)]
        public string Title { get; set; } = string.Empty;
        [Required, StringLength(500, MinimumLength = 3)]
        public string Description { get; set; } = string.Empty;
        [Required, StringLength(500, MinimumLength = 3)]
        public string Syllabus { get; set; } = string.Empty;
        [Required, StringLength(500, MinimumLength = 3)]
        public string Prerequisites { get; set; } = string.Empty;
        public Guid InstructorId { get; set; } = Guid.NewGuid();
    }
}