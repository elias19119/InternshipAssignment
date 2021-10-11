namespace ImageSourcesStorage.Models
{
    using System;

    /// <summary>
    /// PinModel.
    /// </summary>
    public class PinModel
    {
        public Guid PinId { get; set; }

        public Guid UserId { get; set; }

        public string ImagePath { get; set; }
    }
}
