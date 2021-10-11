namespace ImageSourcesStorage.Models
{
    using System;

    /// <summary>
    /// this is the response for Add board to user.
    /// </summary>
    public class PostBoardtoUserResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PostBoardtoUserResponse"/> class.
        /// </summary>
        /// <param name="boardId"></param>
        public PostBoardtoUserResponse(Guid boardId)
        {
            this.BoardId = boardId;
        }

        public Guid BoardId { get; set; }
    }
}
