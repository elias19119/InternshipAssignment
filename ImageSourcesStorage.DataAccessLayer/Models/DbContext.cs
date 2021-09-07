using Microsoft.EntityFrameworkCore;

namespace ImageSourcesStorage.DataAccessLayer.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Pin> ImageSources { get; set; }
        public DbSet<Board> Boards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pin>()
                .HasOne(p => p.Board)
                .WithMany(p => p.Pins)
                .HasForeignKey(p => p.BoardId)
                .HasPrincipalKey(p => p.BoardId)
                .HasConstraintName("FK_pin_board");
        }
    }
}
