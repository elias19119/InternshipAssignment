namespace ImageSourceStorage.Tests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ImageSourcesStorage.Controllers;
    using ImageSourcesStorage.DataAccessLayer;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Xunit;

    public class PinControllerTest
    {
        private readonly Mock<IPinRepository> pinRepository;
        private readonly PinController pinController;

        /// <summary>
        /// Initializes a new instance of the <see cref="PinControllerTest"/> class.
        /// </summary>
        public PinControllerTest()
        {
            this.pinRepository = new Mock<IPinRepository>();
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
    }
}
