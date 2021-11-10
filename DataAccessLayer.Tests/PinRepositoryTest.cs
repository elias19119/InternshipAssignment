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

            await this.dataContext.AddRangeAsync(pins);
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

            await this.dataContext.AddRangeAsync(pins);
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

            await this.dataContext.AddAsync(pin);
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

            await this.dataContext.AddAsync(pin);
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

            await this.dataContext.AddAsync(pin);
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

            await this.dataContext.AddAsync(pin);
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
            var pinEntity = new Pin { PinId = Guid.NewGuid(), BoardId = Guid.NewGuid(), UserId = Guid.NewGuid(), ImagePath = "C:/desktop/cars" };

            await this.dataContext.AddAsync(pinEntity);
            await this.dataContext.SaveChangesAsync();

            await this.pinRepository.InsertPinAsync(pinEntity);

            var pin = await this.pinRepository.GetPinByIdAsync(pinEntity.PinId);

            Assert.Equal(pin.BoardId, pinEntity.BoardId);
            Assert.Equal(pin.UserId, pinEntity.UserId);
            Assert.Equal(pin.ImagePath, pinEntity.ImagePath);
        }

        /// <summary>
        /// should return null.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task InsertPinAsync_should_not_add_pin_to_context_if_data_is_not_valid()
        {
            var pinEntity = new Pin { PinId = Guid.NewGuid(), BoardId = Guid.NewGuid(), UserId = Guid.NewGuid(), ImagePath = "C:/desktop/cars" };

            await this.pinRepository.InsertPinAsync(pinEntity);
            var pin = await this.pinRepository.GetPinByIdAsync(Guid.NewGuid());

            Assert.Null(pin);
        }

        /// <summary>
        /// should return A created pin.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task InsertPinBoard_should_add_pinboard_to_context_if_data_is_valid()
        {
            var pinBoardEntity = new PinBoard() { PinBoardId = Guid.NewGuid(), PinId = Guid.NewGuid(), BoardId = Guid.NewGuid() };

            await this.dataContext.AddAsync(pinBoardEntity);
            await this.dataContext.SaveChangesAsync();

            await this.pinRepository.InsertPinBoard(pinBoardEntity);

            var pinBoard = await this.pinRepository.GetPinBoardByIdAsync(pinBoardEntity.PinBoardId);

            Assert.Equal(pinBoard.PinId, pinBoardEntity.PinId);
            Assert.Equal(pinBoard.BoardId, pinBoardEntity.BoardId);
        }

        /// <summary>
        /// should return null.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task InsertPinBoard_should_not_add_pinboard_to_context_if_data_is_not_valid()
        {
            var pinBoardEntity = new PinBoard() { PinBoardId = Guid.NewGuid(), PinId = Guid.NewGuid(), BoardId = Guid.NewGuid() };

            await this.pinRepository.InsertPinBoard(pinBoardEntity);

            var pinBoard = await this.pinRepository.GetPinBoardByIdAsync(Guid.NewGuid());

            Assert.Null(pinBoard);
        }

        /// <summary>
        /// should return a pin.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task GetPinBoardByIdAsync_should_return_pinboard_if_pinboard_exists()
        {
            var pinBoardEntity = new PinBoard() { PinBoardId = Guid.NewGuid(), PinId = Guid.NewGuid(), BoardId = Guid.NewGuid() };

            await this.dataContext.AddAsync(pinBoardEntity);
            await this.dataContext.SaveChangesAsync();

            var result = await this.pinRepository.GetPinBoardByIdAsync(pinBoardEntity.PinBoardId);

            Assert.NotNull(result);
            Assert.Equal(pinBoardEntity.PinBoardId, result.PinBoardId);
        }

        /// <summary>
        /// should not return a pin.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task GetPinBoardByIdAsync_should_return_Null_if_pinboard_does_not_exist()
        {
            var pinBoardEntity = new PinBoard() { PinBoardId = Guid.NewGuid(), PinId = Guid.NewGuid(), BoardId = Guid.NewGuid() };

            await this.dataContext.AddAsync(pinBoardEntity);
            await this.dataContext.SaveChangesAsync();

            var result = await this.pinRepository.GetPinBoardByIdAsync(Guid.NewGuid());

            Assert.Null(result);
        }

    }
}
