namespace ImageSourceStorage.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
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
        private readonly Mock<IUserRepository<User>> userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="BoardControllerTest"/> class.
        /// </summary>
        public BoardControllerTest()
        {
            this.boardRepository = new Mock<IBoardRepository>();
            this.userRepository = new Mock<IUserRepository<User>>();
            this.controller = new BoardController(this.boardRepository.Object, this.userRepository.Object);
        }

        /// <summary>
        /// should return An OK Result.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task GetUserBoardAsync_should_return_OK_result()
        {
            this.boardRepository.Setup(x => x.GetUserBoardAsync(It.IsAny<Guid>())).ReturnsAsync(new List<Board>());

            var response = await this.controller.GetUserBoardAsync(It.IsAny<Guid>());

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

            this.boardRepository.Setup(x => x.EditBoardOfUserAsync(boardId, userId, name)).Returns(Task.CompletedTask);
            this.boardRepository.Setup(x => x.IsBoardExistsAsync(boardId)).ReturnsAsync(true);
            this.boardRepository.Setup(x => x.IsNameExistsAsync(name)).ReturnsAsync(false);
            this.boardRepository.Setup(x => x.IsBoardBelongToUserAsync(boardId, userId)).ReturnsAsync(true);
            this.userRepository.Setup(x => x.ExistsAsync(userId)).ReturnsAsync(true);

            var result = await this.controller.EditBoardOfUserAsync(boardId, userId, boardRequest);

            Assert.IsType<NoContentResult>(result);
        }
    }
}
