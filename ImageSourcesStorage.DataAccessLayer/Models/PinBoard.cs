namespace ImageSourcesStorage.DataAccessLayer.Models
{
    using System;

    public class PinBoard
    {
        public Guid PinBoardId { get; set; }

        public Guid PinId { get; set; }

        public Guid BoardId { get; set; }
    }
}
