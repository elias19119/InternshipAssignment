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
        private readonly IUserRepository<User> userRepository;
        private readonly GetUserBoardValidator getUserBoardValidator;
        private readonly AddBoardtoUserValidator addBoardValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="BoardController"/> class.
        /// </summary>
        public BoardController(IBoardRepository boardRepository , IUserRepository<User> userRepository)
        {
            this.boardRepository = boardRepository;
            this.userRepository = userRepository;
            this.getUserBoardValidator = new GetUserBoardValidator(userRepository);
            this.addBoardValidator = new AddBoardtoUserValidator(userRepository, boardRepository);
        }

        [HttpGet]
        [Route("{userId}/boards")]
        public async Task<IActionResult> GetUserBoardAsync(Guid userId)
        {
            var board = new Board() { UserId = userId };

            var result = this.getUserBoardValidator.Validate(board);

            if (!result.IsValid)
            {
               return this.NotFound();
            }

            var boards = await this.boardRepository.GetUserBoardAsync(userId);
            var response = new GetUserBoardResponse(boards);

            return this.Ok(response);
        }

        [HttpPost]
        [Route("{userId}/boards")]
        public async Task<IActionResult> PostBoardtoUserAsync(Guid userId, PostBoardtoUserRequest request)
        {
            Board board = new Board()
            {
                UserId = userId,
                Name = request.Name,
            };

            var result = this.addBoardValidator.Validate(board);

            if (!result.IsValid)
            {
                return this.NotFound();
            }

            await this.boardRepository.PostBoardtoUserAsync(userId, board);

            var response = new PostBoardtoUserResponse(board.BoardId);

            return this.CreatedAtAction("PostBoardtoUser", new { id = board.UserId }, response);
        }

    }
}
