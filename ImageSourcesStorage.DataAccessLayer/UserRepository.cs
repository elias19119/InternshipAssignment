using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ImageSourcesStorage.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace ImageSourcesStorage.DataAccessLayer
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetByIdAsync(Guid imageSourceId)
        {
            return await _context.Users.FindAsync(imageSourceId);
        }

        public async Task InsertAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await SaveAsync();
        }

        public async Task UpdateAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await SaveAsync();
        }

        public async Task DeleteAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            _context.Users.Remove(user);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Guid userId)
        {
            return await _context.Users.AnyAsync(a => a.UserId == userId);
        }
    }
}
