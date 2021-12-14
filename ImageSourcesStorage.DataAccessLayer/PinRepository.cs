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
            return await this.dataContext.Pins.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<Pin> GetPinByIdAsync(Guid pinId)
        {
            return await this.dataContext.Pins.FindAsync(pinId);
        }

        public async Task<bool> IsPinExistsAsync(Guid pinId)
        {
            return await this.dataContext.Pins.AnyAsync(x => x.PinId == pinId);
        }

        public async Task InsertPinAsync(Guid pinId, Guid userId, string imagePath, string description)
        {
            var pin = new Pin
            {
                PinId = pinId,
                ImagePath = imagePath,
                UserId = userId,
                Description = description,
            };
            await this.dataContext.Pins.AddAsync(pin);
            await this.dataContext.SaveChangesAsync();
        }

        public async Task DeletePinAsync(Guid pinId)
        {
            var pin = await this.dataContext.Pins.FindAsync(pinId);
            this.dataContext.Pins.Remove(pin);
            await this.SaveAsync();
        }

        public Task SaveAsync()
        {
            return this.dataContext.SaveChangesAsync();
        }

        public async Task EditPinAsync(Guid pinId, string description, string name)
        {
            var pin = await this.dataContext.Pins.FindAsync(pinId);

            if (pin != null)
            {
                pin.Description = description;
                pin.Name = name;
                await this.SaveAsync();
            }

        }

        public async Task<bool> IsPinBelongToUserAsync(Guid pinId, Guid userId)
        {
            return await this.dataContext.Pins.AnyAsync(x => x.UserId == userId && x.PinId == pinId);
        }
    }
}
