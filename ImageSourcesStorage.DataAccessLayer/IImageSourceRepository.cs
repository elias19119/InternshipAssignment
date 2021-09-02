using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace ImageSourcesStorage.DataAccessLayer
{
    public interface IImageSourceRepository
    {
        Task <IEnumerable<ImageSource>> GetAllAsync();
        Task<ImageSource> GetByIdAsync(int imageSourceId);
        Task InsertAsync(ImageSource imageSource);
        Task UpdateAsync(ImageSource imageSource);
        Task DeleteAsync(int imageSourceId);
        Task SaveAsync();
        Task<bool> ExistsAsync(int imageId);
    }
}
