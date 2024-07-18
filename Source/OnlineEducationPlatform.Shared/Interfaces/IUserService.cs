using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineEducationPlatform.Shared.DTOs;
using OnlineEducationPlatform.Shared.Entities;

namespace OnlineEducationPlatform.Shared.Interfaces
{
    public interface IUserService
    {
        Task<IActionResult> RegisterUserAsync(RegisterUserDTO userDto);
        Task<IActionResult> AuthenticateUserAsync(AuthenticateUserDTO dto);
        Task<IActionResult> RefreshTokenAsync(RefreshTokenDTO dto);
    }
}