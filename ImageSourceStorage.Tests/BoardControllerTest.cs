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
            this.boardRepository.Setup(x => x.AddBoardToUserAsync(userId, boardId, name));
            this.boardRepository.Setup(x => x.SaveAsync());

            var response = await this.controller.AddBoardToUserAsync(userId, request);

            Assert.NotNull(response);
            Assert.IsType<CreatedAtActionResult>(response);
        }
    }
}
