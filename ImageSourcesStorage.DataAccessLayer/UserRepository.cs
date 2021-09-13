using System;
using System.Collections.Generic;
using System.Linq;
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
            return await _context.User.ToListAsync();
        }

        public async Task<User> GetByIdAsync(Guid imageSourceId)
        {
            return await _context.User.FindAsync(imageSourceId);
        }

        public async Task InsertAsync(User user)
        {
            user.UserId = Guid.NewGuid();
            user.Score = 10;
            await _context.User.AddAsync(user);
            await SaveAsync();
        }

        public async Task UpdateAsync(User user)
        {
            await _context.User.AddAsync(user);
            await SaveAsync();
        }

        public async Task DeleteAsync(Guid userId)
        {
            var user = await _context.User.FindAsync(userId);
            _context.User.Remove(user);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Guid userId)
        {
            return await _context.User.AnyAsync(a => a.UserId == userId);
        }
    }
}
