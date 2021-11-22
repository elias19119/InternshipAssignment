namespace ImageSourcesStorage.Models
{
    using System;
    using Microsoft.AspNetCore.Http;

    public class UploadImageRequest
    {
        public IFormFile File { get; set; }

        public Guid? PinId { get; set; }

        public string Description { get; set; }
    }
}
