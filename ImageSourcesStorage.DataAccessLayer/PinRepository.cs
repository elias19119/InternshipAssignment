namespace ImageSourcesStorage.DataAccessLayer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ImageSourcesStorage.DataAccessLayer.Models;
    using Microsoft.EntityFrameworkCore;

    public class PinRepository<TPin> : IPinRepository<Pin>
        where TPin : class
    {
        private readonly DataContext dataContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="PinRepository{TPin}"/> class.
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
    }
}
