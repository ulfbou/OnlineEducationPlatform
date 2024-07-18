using OnlineEducationPlatform.Shared.Entities;

namespace OnlineEducationPlatform.Shared.Interfaces
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user);
        Task<User?> GetUserByUsernameAsync(string username);
    }
}