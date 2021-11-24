namespace ImageSourcesStorage.Models
{
    using System;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.DataAccessLayer.Models;

    public class GetPinByIdResponse
    {
        public Guid PinId { get; set; }

        public string Name { get; set; }

        public string ImagePath { get; set; }

        public string Description { get; set; }

        public GetPinByIdResponse(Pin pin)
        {
            this.Name = pin.Name;
            this.PinId = pin.PinId;
            this.ImagePath = pin.ImagePath;
            this.Description = pin.Description;
        }
    }
}
