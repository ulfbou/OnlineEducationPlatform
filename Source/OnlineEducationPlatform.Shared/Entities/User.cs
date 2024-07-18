using Microsoft.AspNetCore.Identity;
using OnlineEducationPlatform.Shared.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace OnlineEducationPlatform.Shared.Entities
{

    public class User : IdentityUser<Guid>, IEntity<Guid>
    {
        [Required]
        [MinLength(3)]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        [MinLength(3)]
        public string LastName { get; set; } = string.Empty;
        [MinLength(3)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string Name => $"{FirstName} {LastName}";

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}