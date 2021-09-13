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
            return await _context.Pin.ToListAsync();
        }

        public async Task<Pin> GetByIdAsync(Guid pinId)
        {
            return await _context.Pin.FindAsync(pinId);
        }

        public async Task InsertAsync(Pin pin)
        {
            await _context.Pin.AddAsync(pin);
            await SaveAsync();
        }

        public async Task UpdateAsync(Pin pin)
        {
            await _context.Pin.AddAsync(pin);
            await SaveAsync();
        }

        public async Task DeleteAsync(Guid pinId)
        {
            var pin = await _context.Pin.FindAsync(pinId);
            _context.Pin.Remove(pin);
        }

        public async Task SaveAsync()
        {
           await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Guid pinId)
        {
            return await _context.Pin.AnyAsync(a => a.PinId == pinId);
        }
    }
}
