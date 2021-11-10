namespace ImageSourcesStorage.Models
{
    using System;

    public class AddPinToBoard
    {
        public Guid UserId { get; set; }

        public Guid BoardId { get; set; }

        public Guid PinId { get; set; }
    }
}
