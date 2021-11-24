namespace ImageSourcesStorage.DataAccessLayer
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using ImageSourcesStorage.DataAccessLayer.Models;
    using Microsoft.EntityFrameworkCore;

    public class PinBoardRepository<TPinBoard> : IPinBoardRepository<PinBoard>
        where TPinBoard : class
    {
        private readonly DataContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="PinBoardRepository{TPinBoard}"/> class.
        /// </summary>
        /// <param name="context"></param>
        public PinBoardRepository(DataContext context)
        {
            this.context = context;
        }

        public PinBoardRepository()
        {
        }

        public async Task<PinBoard> GetPinBoardByIdAsync(Guid pinBoardId)
        {
            return await this.context.PinBoards.FindAsync(pinBoardId);
        }

        public async Task InsertPinBoard(Guid boardId, Guid pinId)
        {
            var pinBoard = new PinBoard
            {
                PinId = pinId,
                BoardId = boardId,
                PinBoardId = Guid.NewGuid(),
            };
            await this.context.PinBoards.AddAsync(pinBoard);
            await this.context.SaveChangesAsync();
        }

        public async Task<bool> IsPinBelongToBoardAsync(Guid boardId, Guid pinId)
        {
            return await this.context.PinBoards.AnyAsync(x => x.PinId == pinId && x.BoardId == boardId);
        }
    }
}
