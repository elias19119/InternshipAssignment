namespace ImageSourcesStorage.Controllers
{
    using System;
    using System.Threading.Tasks;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.DataAccessLayer.Models;
    using ImageSourcesStorage.Models;
    using ImageSourcesStorage.Validators;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    public class BoardController : ControllerBase
    {
        private readonly IBoardRepository boardRepository;
        private readonly IUserRepository<User> userRepository;
        private readonly IPinRepository pinRepository;
        private readonly GetUserBoardValidator getUserBoardValidator;
        private readonly AddBoardtoUserValidator addBoardValidator;
        private readonly GetBoardByIdValidator getBoardIdValidator;
        private readonly DeleteBoardOfUserValidator deleteBoardValidator;
        private readonly EditBoardofUserValidator editBoardOfUserValidator;
        private readonly DeletePinOfBoardValidator deletePinOfBoardValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="BoardController"/> class.
        /// </summary>
        public BoardController(IBoardRepository boardRepository , IUserRepository<User> userRepository, IPinRepository pinRepository)
        {
            this.boardRepository = boardRepository;
            this.userRepository = userRepository;
            this.pinRepository = pinRepository;
            this.getUserBoardValidator = new GetUserBoardValidator(userRepository);
            this.addBoardValidator = new AddBoardtoUserValidator(userRepository, boardRepository);
            this.getBoardIdValidator = new GetBoardByIdValidator(boardRepository);
            this.deleteBoardValidator = new DeleteBoardOfUserValidator(userRepository, boardRepository);
            this.editBoardOfUserValidator = new EditBoardofUserValidator(userRepository, boardRepository);
            this.deletePinOfBoardValidator = new DeletePinOfBoardValidator(pinRepository, boardRepository);
        }

        [HttpGet]
        [Route("api/users/{userId}/boards")]
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
        [Route("api/users/boards/{boardId}")]
        public async Task<IActionResult> GetUserBoardByIdAsync(Guid boardId)
        {
            var board = new Board() { BoardId = boardId };

            var result = this.getBoardIdValidator.Validate(board);

            if (!result.IsValid)
            {
               return this.BadRequest();
            }

            var boards = await this.boardRepository.GetBoardByIdAsync(boardId);
            var response = new GetBoardIdResponse(boards);

            return this.Ok(response);
        }

        [HttpPost]
        [Route("api/users/{userId}/boards")]
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
        [Route("api/users/{userId}/boards/{boardId}")]
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
        [Route("api/users/{userId}/boards/{boardId}")]
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

        [HttpDelete]
        [Route("api/boards/{boardId}/pins/{pinId}")]
        public async Task<IActionResult> DeletePinOfBoardAsync(Guid pinId, Guid boardId)
        {
            var pin = new Pin { PinId = pinId, BoardId = boardId };

            var result = this.deletePinOfBoardValidator.Validate(pin);

            if (!result.IsValid)
            {
                return this.NotFound();
            }

            await this.boardRepository.DeletePinOfBoardAsync(pinId);
            return this.NoContent();
        }
    }
}
