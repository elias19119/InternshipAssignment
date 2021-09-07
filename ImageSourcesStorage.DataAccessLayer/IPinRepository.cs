using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace ImageSourcesStorage.DataAccessLayer
{
    public interface IPinRepository
    {
        Task <IEnumerable<Pin>> GetAllAsync();
        Task<Pin> GetByIdAsync(Guid pinId);
        Task InsertAsync(Pin pin);
        Task UpdateAsync(Pin pin);
        Task DeleteAsync(Guid pinId);
        Task SaveAsync();
        Task<bool> ExistsAsync(Guid pinId);
    }
}
