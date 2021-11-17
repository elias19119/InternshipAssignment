namespace ImageSourcesStorage.DataAccessLayer.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class BoardEntity
    {
        public string Name { get; set; }

        public Guid UserId { get; set; }

        public List<PinModel> pins { get; set; }
    }
}
