using Microsoft.AspNetCore.Identity;
using OnlineEducationPlatform.Shared.Interfaces;

namespace OnlineEducationPlatform.Shared.Entities
{
    public class Role : IdentityRole<Guid>, IEntity<Guid>
    {
        public const string SuperAdmin = "SuperAdmin";
        public const string TenantAdmin = "TenantAdmin";
        public const string CourseCreator = "CourseCreator";
        public const string CourseEditor = "CourseEditor";
        public const string CourseViewer = "CourseViewer";
        public const string Instructor = "Instructor";
        public const string Student = "Student";
        public const string User = "User";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
    }
}