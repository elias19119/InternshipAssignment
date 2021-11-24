namespace ImageSourcesStorage.DataAccessLayer.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class PinModel
    {
        public Guid PinId { get; set; }

        public Guid UserId { get; set; }

        public string ImagePath { get; set; }

        public string Description { get; set; }
    }
}
