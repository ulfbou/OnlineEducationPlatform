using Microsoft.EntityFrameworkCore;
using OnlineEducationPlatform.Persistence.Data;
using OnlineEducationPlatform.Shared.Interfaces;
using OnlineEducationPlatform.Shared.Entities;

namespace OnlineEducationPlatform.Persistence.Repository
{
    public class GenericRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddUserAsync(User user)
        {
            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            try
            {
                return await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}