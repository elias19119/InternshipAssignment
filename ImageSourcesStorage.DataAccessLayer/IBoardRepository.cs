namespace ImageSourcesStorage.DataAccessLayer
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ImageSourcesStorage.DataAccessLayer.Models;

    public interface IBoardRepository
    {
        Task<List<Board>> GetUserBoardAsync(Guid userId);

        Task AddBoardToUserAsync(Guid userId, Guid boardId, string name);

        Task<Board> GetBoardByIdAsync(Guid boardId);

        Task DeleteBoardOfUserAsync(Guid boardId);

        Task<bool> IsNameExistsAsync(string name);

        Task<bool> IsBoardExistsAsync(Guid boardId);

        Task<bool> IsBoardBelongToUserAsync(Guid boardId, Guid userId);

        Task SaveAsync();
    }
}
