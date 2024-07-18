using OnlineEducationPlatform.Shared.Interfaces;

namespace OnlineEducationPlatform.Shared.Entities
{
    public class Tenant : Entity<Guid>, IEntity<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public override string SearchableContent => Name;
    }
}

