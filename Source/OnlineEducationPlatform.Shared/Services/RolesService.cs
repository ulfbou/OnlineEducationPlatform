using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineEducationPlatform.Shared.DTOs;
using OnlineEducationPlatform.Shared.Entities;
using OnlineEducationPlatform.Shared.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OnlineEducationPlatform.Shared.Services
{
    public class RoleService(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        TenantManager<Tenant> tenantManager,
        ILogger<GenericService<Role, Guid>> logger,
        IMapper mapper)
        : BaseService, IRolesService
    {
        private readonly UserManager<User> _userManager = userManager
            ?? throw new ArgumentNullException(nameof(userManager));
        private readonly RoleManager<Role> _roleManager = roleManager
            ?? throw new ArgumentNullException(nameof(roleManager));
        private readonly TenantManager<Tenant> _tenantManager = tenantManager
            ?? throw new ArgumentNullException(nameof(tenantManager));
        private readonly ILogger<GenericService<Role, Guid>> _logger = logger
            ?? throw new ArgumentNullException(nameof(logger));
        private readonly IMapper _mapper = mapper
            ?? throw new ArgumentNullException(nameof(mapper));

        public Task<IActionResult> ReadRolesAsync()
        {
            return Task.FromResult(Ok(_roleManager.Roles));
        }

        public async Task<IActionResult> AssignRoleAsync(AssignRoleDTO dto)
        {
            return await RoleOperationAsync(dto, async user =>
            {
                var result = await _userManager.AddToRoleAsync(user, dto.RoleName);
                if (result.Succeeded)
                {
                    return Ok(new { Message = "Role assigned successfully" });
                }
                else
                {
                    _logger.LogWarning($"Failed to assign role {dto.RoleName} to user {dto.UserName}.");
                    var error = result.Errors.FirstOrDefault();
                    return BadRequest(code: error!.Code, description: error.Description);
                }
            });
        }

        public async Task<IActionResult> RemoveRoleAsync(AssignRoleDTO dto)
        {
            return await RoleOperationAsync(dto, async user =>
            {
                var result = await _userManager.RemoveFromRoleAsync(user, dto.RoleName);
                if (result.Succeeded)
                {
                    return Ok(new { Message = "Role removed successfully" });
                }
                else
                {
                    _logger.LogWarning($"Failed to remove role {dto.RoleName} from user with id {dto.UserName}.");
                    var error = result.Errors.FirstOrDefault();
                    return BadRequest(code: error!.Code, description: error.Description);
                }
            });
        }

        private async Task<IActionResult> RoleOperationAsync(AssignRoleDTO dto, Func<User, Task<IActionResult>> operation)
        {
            ArgumentNullException.ThrowIfNull(dto, nameof(dto));

            try
            {
                var user = await _userManager.FindByNameAsync(dto.UserName);
                if (user is null)
                {
                    _logger.LogWarning($"User with username {dto.UserName} not found.");
                    return NotFound(
                        code: "Not Found",
                        description: $"There seems to be no user by the name of {dto.UserName}.");
                }

                var role = await _roleManager.FindByNameAsync(dto.RoleName);
                if (role is null)
                {
                    _logger.LogWarning($"Role with name {dto.RoleName} not found.");
                    return NotFound(
                        code: "Not Found",
                        description: $"There seems to be no role by the name of {dto.RoleName}.");
                }

                // Find tenant by name
                var tenant = await _tenantManager.FindByNameAsync(dto.TenantName);
                if (tenant is null)
                {
                    _logger.LogWarning($"Tenant with name {dto.TenantName} not found.");
                    return NotFound(
                        code: "Not Found",
                        description: $"There seems to be no tenant by the name of {dto.TenantName}.");

                }

                if (await _userManager.IsInRoleAsync(user, role?.Name!))
                {
                    _logger.LogWarning($"User with username {dto.UserName} is already in role {dto.RoleName}.");
                    return BadRequest(
                        code: "AlreadyInRole",
                        description: $"User with username {dto.UserName} is already in role {dto.RoleName}.");
                }

                if (!await _tenantManager.HasUserRoleAsync(tenant, role?.Name!))
                {
                    _logger.LogWarning($"Tenant with name {dto.TenantName} does not have role {dto.RoleName}.");
                    return BadRequest(
                        code: "RoleNotInTenant",
                        description: $"Tenant with name {dto.TenantName} does not have role {dto.RoleName}.");
                }

                // TODO: Verify that the user is either a super admin or a tenant admin for the tenant that we are assigning/removing the role to.

                return await operation(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failure to assign role {dto.RoleName} to user with id {dto.UserName}.");
                return InternalServerError();
            }
        }
    }
}