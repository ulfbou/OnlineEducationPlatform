namespace OnlineEducationPlatform.Shared.DTOs
{
    public class ReadCourseDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Syllabus { get; set; } = string.Empty;
        public string Prerequisites { get; set; } = string.Empty;
        public Guid InstructorId { get; set; } = Guid.NewGuid();
    }
}