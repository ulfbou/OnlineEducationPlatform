using OnlineEducationPlatform.Shared.Entities;
using OnlineEducationPlatform.Shared.Interfaces;

namespace OnlineEducationPlatform.Shared.Entities
{
    public class RefreshToken : Entity<Guid>, IEntity<Guid>
    {
        public string Token { get; set; } = default!;
        public DateTime ExpiryDate { get; set; } = DateTime.UtcNow;
        public Guid UserId { get; set; }
        public bool Used { get; set; }
        public override string SearchableContent => Token;
    }
}