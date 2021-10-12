namespace ImageSourcesStorage.Models
{
    using System;

    public class GetBoardIdResponse
    {
        public Guid boardId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetBoardIdResponse"/> class.
        /// </summary>
        /// <param name="boardId"></param>
        public GetBoardIdResponse(Guid boardId)
        {
            this.boardId = boardId;
        }
    }
}
