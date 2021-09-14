using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageSourcesStorage.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace ImageSourcesStorage.DataAccessLayer
{
    public class UserRepository<TUser> : IUserRepository<User> where TUser : class
    {
        private readonly DataContext _context;
        private readonly DbSet<User> _entities;
        public UserRepository(DataContext context)
        {
            this._context = context;
            _entities = context.Set<User>();
        }
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }
        public async Task<User> GetByIdAsync(Guid userId)
        {
            return await _entities.FindAsync(userId);
        }
        public async Task InsertAsync(User user)
        {
            user.UserId = Guid.NewGuid();
            user.Score = 10;
            await _entities.AddAsync(user);
            await SaveAsync();

        }
        public async Task UpdateAsync(User user)
        {
            await _entities.AddAsync(user);
            await SaveAsync();
        }
        public async Task DeleteAsync(Guid userId)
        {
            var user = await _entities.FindAsync(userId);
            _entities.Remove(user);
            await SaveAsync();
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<bool> ExistsAsync(Guid userId)
        {
            return await _entities.AnyAsync(a => a.UserId == userId);
        }
    }
}
