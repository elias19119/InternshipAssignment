namespace ImageSourcesStorage.Models
{
    using System;
    using ImageSourcesStorage.DataAccessLayer;

    public class UploadImageResponse
    {
        public Guid PinId { get; set; }

        public UploadImageResponse(Pin pin)
        {
            this.PinId = pin.PinId;
        }

        public UploadImageResponse()
        {

        }
    }
}
