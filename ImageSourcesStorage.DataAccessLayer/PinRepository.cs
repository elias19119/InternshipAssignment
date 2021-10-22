namespace ImageSourcesStorage.DataAccessLayer
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ImageSourcesStorage.DataAccessLayer.Models;
    using Microsoft.EntityFrameworkCore;

    public class PinRepository : IPinRepository
    {
        private readonly DataContext dataContext;
        private readonly DbSet<Pin> entity;

        /// <summary>
        /// Initializes a new instance of the <see cref="PinRepository"/> class.
        /// </summary>
        /// <param name="dataContext"></param>
        public PinRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
            this.entity = dataContext.Set<Pin>();
        }

        public async Task<List<Pin>> GetAllPinsAsync()
        {
            return await this.entity.ToListAsync();
        }
    }
}
