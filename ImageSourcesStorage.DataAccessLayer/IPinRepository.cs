namespace ImageSourcesStorage.DataAccessLayer
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ImageSourcesStorage.DataAccessLayer.Models;

    public interface IPinRepository 
    {
        Task<List<Pin>> GetAllPinsAsync();

        Task<Pin> GetPinByIdAsync(Guid pinId);

        Task InsertPinAsync(Guid pinId, Guid userId, string imagePath);

        Task<bool> IsPinExistsAsync(Guid pinId);
    }
}
