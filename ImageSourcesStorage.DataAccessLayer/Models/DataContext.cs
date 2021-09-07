﻿using Microsoft.EntityFrameworkCore;

namespace ImageSourcesStorage.DataAccessLayer.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Pin> Pins { get; set; }
        public DbSet<Board> Boards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pin>()
              .ToTable("ImageSources")
                .HasOne(p => p.Board);

            modelBuilder.Entity<Board>().ToTable("Board");

        }
    }
}
