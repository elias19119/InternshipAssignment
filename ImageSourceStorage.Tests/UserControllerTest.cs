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

    /// <summary>
    /// Create Test units for Users Controller<see cref="UserControllerTest"/> class.
    /// </summary>
    public class UserControllerTest
    {
        private readonly UserController controller;
        private readonly Mock<IUserRepository<User>> userRepository;
        private readonly Mock<IBoardRepository> boardRepository;
        private readonly Mock<IPinRepository> pinRepository;
        private readonly Mock <IStorage> storage;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserControllerTest"/> class.
        /// </summary>
        public UserControllerTest()
        {
            this.userRepository = new Mock<IUserRepository<User>>();
            this.storage = new Mock<IStorage>();
            this.pinRepository = new Mock<IPinRepository>();
            this.boardRepository = new Mock<IBoardRepository>();
            this.controller = new UserController(this.userRepository.Object, this.storage.Object, this.boardRepository.Object, this.pinRepository.Object);
        }

        /// <summary>
        /// should return An OK Result.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task GetAllUserAsync_should_return_OK_result()
        {
            this.userRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<User>());

            var response = await this.controller.GetUsersAsync();

            Assert.NotNull(response);
            Assert.IsAssignableFrom<IActionResult>(response);
        }

        /// <summary>
        /// should return a user.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task GetUserAsync_should_return_Ok_if_user_exists()
        {
            User user = new User
            {
                UserId = Guid.NewGuid(),
            };

            this.userRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user);
            this.userRepository.Setup(x => x.ExistsAsync(user.UserId)).ReturnsAsync(true);

            var response = await this.controller.GetUserAsync(user.UserId);
            var result = response as OkObjectResult;

            Assert.NotNull(response);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
        }

        /// <summary>
        /// should return A created user.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task PostUserAsync_should_return_ok()
        {
            CreateUserRequest userRequest = new CreateUserRequest
            {
                Name = "Hanna",
            };
            User user = new User
            {
                Name = userRequest.Name,
            };

            this.userRepository.Setup(x => x.InsertAsync(user)).Returns(Task.CompletedTask);

            var response = await this.controller.PostUserAsync(userRequest);

            Assert.NotNull(response);
            Assert.IsType<CreatedAtActionResult>(response);
        }

        /// <summary>
        /// should return A created user.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task DeleteUserAsync_should_remove_User_if_id_exists()
        {
            User user = new User()
            {
                UserId = Guid.NewGuid(),
            };

            this.userRepository.Setup(x => x.GetByIdAsync(user.UserId)).ReturnsAsync(user);
            this.userRepository.Setup(x => x.DeleteAsync(user.UserId)).Returns(Task.CompletedTask);
            this.userRepository.Setup(x => x.ExistsAsync(user.UserId)).ReturnsAsync(true);

            var result = await this.controller.DeleteUserAsync(user.UserId);

            Assert.IsType<NoContentResult>(result);
        }

        /// <summary>
        /// should return updated user.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task PutUserAsync_should_update_User_if_id_exists_and_if_name_is_unique()
        {
            UpdateUserRequest userRequest = new UpdateUserRequest()
            {
                Name = "sana",
                Score = 20,
            };

            User user = new User()
            {
                Name = userRequest.Name,
                Score = userRequest.Score,
                UserId = Guid.NewGuid(),
            };

            this.userRepository.Setup(x => x.GetByIdAsync(user.UserId)).ReturnsAsync(user);
            this.userRepository.Setup(x => x.UpdateAsync(user.UserId, user.Name, user.Score)).Returns(Task.CompletedTask);
            this.userRepository.Setup(x => x.NameExistsAsync(user.Name)).ReturnsAsync(false);
            this.userRepository.Setup(x => x.ExistsAsync(user.UserId)).ReturnsAsync(true);

            var result = await this.controller.PutUserAsync(userRequest, user.UserId);

            Assert.IsType<NoContentResult>(result);
        }

        /// <summary>
        ///  should change score if user is valid.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task ChangeUserScoreAsync_should_change_score_if_user_is_valid()
        {
            User user = new User()
            {
                Name = "Elias",
                Score = 59,
                UserId = Guid.NewGuid(),
            };

            this.userRepository.Setup(x => x.NameExistsAsync(user.Name)).ReturnsAsync(false);
            this.userRepository.Setup(x => x.ExistsAsync(user.UserId)).ReturnsAsync(true);
            this.userRepository.Setup(x => x.ChangeUserScore(user.UserId, ChangeScoreOptions.Increase)).Returns(Task.CompletedTask);

            var result = await this.controller.ChangeUserScoreAsync(user.UserId, ChangeScoreOptions.Decrease);

            Assert.IsType<NoContentResult>(result);
        }

        /// <summary>
        /// should return An OK Result.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task GetUserPinsAsync_should_return_OK_result()
        {
            this.userRepository.Setup(x => x.GetUserPinsAsync(It.IsAny<Guid>())).ReturnsAsync(new List<Pin>());

            var response = await this.controller.GetUserPinsAsync(It.IsAny<Guid>());

            Assert.NotNull(response);
            Assert.IsAssignableFrom<IActionResult>(response);
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

            var pin = new Pin { BoardId = boardId, UserId = userId, ImagePath = request.File.FileName};

            this.userRepository.Setup(x => x.ExistsAsync(userId)).ReturnsAsync(true);
            this.boardRepository.Setup(x => x.IsBoardExistsAsync(boardId)).ReturnsAsync(true);
            this.boardRepository.Setup(x => x.IsBoardBelongToUserAsync(boardId, userId)).ReturnsAsync(true);
            this.pinRepository.Setup(x => x.IsPinBelongToBoardAsync(boardId, (Guid)pinId)).ReturnsAsync(false);
            this.pinRepository.Setup(x => x.InsertPinAsync(pin)).Returns(Task.CompletedTask);

            this.pinRepository.Setup(x=>x.GetPinByIdAsync(pin.PinId)).ReturnsAsync(pin);
            var result = await this.controller.UploadFileAsync(request, userId, boardId);

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

            var pinBoard = new PinBoard() { PinId = (Guid)request.PinId, BoardId = boardId };

            this.userRepository.Setup(x => x.ExistsAsync(userId)).ReturnsAsync(true);
            this.boardRepository.Setup(x => x.IsBoardExistsAsync(boardId)).ReturnsAsync(true);
            this.boardRepository.Setup(x => x.IsBoardBelongToUserAsync(boardId, userId)).ReturnsAsync(true);
            this.pinRepository.Setup(x => x.IsPinBelongToBoardAsync(boardId, (Guid)pinId)).ReturnsAsync(false);
            this.pinRepository.Setup(x => x.InsertPinBoard(pinBoard)).Returns(Task.CompletedTask);

            var result = await this.controller.UploadFileAsync(request, userId, boardId);

            Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
