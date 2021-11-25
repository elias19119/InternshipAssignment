namespace ImageSourcesStorage.DataAccessLayer
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ImageSourcesStorage.DataAccessLayer.Models;

    public interface IBoardRepository<in TBoard>
        where TBoard : class
    {
        Task<List<BoardModelDetails>> GetUserBoardsAsync(Guid userId);

        Task AddBoardToUserAsync(Guid userId, Guid boardId, string name);

        Task<Board> GetBoardByIdAsync(Guid boardId);

        Task DeleteBoardOfUserAsync(Guid boardId);

        Task DeletePinOfBoardAsync(Guid pinId);

        Task<bool> IsNameExistsAsync(string name);

        Task<bool> IsBoardExistsAsync(Guid boardId);

        Task<bool> IsBoardBelongToUserAsync(Guid boardId, Guid userId);

        Task EditNameOfBoardAsync(Guid boardId, Guid userId, string name);

        Task SaveAsync();
    }
}
