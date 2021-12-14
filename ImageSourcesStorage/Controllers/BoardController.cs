namespace ImageSourcesStorage.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.DataAccessLayer.Models;
    using ImageSourcesStorage.Models;
    using ImageSourcesStorage.Validators;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BoardController : ControllerBase
    {
        private readonly IBoardRepository boardRepository;
        private readonly IUserRepository userRepository;
        private readonly IPinRepository pinRepository;
        private readonly IPinBoardRepository pinBoardRepository;
        private readonly IMapper mapper;
        private readonly GetUserBoardValidator getUserBoardValidator;
        private readonly AddBoardtoUserValidator addBoardValidator;
        private readonly GetBoardByIdValidator getBoardIdValidator;
        private readonly DeleteBoardOfUserValidator deleteBoardValidator;
        private readonly EditBoardofUserValidator editBoardOfUserValidator;
        private readonly DeletePinOfBoardValidator deletePinOfBoardValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="BoardController"/> class.
        /// </summary>
        public BoardController(IBoardRepository boardRepository , IUserRepository userRepository, IPinRepository pinRepository, IPinBoardRepository pinBoardRepository, IMapper mapper)
        {
            this.boardRepository = boardRepository;
            this.userRepository = userRepository;
            this.pinRepository = pinRepository;
            this.pinBoardRepository = pinBoardRepository;
            this.mapper = mapper;
            this.getUserBoardValidator = new GetUserBoardValidator(userRepository);
            this.addBoardValidator = new AddBoardtoUserValidator(userRepository, boardRepository);
            this.getBoardIdValidator = new GetBoardByIdValidator(boardRepository);
            this.deleteBoardValidator = new DeleteBoardOfUserValidator(userRepository, boardRepository);
            this.editBoardOfUserValidator = new EditBoardofUserValidator(userRepository, boardRepository);
            this.deletePinOfBoardValidator = new DeletePinOfBoardValidator(pinRepository, boardRepository, pinBoardRepository);
        }

        /// <summary>
        /// Gets boards by userid.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>200.</returns>
        /// <response code="404"> userId Not Found.</response>
        [HttpGet]
        [Route("api/users/{userId}/boards")]
        [ProducesResponseType(typeof(List<BoardModelDetails>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserBoardsAsync(Guid userId)
        {
            var board = new BoardModelDetails() { UserId = userId };

            var result = this.getUserBoardValidator.Validate(board);

            if (!result.IsValid)
            {
               return this.NotFound();
            }

            var boards = await this.boardRepository.GetUserBoardsAsync(userId);

            List<BoardModelDetails> boardsResponse = this.mapper.Map<List<BoardModelDetails>>(boards);

            return this.Ok(boardsResponse);
        }

        /// <summary>
        /// Gets a board by id.
        /// </summary>
        /// <param name="boardId"></param>
        /// <returns>200.</returns>
        /// <response code="404"> boardId Not Found.</response>
        [HttpGet]
        [Route("api/users/boards/{boardId}")]
        [ProducesResponseType(typeof(GetBoardIdResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserBoardByIdAsync(Guid boardId)
        {
            var board = new Board() { BoardId = boardId };

            var result = this.getBoardIdValidator.Validate(board);

            if (!result.IsValid)
            {
                return this.NotFound();
            }

            var boards = await this.boardRepository.GetBoardByIdAsync(boardId);
            var response = new GetBoardIdResponse(boards);

            return this.Ok(response);
        }

        /// <summary>
        /// Adds a board to a user.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="request"></param>
        /// <returns>201.</returns>
        /// <response code="404"> userId Not Found.</response>
        [HttpPost]
        [Route("api/users/{userId}/boards")]
        [ProducesResponseType(typeof(AddBoardtoUserResponse), StatusCodes.Status201Created)]
        public async Task<IActionResult> AddBoardToUserAsync(Guid userId, AddBoardtoUserRequest request)
        {
            Board board = new Board()
            {
                UserId = userId,
                Name = request.Name,
                BoardId = Guid.NewGuid(),
            };

            var result = this.addBoardValidator.Validate(board);

            if (!result.IsValid)
            {
                return this.NotFound();
            }

            await this.boardRepository.AddBoardToUserAsync(userId, board.BoardId, board.Name);
            var response = new AddBoardtoUserResponse(board.BoardId);

            return this.CreatedAtAction(nameof(this.GetUserBoardByIdAsync), new { board.BoardId }, response);
        }

        /// <summary>
        /// Deletes a board of a user.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="boardId"></param>
        /// <returns>204.</returns>
        /// <response code="404"> userId or boardId Not Found.</response>
        [HttpDelete]
        [Route("api/users/{userId}/boards/{boardId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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

        /// <summary>
        /// Edits the name of the board.
        /// </summary>
        /// <param name="boardId"></param>
        /// <param name="userId"></param>
        /// <param name="request"></param>
        /// <returns>204.</returns>
        /// <response code="404"> userId or boardId Not Found.</response>
        [HttpPut]
        [Route("api/users/{userId}/boards/{boardId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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

        /// <summary>
        /// Deletes a pin from a board.
        /// </summary>
        /// <param name="pinId"></param>
        /// <param name="boardId"></param>
        /// <returns>204.</returns>
        /// <response code="404"> pinId or boardId Not Found.</response>
        [HttpDelete]
        [Route("api/boards/{boardId}/pins/{pinId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeletePinOfBoardAsync(Guid pinId, Guid boardId)
        {
            var pinBoard = new PinBoard { PinId = pinId, BoardId = boardId };

            var result = this.deletePinOfBoardValidator.Validate(pinBoard);

            if (!result.IsValid)
            {
                return this.NotFound();
            }

            await this.boardRepository.DeletePinOfBoardAsync(pinId);
            return this.NoContent();
        }
    }
}
