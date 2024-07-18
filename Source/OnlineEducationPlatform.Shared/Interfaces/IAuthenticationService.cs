using Microsoft.AspNetCore.Mvc;
using OnlineEducationPlatform.Shared.DTOs;

namespace OnlineEducationPlatform.Shared.Interfaces
{
    public interface IAuthenticationService
    {
        Task<IActionResult> LoginUserAsync(LoginUserDTO dto);
        Task<IActionResult> RegisterUserAsync(RegisterUserDTO userDto);
        Task<IActionResult> RefreshTokenAsync(RefreshTokenDTO dto);
    }
}