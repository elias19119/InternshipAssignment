namespace ImageSourcesStorage.DataAccessLayer.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Pin
    {
        public Guid PinId { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }

        public string ImagePath { get; set; }

        public Guid UserId { get; set; }

        public User Owner { get; set; }
    }
}
