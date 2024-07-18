using System.ComponentModel.DataAnnotations;

namespace OnlineEducationPlatform.Shared.DTOs
{
    public class RefreshTokenDTO
    {
        [Required]
        public string Token { get; set; } = string.Empty;
    }
}