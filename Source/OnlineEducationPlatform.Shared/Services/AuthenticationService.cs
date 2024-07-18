using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OnlineEducationPlatform.Shared.DTOs;
using OnlineEducationPlatform.Shared.Entities;
using OnlineEducationPlatform.Shared.Identity;
using OnlineEducationPlatform.Shared.Interfaces;

namespace OnlineEducationPlatform.Shared.Services
{
    public class AuthenticationService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IConfiguration configuration,
            ILogger<UserService2> logger,
            IGenericRepository<RefreshToken, Guid> refreshTokenRepository,
            DefaultJwtSecurityTokenHandler tokenHandler)
        : BaseService, IAuthenticationService
    {
        private readonly UserManager<User> _userManager = userManager
            ?? throw new ArgumentNullException(nameof(userManager));
        private readonly SignInManager<User> _signInManager = signInManager
            ?? throw new ArgumentNullException(nameof(signInManager));
        private readonly IConfiguration _configuration = configuration
            ?? throw new ArgumentNullException(nameof(configuration));
        private readonly ILogger _logger = logger
            ?? throw new ArgumentNullException(nameof(logger));
        private readonly DefaultJwtSecurityTokenHandler _tokenHandler = tokenHandler
            ?? throw new ArgumentNullException(nameof(tokenHandler));
        private readonly IGenericRepository<RefreshToken, Guid> _refreshTokenRepository = refreshTokenRepository
            ?? throw new ArgumentNullException(nameof(refreshTokenRepository));

        public async Task<IActionResult> RegisterUserAsync(RegisterUserDTO dto)
        {
            ArgumentNullException.ThrowIfNull(dto, nameof(dto));

            if (await _userManager.FindByNameAsync(dto.UserName) != null) return BadRequest(code: "DuplicateUserName", description: "User name is already taken. Please, provide a unique user name.");
            if (await _userManager.FindByEmailAsync(dto.Email) != null) return BadRequest(code: "DuplicateEmail", description: "Email address is already taken. Please provide a unique email address.");

            User user = new User
            {
                Id = Guid.NewGuid(),
                UserName = dto.UserName,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        public async Task<IActionResult> LoginUserAsync(LoginUserDTO dto)
        {
            ArgumentNullException.ThrowIfNull(dto, nameof(dto));
            var user = await _userManager.FindByNameAsync(dto.UserName);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password)) return Unauthorized();

            var token = _tokenHandler.CreateToken(user);

            if (token is null) return Unauthorized("Failed to create token.");
            return Ok(new { Token = token });
        }

        public async Task<IActionResult> RefreshTokenAsync(RefreshTokenDTO dto)
        {
            ArgumentNullException.ThrowIfNull(dto, nameof(dto));

            var storedToken = await _refreshTokenRepository.FindEntityAsync(rt => rt.Token == dto.Token);
            if (storedToken == null || storedToken.ExpiryDate < DateTime.UtcNow || storedToken.Used)
            {
                return Unauthorized("Invalid, expired, or previously used refresh token.");
            }

            var user = await _userManager.FindByIdAsync(storedToken.UserId.ToString());
            if (user == null)
            {
                return Unauthorized("User not found.");
            }

            storedToken.Used = true;
            await _refreshTokenRepository.UpdateEntityAsync(storedToken);

            var newAccessToken = _tokenHandler.CreateToken(user);
            var newRefreshToken = GenerateRefreshToken(user);

            await _refreshTokenRepository.CreateEntityAsync(newRefreshToken);
            await _refreshTokenRepository.SaveChangesAsync();

            return Ok(new { AccessToken = newAccessToken, RefreshToken = newRefreshToken.Token });
        }

        private RefreshToken GenerateRefreshToken(User user)
        {
            return new RefreshToken
            {
                Token = Guid.NewGuid().ToString(),
                ExpiryDate = DateTime.UtcNow.AddDays(30),
                UserId = user.Id
            };
        }
    }
}