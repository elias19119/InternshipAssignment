using System;
using System.Collections.Generic;
using System.Text;
using ImageSourcesStorage.DataAccessLayer.Models;

namespace ImageSourcesStorage.DataAccessLayer
{
    public class Pin
    {
        public Guid Id { get; set; } 
        public double Price { get; set; }
        public Format Format { get; set; }
        public string Description { get; set; }
        public Guid BoardId { get; set; }
        public Board Board { get; set; }
    }
}
