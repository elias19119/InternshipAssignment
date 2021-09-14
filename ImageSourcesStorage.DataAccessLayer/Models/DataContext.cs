using Microsoft.EntityFrameworkCore;

namespace ImageSourcesStorage.DataAccessLayer.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }
        public DbSet<User> User { get; set; }
        public DbSet<Pin> Pin { get; set; }
        public DbSet<Board> Board { get; set; }
        public DbSet<PinBoard> PinBoards { get; set; }
    }
}
