namespace ImageSourcesStorage.Models
{
    using System;
    using ImageSourcesStorage.DataAccessLayer;

    public class UploadImageResponse
    {
        public Guid PinId { get; set; }

        public UploadImageResponse(Guid pinId)
        {
            this.PinId = pinId;
        }

        public UploadImageResponse()
        {

        }
    }
}
