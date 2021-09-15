using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ImageSourcesStorage.Controllers;
using ImageSourcesStorage.DataAccessLayer;
using ImageSourcesStorage.DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ImageSourcesStorage.Tests
{
    public class UserControllerTests
    {
        private UserController _controller;
        private Mock<IUserRepository> _userRepo;

        public UserControllerTests()
        {

            _userRepo = new Mock<IUserRepository>();
            
            _controller = new UserController(_userRepo.Object);
        }
StyleCop.Error.MSBuild 

        [Fact]
        public async Task GetAllUserAsync_should_return_OK_result()
        {
            _userRepo.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<User>());


            var response = await _controller.GetAllUserAsync();
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
            throw new NotImplementedException();
        }

    }
}
