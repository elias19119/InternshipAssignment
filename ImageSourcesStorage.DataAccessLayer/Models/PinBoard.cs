using System;
using System.Collections.Generic;
using System.Text;

namespace ImageSourcesStorage.DataAccessLayer.Models
{
    public class PinBoard
    { 
        public Guid PinBoardId { get; set; }
        public Guid PinId { get; set; }
        public Guid BoardId { get; set; }
    }
}
