namespace ImageSourcesStorage.DataAccessLayer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ImageSourcesStorage.DataAccessLayer.Models;
    using Microsoft.EntityFrameworkCore;

    public class UserRepository : IUserRepository
    {
        private readonly DataContext context;

        public UserRepository(DataContext context)
        {
            this.context = context;
        }

        public UserRepository()
        {
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await this.context.Users.ToListAsync();
        }

        public async Task<User> GetByIdAsync(Guid userId)
        {
            return await this.context.Users.FindAsync(userId);
        }

        public async Task InsertAsync(User user)
        {
            user.UserId = Guid.NewGuid();
            user.Score = 10;
            await this.context.Users.AddAsync(user);
            await this.SaveAsync();
        }

        public async Task UpdateAsync(Guid userId, string name, int score)
        {
            var user = await this.context.Users.FindAsync(userId);

            if (user != null)
            {
                user.Name = name;
                user.Score = score;
                await this.SaveAsync();
            }
        }

        public async Task DeleteAsync(Guid userId)
        {
            var user = await this.context.Users.FindAsync(userId);
            this.context.Users.Remove(user);
            await this.SaveAsync();
        }

        public async Task SaveAsync()
        {
            await this.context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Guid userId)
        {
            return await this.context.Users.AnyAsync(a => a.UserId == userId);
        }

        public async Task<bool> NameExistsAsync(string name)
        {
            return await this.context.Users.AnyAsync(a => a.Name == name);
        }

        public async Task ChangeUserScore(Guid userId, ChangeScoreOptions changeScoreOptions)
        {
            var user = await this.context.Users.FindAsync(userId);

            if (user != null)
            {
                if (user.Score > 0)
                {
                    if (changeScoreOptions == ChangeScoreOptions.Decrease)
                    {
                        user.Score -= 1;
                    }
                    else
                    {
                        user.Score += 1;
                    }

                    await this.SaveAsync();
                }
            }
        }

        public async Task<List<Pin>> GetUserPinsAsync(Guid userId)
        {
            return await this.context.Pins
           .Where(u => u.UserId == userId)
           .ToListAsync();
        }

        public int GetUserScore(Guid userId)
        {
            return this.context.Users.Where(x => x.UserId == userId).Select(x=>x.Score).FirstOrDefault();
        }
    }
}
