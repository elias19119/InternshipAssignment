namespace ImageSourceStorage.Tests
{
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

    public class PinControllerTest
    {
        private readonly Mock<IPinRepository> pinRepository;
        private readonly Mock<IUserRepository<User>> userRepository;
        private readonly PinController pinController;

        /// <summary>
        /// Initializes a new instance of the <see cref="PinControllerTest"/> class.
        /// </summary>
        public PinControllerTest()
        {
            this.pinRepository = new Mock<IPinRepository>();
            this.userRepository = new Mock<IUserRepository<User>>();
            this.pinController = new PinController(this.pinRepository.Object);
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
    }
}
