namespace ImageSourcesStorage.DataAccessLayer
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ImageSourcesStorage.DataAccessLayer.Models;
    using Microsoft.EntityFrameworkCore;

    public class UserRepository<TUser> : IUserRepository<User>
        where TUser : class
    {
        private readonly DataContext context;
        private readonly DbSet<User> entities;

        public UserRepository(DataContext context)
        {
            this.context = context;
            this.entities = context.Set<User>();
        }

        public UserRepository()
        {
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await this.entities.ToListAsync();
        }

        public async Task<User> GetByIdAsync(Guid userId)
        {
            return await this.entities.FindAsync(userId);
        }

        public async Task InsertAsync(User user)
        {
            user.UserId = Guid.NewGuid();
            user.Score = 10;
            await this.entities.AddAsync(user);
            await this.SaveAsync();
        }

        public async Task UpdateAsync(Guid userId, string name, int score)
        {
            var user = await this.entities.FindAsync(userId);

            if (user != null)
            {
                user.Name = name;
                user.Score = score;
                await this.SaveAsync();
            }
        }

        public async Task DeleteAsync(Guid userId)
        {
            var user = await this.entities.FindAsync(userId);
            this.entities.Remove(user);
            await this.SaveAsync();
        }

        public async Task SaveAsync()
        {
            await this.context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Guid userId)
        {
            return await this.entities.AnyAsync(a => a.UserId == userId);
        }

        public async Task<bool> NameExistsAsync(string name)
        {
            return await this.entities.AnyAsync(a => a.Name == name);
        }

        public async Task ChangeUserScore(Guid userId, ChangeScoreOptions changeScoreOptions)
        {
            var user = await this.entities.FindAsync(userId);

            if (user != null)
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
}
