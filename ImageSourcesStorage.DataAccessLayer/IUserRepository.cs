namespace ImageSourcesStorage.DataAccessLayer
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ImageSourcesStorage.DataAccessLayer.Models;

    public interface IUserRepository<in TUser>
        where TUser : class
    {
        Task<IEnumerable<User>> GetAllAsync();

        Task<User> GetByIdAsync(Guid userId);

        Task InsertAsync(TUser user);

        Task UpdateAsync(TUser user);

        Task DeleteAsync(Guid userId);

        Task SaveAsync();

        Task<bool> ExistsAsync(Guid userId);

        Task<bool> NameExistsAsync(string name);
    }
}
