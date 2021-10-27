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
        private readonly DeleteBoardOfUserValidator deleteBoardValidator;
        private readonly EditBoardofUserValidator editBoardOfUserValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="BoardController"/> class.
        /// </summary>
        public BoardController(IBoardRepository boardRepository , IUserRepository<User> userRepository)
        {
            this.boardRepository = boardRepository;
            this.userRepository = userRepository;
            this.getUserBoardValidator = new GetUserBoardValidator(userRepository);
            this.addBoardValidator = new AddBoardtoUserValidator(userRepository, boardRepository);
            this.getBoardIdValidator = new GetBoardByIdValidator(boardRepository);
            this.deleteBoardValidator = new DeleteBoardOfUserValidator(userRepository, boardRepository);
            this.editBoardOfUserValidator = new EditBoardofUserValidator(userRepository, boardRepository);
        }

        [HttpGet]
        [Route("{userId}/boards")]
        public async Task<IActionResult> GetUserBoardAsync(Guid userId)
        {
            var result = this.getUserBoardValidator.Validate(userId);

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
            var result = this.getBoardIdValidator.Validate(boardId);

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

        [HttpDelete]
        [Route("{userId}/boards/{boardId}")]
        public async Task<IActionResult> DeleteBoardOfUserAsync(Guid userId, Guid boardId)
        {
            var board = new Board { UserId = userId, BoardId = boardId };

            var result = this.deleteBoardValidator.Validate(board);

            if (!result.IsValid)
            {
                return this.NotFound();
            }

            await this.boardRepository.DeleteBoardOfUserAsync(boardId);
            return this.NoContent();
        }

        [HttpPut]
        [Route("{userId}/boards/{boardId}")]
        public async Task<IActionResult> EditNameOfBoardAsync(Guid boardId, Guid userId, UpdateBoardOfUserRequest request)
        {
            var board = new Board
            {
                Name = request.Name,
                BoardId = boardId,
                UserId = userId,
            };

            var result = this.editBoardOfUserValidator.Validate(board);

            if (!result.IsValid)
            {
                return this.BadRequest();
            }

            await this.boardRepository.EditNameOfBoardAsync(board.BoardId, board.UserId, request.Name);
            return this.NoContent();
        }
    }
}
