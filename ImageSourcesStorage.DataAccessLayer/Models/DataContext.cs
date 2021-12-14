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

        public DbSet<Credentials> Credentials { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>();

            modelBuilder.Entity<Board>();

            modelBuilder.Entity<Pin>()
           .HasKey(x => x.PinId);

            modelBuilder.Entity<Credentials>().HasNoKey();
        }
    }
}
