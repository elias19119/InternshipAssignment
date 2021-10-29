namespace ImageSourcesStorage.Models
{
    using System;
    using ImageSourcesStorage.DataAccessLayer.Models;

    public class GetBoardIdResponse
    {
        public Guid boardId { get; set; }

        public Guid UserId { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetBoardIdResponse"/> class.
        /// </summary>
        /// <param name="boardId"></param>
        public GetBoardIdResponse(Board board)
        {
            this.boardId = board.BoardId;
            this.Name = board.Name;
            this.UserId = board.UserId;
        }
    }
}
