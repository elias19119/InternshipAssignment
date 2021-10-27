namespace ImageSourcesStorage.Models
{
    using System.Collections.Generic;
    using ImageSourcesStorage.DataAccessLayer;

    public class GetUserPinsResponse
    {
        public List<PinsModel> PinsModel { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserPinsResponse"/> class.
        /// </summary>
        /// <param name="pins"></param>
        public GetUserPinsResponse(List<Pin> pins)
        {
            this.PinsModel = new List<PinsModel>();

            foreach (var pin in pins)
            {
                var pinModel = new PinsModel
                {
                    PinId = pin.PinId,
                    ImagePath = pin.ImagePath,
                    Name = pin.Name,
                    Description = pin.Description,
                };
                this.PinsModel.Add(pinModel);
            }
        }
    }
}
