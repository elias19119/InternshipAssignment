namespace ImageSourceStorage.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using ImageSourcesStorage.Controllers;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.DataAccessLayer.Models;
    using ImageSourcesStorage.Models;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Xunit;

    /// <summary>
    /// this class is for testing the board controller.
    /// </summary>
    public class BoardControllerTest
    {
        private readonly BoardController controller;
        private readonly Mock<IBoardRepository> boardRepository;
        private readonly Mock<IUserRepository> userRepository;
        private readonly Mock<IPinRepository> pinRepository;
        private readonly Mock<IPinBoardRepository> pinBoardRepository;
        private readonly Mock<IMapper> mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="BoardControllerTest"/> class.
        /// </summary>
        public BoardControllerTest()
        {
            this.boardRepository = new Mock<IBoardRepository>();
            this.userRepository = new Mock<IUserRepository>();
            this.pinRepository = new Mock<IPinRepository>();
            this.pinBoardRepository = new Mock<IPinBoardRepository>();
            this.mapper = new Mock<IMapper>();
            this.controller = new BoardController(this.boardRepository.Object, this.userRepository.Object, this.pinRepository.Object, this.pinBoardRepository.Object, this.mapper.Object);
        }

        ///// <summary>
        ///// should return An OK Result.
        ///// </summary>
        ///// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task GetUserBoardAsync_should_return_OK_result()
        {
            this.boardRepository.Setup(x => x.GetUserBoardsAsync(It.IsAny<Guid>())).ReturnsAsync(new List<BoardModelDetails>());

            var response = await this.controller.GetUserBoardsAsync(It.IsAny<Guid>());

            Assert.NotNull(response);
            Assert.IsAssignableFrom<IActionResult>(response);
        }

        /// <summary>
        /// should return An OK Result.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task AddBoardtoUserAsync_should_return_OK_result()
        {
            var request = new AddBoardtoUserRequest()
            {
                Name = "cars",
            };

            var name = request.Name;
            var boardId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            this.userRepository.Setup(x => x.ExistsAsync(userId)).ReturnsAsync(true);
            this.boardRepository.Setup(x => x.AddBoardToUserAsync(userId, boardId, name)).Returns(Task.CompletedTask);
            this.boardRepository.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);

            var response = await this.controller.AddBoardToUserAsync(userId, request);

            Assert.NotNull(response);
            Assert.IsType<CreatedAtActionResult>(response);
        }

        /// <summary>
        /// should delete A board.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task DeleteBoardOfUserAsync_should_remove_board_if_boardId_and_userId_exists()
        {
            var userId = Guid.NewGuid();
            var boardId = Guid.NewGuid();
            var name = "cars";

            this.userRepository.Setup(x => x.ExistsAsync(userId)).ReturnsAsync(true);
            this.boardRepository.Setup(x => x.IsBoardBelongToUserAsync(boardId, userId)).ReturnsAsync(true);
            this.boardRepository.Setup(x => x.IsBoardExistsAsync(boardId)).ReturnsAsync(true);

            var result = await this.controller.DeleteBoardOfUserAsync(userId, boardId);

            Assert.IsType<NoContentResult>(result);
        }

        /// <summary>
        /// should return updated board.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task EditBoardOfUserAsync_should_update_board_if_id_exists_and_if_board_belongs_to_user()
        {
            var boardRequest = new UpdateBoardOfUserRequest()
            {
                Name = "cards",
            };

            var name = boardRequest.Name;
            var userId = Guid.NewGuid();
            var boardId = Guid.NewGuid();

            this.boardRepository.Setup(x => x.EditNameOfBoardAsync(boardId, userId, name)).Returns(Task.CompletedTask);
            this.boardRepository.Setup(x => x.IsBoardExistsAsync(boardId)).ReturnsAsync(true);
            this.boardRepository.Setup(x => x.IsNameExistsAsync(name)).ReturnsAsync(false);
            this.boardRepository.Setup(x => x.IsBoardBelongToUserAsync(boardId, userId)).ReturnsAsync(true);
            this.userRepository.Setup(x => x.ExistsAsync(userId)).ReturnsAsync(true);

            var result = await this.controller.EditNameOfBoardAsync(boardId, userId, boardRequest);

            Assert.IsType<NoContentResult>(result);
        }

        /// <summary>
        /// should delete a pin from a board.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task DeletePinOfBoardAsync_should_delete_pin_if_pin_belongs_to_board()
        {
            var boardId = Guid.NewGuid();
            var pinId = Guid.NewGuid();

            this.boardRepository.Setup(x => x.IsBoardExistsAsync(boardId)).ReturnsAsync(true);
            this.pinBoardRepository.Setup(x => x.IsPinBelongToBoardAsync(boardId, pinId)).ReturnsAsync(true);
            this.pinRepository.Setup(x => x.IsPinExistsAsync(pinId)).ReturnsAsync(true);
            this.boardRepository.Setup(x => x.DeletePinOfBoardAsync(pinId)).Returns(Task.CompletedTask);

            var result = await this.controller.DeletePinOfBoardAsync(pinId, boardId);

            Assert.IsType<NoContentResult>(result);
        }
    }
}
