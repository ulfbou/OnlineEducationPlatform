using Microsoft.AspNetCore.Identity;

namespace OnlineEducationPlatform.Shared.Entities
{
    public class UserRole : IdentityUserRole<Guid>
    {
        public Guid TenantId { get; set; } = Guid.NewGuid();
        public User User { get; set; } = new User();
        public Role Role { get; set; } = new Role();
        public virtual Tenant Tenant { get; set; } = new Tenant();
    }
}