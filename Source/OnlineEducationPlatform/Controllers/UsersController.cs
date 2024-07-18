using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineEducationPlatform.Shared.DTOs;
using OnlineEducationPlatform.Shared.Entities;
using OnlineEducationPlatform.Shared.Interfaces;

namespace OnlineEducationPlatform.Controllers
{
    public class UsersController(IUserService userService, ILogger<UsersController> logger) : ControllerBase
    {
        private readonly IUserService _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        private readonly ILogger<UsersController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUserAsync([FromBody] RegisterUserDTO dto)
        {
            ArgumentNullException.ThrowIfNull(dto, nameof(dto));

            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"Invalid model state for the {dto}.");
                return BadRequest(ModelState);
            }

            return await _userService.RegisterUserAsync(dto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> AuthenticateUserAsync([FromBody] AuthenticateUserDTO dto)
        {
            ArgumentNullException.ThrowIfNull(dto, nameof(dto));

            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"Invalid model state for the {dto}.");
                return BadRequest(ModelState);
            }

            return await _userService.AuthenticateUserAsync(dto);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenDTO dto)
        {
            ArgumentNullException.ThrowIfNull(dto, nameof(dto));

            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"Invalid model state for the {dto}.");
                return BadRequest(ModelState);
            }

            return await _userService.RefreshTokenAsync(dto);
        }
    }
}