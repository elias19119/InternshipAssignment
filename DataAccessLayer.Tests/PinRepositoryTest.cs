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
    }
}
