namespace ImageSourcesStorage.DataAccessLayer
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPinRepository
    {
        Task<List<Pin>> GetAllPinsAsync();

        Task<List<Pin>> GetUserPinsAsync(Guid userId);

        Task<Pin> GetPinByIdAsync(Guid pinId);

        Task<bool> IsPinExistsAsync(Guid pinId);
    }
}
