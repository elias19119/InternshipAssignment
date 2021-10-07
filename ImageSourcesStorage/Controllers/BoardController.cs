namespace ImageSourcesStorage.Controllers
{
    using System;
    using System.Threading.Tasks;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.DataAccessLayer.Models;
    using ImageSourcesStorage.DataAccessLayer.Validators;
    using ImageSourcesStorage.Models;
    using ImageSourcesStorage.Validators;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/users")]
    [ApiController]
    public class BoardController : ControllerBase
    {
        private readonly IBoardRepository boardRepository;
        private readonly GetUserBoardValidator getUserIdboardValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="BoardController"/> class.
        /// </summary>
        public BoardController(IBoardRepository boardRepository)
        {
            this.boardRepository = boardRepository;
            this.getUserIdboardValidator = new GetUserBoardValidator(boardRepository);
        }

        [HttpGet]
        [Route("{userId}/boards")]
        public async Task<IActionResult> GetUserBoardAsync(Guid userId)
        {
            var board = new Board() { UserId = userId };

            var result = this.getUserIdboardValidator.Validate(board);

            if (!result.IsValid)
            {
               return this.NotFound();
            }

            var boards = await this.boardRepository.GetUserBoardAsync(userId);
            var respnse = new GetUserBoardResponse(boards);

            return this.Ok(respnse);
        }
    }
}
