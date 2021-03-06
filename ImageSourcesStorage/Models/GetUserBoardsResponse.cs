namespace ImageSourcesStorage.Models
{
    using System.Collections.Generic;
    using ImageSourcesStorage.DataAccessLayer.Models;

    /// <summary>
    /// The response body for get all boards of User.
    /// </summary>
    public class GetUserBoardsResponse
    {
        public List<BoardModel> BoardModels { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserBoardsResponse"/> class.
        /// </summary>
        /// <param name="boards"></param>
        public GetUserBoardsResponse(List<BoardModelDetails> boards)
        {
            this.BoardModels = new List<BoardModel>();
            foreach (var board in boards)
            {
                BoardModel boardModel = new BoardModel
                {
                    UserId = board.UserId,
                    Name = board.Name,
                    Pins = new List<PinModel>(),
                };

                foreach (var pin in boardModel.Pins)
                {
                    var pinModel = new PinModel
                    {
                        PinId = pin.PinId,
                        ImagePath = pin.ImagePath,
                        UserId = pin.UserId,
                        Description = pin.Description,
                    };
                    boardModel.Pins.Add(pinModel);
                }

                this.BoardModels.Add(boardModel);
            }
        }
    }
}
