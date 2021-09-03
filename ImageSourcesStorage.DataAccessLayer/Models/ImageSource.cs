using System;
using System.Collections.Generic;
using System.Text;
using ImageSourcesStorage.DataAccessLayer.Models;

namespace ImageSourcesStorage.DataAccessLayer
{
    public class ImageSource
    {
        public Guid Id { get; set; } 
        public double Price { get; set; }
        public Format Format { get; set; }
    }
}
