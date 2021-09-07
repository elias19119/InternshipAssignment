using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace ImageSourcesStorage.DataAccessLayer
{
    public class PinContext : DbContext
    {
        public PinContext(DbContextOptions<PinContext> options)
            : base(options)
        {
        }
        public DbSet<Pin> ImageSources { get; set; }
    }
}
