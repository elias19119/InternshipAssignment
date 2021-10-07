namespace ImageSourcesStorage.DataAccessLayer
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ImageSourcesStorage.DataAccessLayer.Models;

    public interface IBoardRepository
    {
        Task<List<Board>> GetUserBoardAsync(Guid userId);

        Task<bool> UserIdExistsAsync(Guid userId);
    }
}
