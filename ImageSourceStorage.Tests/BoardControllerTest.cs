namespace ImageSourceStorage.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ImageSourcesStorage.Controllers;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.DataAccessLayer.Models;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="BoardControllerTest"/> class.
        /// </summary>
        public BoardControllerTest()
        {
            this.boardRepository = new Mock<IBoardRepository>();
            this.controller = new BoardController(this.boardRepository.Object);
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
    }
}
