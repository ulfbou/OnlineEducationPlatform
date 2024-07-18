using Microsoft.AspNetCore.Mvc;
using OnlineEducationPlatform.Shared.DTOs;
using OnlineEducationPlatform.Shared.Entities;

namespace OnlineEducationPlatform.Shared.Interfaces
{
    public interface IRolesService
    {
        Task<IActionResult> ReadRolesAsync();
        Task<IActionResult> AssignRoleAsync(AssignRoleDTO dto);
        Task<IActionResult> RemoveRoleAsync(AssignRoleDTO dto);
    }
}