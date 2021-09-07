using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace ImageSourcesStorage.DataAccessLayer
{
    public class PinRepository : IPinRepository
    {
        private readonly PinContext _context;
        public PinRepository(PinContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Pin>> GetAllAsync()
        {
            return await _context.ImageSources.ToListAsync();
        }

        public async Task<Pin> GetByIdAsync(Guid imageSourceId)
        {
            return await _context.ImageSources.FindAsync(imageSourceId);
        }

        public async Task InsertAsync(Pin pin)
        {
            await _context.ImageSources.AddAsync(pin);
            await SaveAsync();
        }

        public async Task UpdateAsync(Pin pin)
        {
            await _context.ImageSources.AddAsync(pin);
            await SaveAsync();
        }

        public async Task DeleteAsync(Guid imageSourceId)
        {
            var imageSource = await _context.ImageSources.FindAsync(imageSourceId);
            _context.ImageSources.Remove(imageSource);
        }

        public async Task SaveAsync()
        {
           await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Guid imageId)
        {
            return await _context.ImageSources.AnyAsync(a => a.Id == imageId);
        }
    }
}
