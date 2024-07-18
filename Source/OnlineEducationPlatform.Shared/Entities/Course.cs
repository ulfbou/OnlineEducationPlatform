using OnlineEducationPlatform.Shared.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace OnlineEducationPlatform.Shared.Entities
{
    public class Course : Entity, IEntity
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
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public override string SearchableContent => $"ID:`{Id}` Title:`{Title}` Syllabus:`{Syllabus}` Prerequisities:`{Prerequisites}`";
    }
}