namespace ImageSourcesStorage.Models
{
    using System;
    using System.Collections.Generic;

    public class BoardModel
    {
        public string Name { get; set; }

        public Guid UserId { get; set; }

        public List<PinModel> Pins { get; set; }
    }
}
