namespace ImageSourceStorage.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using ImageSourcesStorage.Controllers;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.DataAccessLayer.Models;
    using ImageSourcesStorage.Models;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="UserControllerTest"/> class.
        /// </summary>
        public UserControllerTest()
        {
            this.userRepository = new Mock<IUserRepository<User>>();
            this.controller = new UserController(this.userRepository.Object);
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

            this.userRepository.Setup(x => x.InsertAsync(user));

            var response = await this.controller.PostUserAsync(userRequest);

            Assert.NotNull(response);
            Assert.IsType<CreatedAtActionResult>(response);
        }

        /// <summary>
        /// should return A created user.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task DeleteUserAsync_should_remove_User_if_id_exist()
        {
            var userId = Guid.NewGuid();
            this.userRepository.Setup(user => user.GetByIdAsync(userId)).ReturnsAsync(new User() { });
            this.userRepository.Setup(user => user.DeleteAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);

            await this.controller.DeleteUserAsync(userId);

            this.userRepository.Verify(user => user.DeleteAsync(It.IsAny<Guid>()));
        }
    }
}
