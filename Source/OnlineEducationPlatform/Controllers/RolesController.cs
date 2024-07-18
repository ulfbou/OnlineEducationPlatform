using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineEducationPlatform.Shared.DTOs;
using OnlineEducationPlatform.Shared.Entities;
using OnlineEducationPlatform.Shared.Interfaces;

namespace OnlineEducationPlatform.Controllers
{
    /*
    - Get Roles (`GET /roles`)
      - Access: Yes, can view all available roles.
    - Assign Role (`POST /roles/assign`)
      - Access: Yes, can assign roles to any user across any tenant.
    - Remove Role (`POST /roles/remove`)
      - Access: Yes, can remove roles from any user across any tenant.
    - Get User Roles (`GET /roles/user`)
      - Access: Yes, can retrieve roles for any user across any tenant.
     */
    [Authorize(Policy = Policy.SuperAdmin)]
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController(IRolesService rolesService, ILogger<RolesController> logger) : Controller
    {
        private readonly IRolesService _roleService = rolesService
            ?? throw new ArgumentNullException(nameof(rolesService));
        private readonly ILogger<RolesController> _logger = logger
            ?? throw new ArgumentNullException(nameof(logger));

        // GET: /api/Roles
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await _roleService.ReadRolesAsync();
        }

        // POST: /Roles/Assign
        [Authorize(Policy = Policy.TenantAdmin)]
        [HttpPost("assign")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await _roleService.AssignRoleAsync(dto);
        }

        // POST: /Roles/Remove
        [Authorize(Policy = Policy.TenantAdmin)]
        [HttpPost("remove")]
        public async Task<IActionResult> RemoveRole([FromBody] AssignRoleDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await _roleService.RemoveRoleAsync(dto);
        }

        // GET: /Roles/User

    }
}