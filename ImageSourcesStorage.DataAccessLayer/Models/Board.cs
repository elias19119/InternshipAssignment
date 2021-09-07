using System;
using System.Collections.Generic;
using System.Text;

namespace ImageSourcesStorage.DataAccessLayer.Models
{
    public class Board
    {
        public Guid BoardId { get; set; }
        public User User { get; set; }
        public string Name { get; set; }

        public List<Pin> Pins { get; set; }

    }
}
