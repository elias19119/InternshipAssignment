namespace ImageSourceStorage.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;
    using ImageSourcesStorage.Controllers;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.DataAccessLayer.Models;
    using ImageSourcesStorage.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Xunit;

    public class PinControllerTest
    {
        private readonly Mock<IPinRepository> pinRepository;
        private readonly Mock<IUserRepository<User>> userRepository;
        private readonly Mock<IBoardRepository> boardRepository;
        private readonly Mock<IPinBoardRepository<PinBoard>> pinBoardRepository;
        private readonly PinController pinController;
        private readonly Mock<IStorage> storage;

        /// <summary>
        /// Initializes a new instance of the <see cref="PinControllerTest"/> class.
        /// </summary>
        public PinControllerTest()
        {
            this.pinRepository = new Mock<IPinRepository>();
            this.userRepository = new Mock<IUserRepository<User>>();
            this.boardRepository = new Mock<IBoardRepository>();
            this.storage = new Mock<IStorage>();
            this.pinBoardRepository = new Mock<IPinBoardRepository<PinBoard>>();
            this.pinController = new PinController(this.pinRepository.Object, this.userRepository.Object, this.storage.Object, this.boardRepository.Object, this.pinBoardRepository.Object);
        }

        /// <summary>
        /// should return An OK Result.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task GetAllPinsAsync_should_return_OK_result()
        {
            this.pinRepository.Setup(x => x.GetAllPinsAsync()).ReturnsAsync(new List<Pin>());

            var response = await this.pinController.GetAllPinsAsync();

            Assert.NotNull(response);
            Assert.IsAssignableFrom<IActionResult>(response);
        }

        /// <summary>
        /// should return a pin.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task GetPinByIdAsync_should_return_Ok_if_pin_exists()
        {
            var pin = new Pin
            {
                PinId = Guid.NewGuid(),
            };

            this.pinRepository.Setup(x => x.GetPinByIdAsync(It.IsAny<Guid>())).ReturnsAsync(pin);
            this.pinRepository.Setup(x => x.IsPinExistsAsync(pin.PinId)).ReturnsAsync(true);

            var response = await this.pinController.GetPinByIdAsync(pin.PinId);
            var result = response as OkObjectResult;

            Assert.NotNull(response);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
        }

        /// <summary>
        /// should upload a file.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task UploadFileAsync_Should_Upload_File_if_data_are_valid()
        {
            var fileMock = new Mock<IFormFile>();

            var content = "this is a test file";
            var fileName = "test.pdf";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);

            var file = fileMock.Object;

            var request = new UploadImageRequest
            {
                File = file,
                PinId = Guid.Empty,
            };

            var boardId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var pinId = request.PinId;
            var description = "BMW";

            var pin = new Pin {UserId = userId, ImagePath = request.File.FileName };

            this.userRepository.Setup(x => x.ExistsAsync(userId)).ReturnsAsync(true);
            this.boardRepository.Setup(x => x.IsBoardExistsAsync(boardId)).ReturnsAsync(true);
            this.boardRepository.Setup(x => x.IsBoardBelongToUserAsync(boardId, userId)).ReturnsAsync(true);
            this.pinBoardRepository.Setup(x => x.IsPinBelongToBoardAsync(boardId, (Guid)pinId)).ReturnsAsync(false);
            this.pinRepository.Setup(x => x.InsertPinAsync((Guid)pinId, userId, file.FileName, description)).Returns(Task.CompletedTask);

            var result = await this.pinController.AddPinToTheBoardAsync(request, userId, boardId);

            Assert.IsAssignableFrom<IActionResult>(result);
        }

        /// <summary>
        /// should upload a file.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task UploadFileAsync_Should_add_a_pinboard_if_data_are_valid()
        {
            var request = new UploadImageRequest
            {
                File = null,
                PinId = Guid.NewGuid(),
            };

            var boardId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var pinId = request.PinId;

            this.userRepository.Setup(x => x.ExistsAsync(userId)).ReturnsAsync(true);
            this.boardRepository.Setup(x => x.IsBoardExistsAsync(boardId)).ReturnsAsync(true);
            this.boardRepository.Setup(x => x.IsBoardBelongToUserAsync(boardId, userId)).ReturnsAsync(true);
            this.pinBoardRepository.Setup(x => x.IsPinBelongToBoardAsync(boardId, (Guid)pinId)).ReturnsAsync(false);
            this.pinBoardRepository.Setup(x => x.InsertPinBoard(boardId, (Guid)pinId)).Returns(Task.CompletedTask);

            var result = await this.pinController.AddPinToTheBoardAsync(request, userId, boardId);

            Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
