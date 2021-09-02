using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ImageSourcesStorage.Models
{
    public class ImageSourceContext : DbContext
    {

        public ImageSourceContext(DbContextOptions<ImageSourceContext> options)
        :base(options)
        {}
        public DbSet<ImageSource> ImageSources { get; set; }
    }
}
