using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageSourcesStorage.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;


namespace ImageSourcesStorage.DataAccessLayer
{
    public class PinRepository : IPinRepository
    {
        private readonly DataContext _context;
        public PinRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Pin>> GetAllAsync()
        {
            return await _context.Pins.Include(p => p.Board).ToListAsync();
        }

        public async Task<Pin> GetByIdAsync(Guid imageSourceId)
        {
            return await _context.Pins.FindAsync(imageSourceId);
        }

        public async Task InsertAsync(Pin pin)
        {
            await _context.Pins.AddAsync(pin);
            await SaveAsync();
        }

        public async Task UpdateAsync(Pin pin)
        {
            await _context.Pins.AddAsync(pin);
            await SaveAsync();
        }

        public async Task DeleteAsync(Guid imageSourceId)
        {
            var imageSource = await _context.Pins.FindAsync(imageSourceId);
            _context.Pins.Remove(imageSource);
        }

        public async Task SaveAsync()
        {
           await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Guid imageId)
        {
            return await _context.Pins.AnyAsync(a => a.Id == imageId);
        }
    }
}
