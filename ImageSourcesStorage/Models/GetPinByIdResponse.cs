namespace ImageSourcesStorage.Models
{
    using ImageSourcesStorage.DataAccessLayer;

    public class GetPinByIdResponse
    {
        public PinsModel Pin { get; set; }

        public GetPinByIdResponse(Pin pin)
        {
            this.Pin = new PinsModel();
            this.Pin.Name = pin.Name;
            this.Pin.PinId = pin.PinId;
            this.Pin.ImagePath = pin.ImagePath;
            this.Pin.Description = pin.Description;
        }
    }
}
