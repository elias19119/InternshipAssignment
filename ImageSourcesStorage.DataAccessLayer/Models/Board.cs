namespace ImageSourcesStorage.DataAccessLayer.Models
{
    using System;
    using System.Collections.Generic;

    public class Board
    {
        public Guid BoardId { get; set; }

        public User Owner { get; set; }

        public Guid UserId { get; set; }

        public string Name { get; set; }
    }
}
