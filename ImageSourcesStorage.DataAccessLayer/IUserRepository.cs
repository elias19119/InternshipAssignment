namespace ImageSourcesStorage.DataAccessLayer
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ImageSourcesStorage.DataAccessLayer.Models;

    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();

        Task<User> GetByIdAsync(Guid userId);

        Task InsertAsync(User user);

        Task UpdateAsync(Guid userId, string name, int score);

        Task DeleteAsync(Guid userId);

        Task SaveAsync();

        Task ChangeUserScore(Guid userId, ChangeScoreOptions changeScoreOptions);

        Task<List<Pin>> GetUserPinsAsync(Guid userId);

        Task<bool> ExistsAsync(Guid userId);

        Task<bool> NameExistsAsync(string name);

        int GetUserScore(Guid userId);
    }
}
