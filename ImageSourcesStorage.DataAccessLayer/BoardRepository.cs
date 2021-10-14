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

        public async Task<List<Board>> GetUserBoardAsync(Guid userId)
        {
            return await this.context.Boards.Include(u => u.Pins)
            .Where(u => u.UserId == userId)
            .ToListAsync();
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
    }
}
