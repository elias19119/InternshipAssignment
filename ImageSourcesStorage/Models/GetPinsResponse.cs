namespace ImageSourcesStorage.Models
{
    using System.Collections.Generic;
    using ImageSourcesStorage.DataAccessLayer;

    public class GetPinsResponse
    {
        public List<PinsModel> pinsModel { get; set; }

        public GetPinsResponse(List<Pin> pins)
        {
            this.pinsModel = new List<PinsModel>();

            foreach (var pin in pins)
            {
                PinsModel pinModel = new PinsModel
                {
                    Name = pin.Name,
                    PinId = pin.PinId,
                    ImagePath = pin.ImagePath,
                    Description = pin.Description,
                };
                this.pinsModel.Add(pinModel);
            }
        }
    }
}
