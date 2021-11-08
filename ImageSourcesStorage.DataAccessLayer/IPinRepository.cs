namespace ImageSourcesStorage.DataAccessLayer
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPinRepository
    {
        Task<List<Pin>> GetAllPinsAsync();

        Task<Pin> GetPinByIdAsync(Guid pinId);

        Task<bool> IsPinExistsAsync(Guid pinId);

        Task<bool> IsPinBelongToBoardAsync(Guid boardId, Guid pinId);
    }
}
