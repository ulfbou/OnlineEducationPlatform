using System.ComponentModel.DataAnnotations;

namespace OnlineEducationPlatform.Shared.DTOs
{
    public class RegisterUserDTO
    {
        [Required, StringLength(100, MinimumLength = 3)]
        public string FirstName { get; set; } = string.Empty;
        [Required, StringLength(100, MinimumLength = 3)]
        public string LastName { get; set; } = string.Empty;
        [Required, StringLength(100, MinimumLength = 3)]
        public string UserName { get; set; } = string.Empty;
        [Required, StringLength(50, MinimumLength = 3)]
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}