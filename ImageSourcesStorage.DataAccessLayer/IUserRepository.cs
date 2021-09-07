using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ImageSourcesStorage.DataAccessLayer.Models;

namespace ImageSourcesStorage.DataAccessLayer
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(Guid userId);
        Task InsertAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(Guid userId);
        Task SaveAsync();
        Task<bool> ExistsAsync(Guid userId);
    }
}
