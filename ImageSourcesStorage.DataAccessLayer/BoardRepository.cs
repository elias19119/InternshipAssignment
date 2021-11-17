namespace ImageSourcesStorage.DataAccessLayer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ImageSourcesStorage.DataAccessLayer.Models;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Board logic.
    /// </summary>
    public class BoardRepository : IBoardRepository
    {
        private readonly DataContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="BoardRepository"/> class.
        /// </summary>
        /// <param name="context"></param>
        public BoardRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<List<BoardEntity>> GetUserBoardAsync(Guid userId)
        {
            var result = new List<BoardEntity>();

            IQueryable<Guid> boardIds = this.context.Boards.Where(x => x.UserId == userId).Select(y => y.BoardId);

            foreach (var boardId in boardIds)
            {
                var board = await this.context.Boards.FindAsync(boardId);
                var boardModel = new BoardEntity();
                boardModel.Name = board.Name;
                boardModel.UserId = board.UserId;

                var pinIds = this.context.PinBoards.Where(x => x.BoardId == boardId).Select(y => y.PinId);
                List<Pin> pins = this.context.Pins.Where(x => pinIds.Contains(x.PinId)).ToList();
                List<PinModel> pinModels = pins.Select(x => new PinModel() { PinId = x.PinId, ImagePath = x.ImagePath, UserId = x.UserId, Description = x.Description }).ToList();

                boardModel.pins = pinModels;
                result.Add(boardModel);
            }

            return result;
        }

        public async Task AddBoardToUserAsync(Guid userId, Guid boardId, string name)
        {
            Board board = new Board()
            {
                BoardId = boardId,
                UserId = userId,
                Name = name,
            };

            await this.context.Boards.AddAsync(board);
            await this.SaveAsync();
        }

        public async Task DeleteBoardOfUserAsync(Guid boardId)
        {
            var board = await this.context.Boards.FindAsync(boardId);
            this.context.Boards.Remove(board);
            await this.SaveAsync();
        }

        public Task SaveAsync()
        {
            return this.context.SaveChangesAsync();
        }

        public async Task<bool> IsNameExistsAsync(string name)
        {
            return await this.context.Boards.AnyAsync(a => a.Name == name);
        }

        public async Task<Board> GetBoardByIdAsync(Guid boardId)
        {
            return await this.context.Boards.FindAsync(boardId);
        }

        public async Task<bool> IsBoardExistsAsync(Guid boardId)
        {
            return await this.context.Boards.AnyAsync(x => x.BoardId == boardId);
        }

        public Task<bool> IsBoardBelongToUserAsync(Guid boardId, Guid userId)
        {
            return this.context.Boards.AnyAsync(x => x.BoardId == boardId && x.UserId == userId);
        }

        public async Task EditNameOfBoardAsync(Guid boardId, Guid userId, string name)
        {
            var board = await this.context.Boards.FindAsync(boardId);

            if (board != null)
            {
                board.Name = name;
                await this.SaveAsync();
            }
        }

        public async Task DeletePinOfBoardAsync(Guid pinId)
        {
            var pin = this.context.PinBoards.FirstOrDefault(x => x.PinId == pinId);
            this.context.PinBoards.Remove(pin);
            await this.SaveAsync();
        }
    }
}
