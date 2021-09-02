using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing.Constraints;

namespace ImageSourcesStorage.Models
{
    public class ImageSource
    {
        public long Id { get; set; }
        public double Price { get; set; }
    }
}
