namespace ImageSourcesStorage.DataAccessLayer.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class BoardModelDetails
    {
        public string Name { get; set; }

        public Guid UserId { get; set; }

        public List<PinModel> Pins { get; set; }
    }
}
