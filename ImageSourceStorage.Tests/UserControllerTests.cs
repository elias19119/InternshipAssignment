using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImageSourcesStorage.Controllers;
using ImageSourcesStorage.DataAccessLayer;
using ImageSourcesStorage.DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ImageSourceStorage.Tests
{
    public class UserControllerTests
    {
        private UserController _controller;
        private Mock<IUserRepository<User>> _userRepo;
        public UserControllerTests()
        {
            _userRepo = new Mock<IUserRepository<User>>();
            _controller = new UserController(_userRepo.Object);
        }


        [Fact]
        public async Task GetAllUserAsync_should_return_OK_result()
        {
            _userRepo.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<User>());
            var response = await _controller.GetUsersAsync();
            Assert.NotNull(response);
            Assert.IsAssignableFrom<ActionResult<IEnumerable<User>>>(response);
        }

        [Fact]
        public async Task GetUserAsync_should_return_NotFound_if_user_not_exists()
        {
            _userRepo.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(default(User));
            var response = await _controller.GetUserAsync(Guid.NewGuid());
            Assert.NotNull(response);
            var result = response as StatusCodeResult;
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task GetUserAsync_should_return_Ok_if_user_exists()
        {
            _userRepo.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new User(new Guid("4a8eb0eb-1216-4aa9-aba1-3167743a7bc9") , "elias" , 10));
            var response = await _controller.GetUserAsync(new Guid("4a8eb0eb-1216-4aa9-aba1-3167743a7bc9"));
            Assert.NotNull(response);
            var result = response as OkObjectResult;
            Assert.IsType<OkObjectResult>(response);
        }

    }
}
