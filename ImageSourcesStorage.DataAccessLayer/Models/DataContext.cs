// <copyright file="DataContext.cs" company="INDG">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ImageSourcesStorage.DataAccessLayer.Models
{
    using Microsoft.EntityFrameworkCore;

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
