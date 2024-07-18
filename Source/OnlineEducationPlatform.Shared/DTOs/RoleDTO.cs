using System.ComponentModel.DataAnnotations;

namespace OnlineEducationPlatform.Shared.DTOs
{
    public class AssignRoleDTO
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string RoleName { get; set; } = string.Empty;
        [Required]
        public string TenantName { get; set; } = string.Empty;
    }
}