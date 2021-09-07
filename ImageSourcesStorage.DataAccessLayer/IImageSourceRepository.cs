using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace ImageSourcesStorage.DataAccessLayer
{
    public interface IImageSourceRepository
    {
        Task <IEnumerable<ImageSource>> GetAllAsync();
        Task<ImageSource> GetByIdAsync(Guid imageSourceId);
        Task InsertAsync(ImageSource imageSource);
        Task UpdateAsync(ImageSource imageSource);
        Task DeleteAsync(Guid imageSourceId);
        Task SaveAsync();
        Task<bool> ExistsAsync(Guid imageId);
    }
}
