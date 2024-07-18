using OnlineEducationPlatform.Shared.Interfaces;

namespace OnlineEducationPlatform.Shared.Entities
{
    public abstract class Entity : Entity<int> { }
    public abstract class Entity<TIdentifier> : IEntity<TIdentifier> where TIdentifier : IEquatable<TIdentifier>
    {
        public TIdentifier Id { get; set; } = default(TIdentifier)!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public abstract string SearchableContent { get; }
    }
}