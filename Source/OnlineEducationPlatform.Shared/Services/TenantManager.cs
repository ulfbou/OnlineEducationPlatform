using AutoMapper;
using Microsoft.Extensions.Logging;
using OnlineEducationPlatform.Shared.Entities;
using OnlineEducationPlatform.Shared.Interfaces;

namespace OnlineEducationPlatform.Shared.Services
{
    public class TenantManager<T>(
            IGenericRepository<Tenant, Guid> repository,
            ILogger<GenericService<Tenant, Guid>> logger,
            IMapper mapper)
    {
        private readonly IGenericRepository<Tenant, Guid> _repository = repository
            ?? throw new ArgumentNullException(nameof(repository));
        private readonly ILogger<GenericService<Tenant, Guid>> _logger = logger
            ?? throw new ArgumentNullException(nameof(logger));
        private readonly IMapper _mapper = mapper
            ?? throw new ArgumentNullException(nameof(mapper));

        public async Task<Tenant?> FindByNameAsync(string tenantName)
        {
            _logger.LogDebug($"Finding tenant by name: {tenantName}.");
            return await _repository.FindEntityAsync(e => e.Name.Equals(tenantName));
        }

        public Task<bool> HasUserRoleAsync(Tenant tenant, string roleName)
        {
            ArgumentNullException.ThrowIfNull(nameof(tenant));
            ArgumentNullException.ThrowIfNull(nameof(roleName));

            _logger.LogDebug($"Checking if tenant has role: {roleName}.");
            return Task.FromResult(tenant.UserRoles.Any(ur => ur.Role.Name?.Equals(roleName) ?? false));
        }
    }
}