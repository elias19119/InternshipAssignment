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

        Task<PinBoard> GetPinBoardByIdAsync(Guid pinBoardId);

        Task InsertPinAsync(Pin pin);

        Task<bool> IsPinExistsAsync(Guid pinId);

        Task<bool> IsPinBelongToBoardAsync(Guid boardId, Guid pinId);

        Task InsertPinBoard(PinBoard pinBoard);
    }
}
