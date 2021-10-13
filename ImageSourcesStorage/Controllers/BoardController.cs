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
        private readonly GetBoardByIdValidator getBoardIdValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="BoardController"/> class.
        /// </summary>
        public BoardController(IBoardRepository boardRepository , IUserRepository<User> userRepository)
        {
            this.boardRepository = boardRepository;
            this.userRepository = userRepository;
            this.getUserBoardValidator = new GetUserBoardValidator(userRepository);
            this.addBoardValidator = new AddBoardtoUserValidator(userRepository, boardRepository);
            this.getBoardIdValidator = new GetBoardByIdValidator(this.boardRepository);
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

        [HttpGet]
        [Route("boards/{boardId}")]
        public async Task<IActionResult> GetUserBoardByIdAsync(Guid boardId)
        {
            var board = new Board() { BoardId = boardId };

            var result = this.getBoardIdValidator.Validate(board);

            if (!result.IsValid)
            {
               return this.BadRequest();
            }

            var boards = await this.boardRepository.GetBoardByIdAsync(boardId);
            var response = new GetBoardIdResponse(boards.BoardId);

            return this.Ok(response);
        }

        [HttpPost]
        [Route("{userId}/boards")]
        public async Task<IActionResult> AddBoardToUserAsync(Guid userId, AddBoardtoUserRequest request)
        {
            var boardId = Guid.NewGuid();
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

            await this.boardRepository.AddBoardToUserAsync(userId, board.BoardId, board.Name);
            var response = new AddBoardtoUserResponse(boardId);

            return this.CreatedAtAction(nameof(this.GetUserBoardByIdAsync), new { boardId }, response);
        }
    }
}
