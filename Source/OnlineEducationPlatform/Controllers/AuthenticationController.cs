using Microsoft.AspNetCore.Mvc;
using OnlineEducationPlatform.Shared.DTOs;
using OnlineEducationPlatform.Shared.Interfaces;
using OnlineEducationPlatform.Shared.Services;

namespace OnlineEducationPlatform.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController(IAuthenticationService authenticationService, ILogger<UsersController> logger) : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        private readonly ILogger<UsersController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserDTO dto)
        {
            ArgumentNullException.ThrowIfNull(nameof(dto));

            if (ModelState.IsValid)
            {
                return await _authenticationService.LoginUserAsync(dto);
            }

            _logger.LogWarning($"Invalid model state for the {dto}.");
            return BadRequest(ModelState);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDTO dto)
        {
            ArgumentNullException.ThrowIfNull(nameof(dto));

            if (ModelState.IsValid)
            {
                return await _authenticationService.RegisterUserAsync(dto);
            }

            _logger.LogWarning($"Invalid model state for the {dto}.");
            return BadRequest(ModelState);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenDTO dto)
        {
            ArgumentNullException.ThrowIfNull(dto, nameof(dto));

            if (ModelState.IsValid)
            {
                return await _authenticationService.RefreshTokenAsync(dto);
            }

            _logger.LogWarning($"Invalid model state for the {dto}.");
            return BadRequest(ModelState);
        }
    }
}