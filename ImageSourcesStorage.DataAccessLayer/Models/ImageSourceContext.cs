using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace ImageSourcesStorage.DataAccessLayer
{
    public class ImageSourceContext : DbContext
    {
        public ImageSourceContext(DbContextOptions<ImageSourceContext> options)
            : base(options)
        {
        }
        public DbSet<ImageSource> ImageSources { get; set; }
    }
}
