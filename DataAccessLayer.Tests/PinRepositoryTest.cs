namespace DataAccessLayer.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.DataAccessLayer.Models;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    /// <summary>
    /// this class is to test the Pin Repository.
    /// </summary>
    public class PinRepositoryTest
    {
        private readonly DataContext dataContext;
        private readonly PinRepository pinRepository;
        private readonly PinBoardRepository pinBoardRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PinRepositoryTest"/> class.
        /// </summary>
        public PinRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "FakeConnectionString")
                .Options;
            this.dataContext = new DataContext(options);
            this.pinRepository = new PinRepository(this.dataContext);
            this.pinBoardRepository = new PinBoardRepository(this.dataContext);
        }

        /// <summary>
        /// should return An OK Result.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task GetAllPinsAsync_should_return_list_of_pins_if_pins_exists()
        {
            var pins = new List<Pin>
            {
                new Pin
                {
                    UserId = Guid.NewGuid(),
                    Name = "elias",
                    PinId = Guid.NewGuid(),
                    Description = "cars",
                },
                new Pin
                {
                    UserId = Guid.NewGuid(),
                    Name = "larisa",
                    PinId = Guid.NewGuid(),
                    Description = "nature",
                },
            };

            await this.dataContext.Pins.AddRangeAsync(pins);
            await this.dataContext.SaveChangesAsync();

            var result = await this.pinRepository.GetAllPinsAsync();

            Assert.NotEmpty(result);
            Assert.Equal(pins.Count, result.Count);
        }

        /// <summary>
        /// should return empty list.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task GetAllPinsAsync_should_return_empty_list_if_pins_do_not_exist()
        {
            var pins = new List<Pin>();

            await this.dataContext.Pins.AddRangeAsync(pins);
            await this.dataContext.SaveChangesAsync();

            var result = await this.pinRepository.GetAllPinsAsync();

            Assert.Empty(result);
            Assert.Equal(pins.Count, result.Count);
        }

        /// <summary>
        /// should return a pin.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task GetPinByIdAsync_should_return_pin_if_pin_exists()
        {
            var pin = new Pin { PinId = Guid.NewGuid() };

            await this.dataContext.Pins.AddRangeAsync(pin);
            await this.dataContext.SaveChangesAsync();

            var result = await this.pinRepository.GetPinByIdAsync(pin.PinId);

            Assert.NotNull(result);
            Assert.Equal(pin.PinId, result.PinId);
        }

        /// <summary>
        /// should not return a pin.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task GetPinByIdAsync_should_return_Null_if_pin_does_not_exist()
        {
            var testId = Guid.NewGuid();
            var pin = new Pin { PinId = Guid.NewGuid() };

            await this.dataContext.Pins.AddRangeAsync(pin);
            await this.dataContext.SaveChangesAsync();

            var result = await this.pinRepository.GetPinByIdAsync(testId);

            Assert.Null(result);
        }

        /// <summary>
        /// should return id exist.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task IsPinExistsAsync_should_return_true_if_id_exists()
        {
            var pin = new Pin { PinId = Guid.NewGuid() };

            await this.dataContext.Pins.AddRangeAsync(pin);
            await this.dataContext.SaveChangesAsync();

            var result = await this.pinRepository.IsPinExistsAsync(pin.PinId);

            Assert.True(result);
        }

        /// <summary>
        /// should return id does not exist.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task IsPinExistsAsync_should_return_false_if_id_does_not_exists()
        {
            var testId = Guid.NewGuid();
            var pin = new Pin { PinId = Guid.NewGuid() };

            await this.dataContext.Pins.AddRangeAsync(pin);
            await this.dataContext.SaveChangesAsync();

            var result = await this.pinRepository.IsPinExistsAsync(testId);

            Assert.False(result);
        }

        /// <summary>
        /// should return A created pin.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task InsertPinAsync_should_add_pin_to_context_if_data_is_valid()
        {
            var pinId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var imagePath = "C:/desktop/cars";
            var description = "BMW";

            await this.pinRepository.InsertPinAsync(pinId, userId, imagePath, description);

            var pin = await this.pinRepository.GetPinByIdAsync(pinId);

            Assert.Equal(pin.UserId, userId);
            Assert.Equal(pin.ImagePath, imagePath);
        }
    }
}
