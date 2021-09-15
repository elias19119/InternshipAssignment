namespace ImageSourceStorage.Tests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ImageSourcesStorage.Controllers;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.DataAccessLayer.Models;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Xunit;

    /// <summary>
    /// Create Test units for User Controller<see cref="UserControllerTest"/> class.
    /// </summary>
    public class UserControllerTest
    {
        private readonly UserController controller;
        private readonly Mock<IUserRepository<User>> userRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserControllerTest"/> class.
        /// </summary>
        public UserControllerTest()
        {
            this.userRepo = new Mock<IUserRepository<User>>();
            this.controller = new UserController(this.userRepo.Object);
        }

        /// <summary>
        /// should return An OK Result.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task GetAllUserAsync_should_return_OK_result()
        {
            this.userRepo.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<User>());
            var response = await this.controller.GetUsersAsync();
            Assert.NotNull(response);
            Assert.IsAssignableFrom<IActionResult>(response);
        }
    }
}
