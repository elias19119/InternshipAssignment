namespace ImageSourcesStorage.DataAccessLayer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ImageSourcesStorage.DataAccessLayer.Models;
    using Microsoft.EntityFrameworkCore;

    public class PinRepository : IPinRepository
    {
        private readonly DataContext dataContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="PinRepository"/> class.
        /// </summary>
        /// <param name="dataContext"></param>
        public PinRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<List<Pin>> GetAllPinsAsync()
        {
            return await this.dataContext.Pins.ToListAsync();
        }

        public async Task<Pin> GetPinByIdAsync(Guid pinId)
        {
            return await this.dataContext.Pins.FindAsync(pinId);
        }

        public async Task<bool> IsPinExistsAsync(Guid pinId)
        {
            return await this.dataContext.Pins.AnyAsync(x => x.PinId == pinId);
        }

        public Task<bool> IsPinBelongToBoardAsync(Guid boardId, Guid pinId)
        {
            return this.dataContext.Pins.AnyAsync(x => x.PinId == pinId && x.BoardId == boardId);
        }

        public async Task InsertPinAsync(Pin pin)
        {
            pin.PinId = Guid.NewGuid();
            await this.dataContext.Pins.AddAsync(pin);
            await this.dataContext.SaveChangesAsync();
        }

        public async Task InsertPinBoard(PinBoard pinBoard)
        {
            pinBoard.PinBoardId = Guid.NewGuid();
            await this.dataContext.PinBoards.AddAsync(pinBoard);
            await this.dataContext.SaveChangesAsync();
        }

        public async Task<PinBoard> GetPinBoardByIdAsync(Guid pinBoardId)
        {
            return await this.dataContext.PinBoards.FindAsync(pinBoardId);
        }
    }
}
