namespace ImageSourcesStorage.Models
{
    using System;

    /// <summary>
    /// this is the response for Add board to user.
    /// </summary>
    public class AddBoardtoUserResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddBoardtoUserResponse"/> class.
        /// </summary>
        /// <param name="boardId"></param>
        public AddBoardtoUserResponse(Guid boardId)
        {
            this.boardId = boardId;
        }

        public Guid boardId { get; set; }
    }
}
