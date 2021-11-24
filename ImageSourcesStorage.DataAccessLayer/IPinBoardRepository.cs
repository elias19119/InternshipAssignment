namespace ImageSourcesStorage.DataAccessLayer
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using ImageSourcesStorage.DataAccessLayer.Models;

    public interface IPinBoardRepository<in TPinBoard>
        where TPinBoard : class
    {
        Task<PinBoard> GetPinBoardByIdAsync(Guid pinBoardId);

        Task<bool> IsPinBelongToBoardAsync(Guid boardId, Guid pinId);

        Task InsertPinBoard(Guid boardId, Guid pinId);
    }
}
