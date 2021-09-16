namespace ImageSourcesStorage.DataAccessLayer.Models
{
    using Microsoft.EntityFrameworkCore;

    public class DataContext : DbContext
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Pin> Pins { get; set; }

        public DbSet<Board> Boards { get; set; }

        public DbSet<PinBoard> PinBoards { get; set; }
    }
}
