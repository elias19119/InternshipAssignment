namespace ImageSourcesStorage.DataAccessLayer
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using ImageSourcesStorage.DataAccessLayer.Models;

    public class Pin
    {
        public Guid PinId { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }

        public Guid UserId { get; set; }

        public User Owner { get; set; }
    }
}
