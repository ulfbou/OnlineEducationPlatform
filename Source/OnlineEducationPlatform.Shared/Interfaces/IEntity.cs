namespace OnlineEducationPlatform.Shared.Interfaces
{
    public interface IEntity : IEntity<int> { }
    public interface IEntity<TIdentifier> where TIdentifier : IEquatable<TIdentifier>
    {
        TIdentifier Id { get; set; }
        DateTime CreatedAt { get; set; }
    }
}