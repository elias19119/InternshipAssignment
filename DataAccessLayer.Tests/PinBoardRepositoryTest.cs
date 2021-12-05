namespace DataAccessLayer.Tests
{
    using System;
    using System.Threading.Tasks;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.DataAccessLayer.Models;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class PinBoardRepositoryTest
    {
        private readonly DataContext dataContext;
        private readonly PinBoardRepository pinBoardRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PinBoardRepositoryTest"/> class.
        /// </summary>
        public PinBoardRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "FakeConnectionString")
                .Options;
            this.dataContext = new DataContext(options);
            this.pinBoardRepository = new PinBoardRepository(this.dataContext);
        }

        /// <summary>
        /// should return a pin.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task GetPinBoardByIdAsync_should_return_pinboard_if_pinboard_exists()
        {
            var pinBoardEntity = new PinBoard() { PinBoardId = Guid.NewGuid(), PinId = Guid.NewGuid(), BoardId = Guid.NewGuid() };

            await this.dataContext.PinBoards.AddAsync(pinBoardEntity);
            await this.dataContext.SaveChangesAsync();

            var result = await this.pinBoardRepository.GetPinBoardByIdAsync(pinBoardEntity.PinBoardId);

            Assert.NotNull(result);
            Assert.Equal(pinBoardEntity.PinBoardId, result.PinBoardId);
        }

        /// <summary>
        /// should return a pin.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task GetPinBoardByIdAsync_should_not_return_pinboard_if_pinboard_does_not_exists()
        {
            var pinBoardEntity = new PinBoard() { PinBoardId = Guid.NewGuid(), PinId = Guid.NewGuid(), BoardId = Guid.NewGuid() };

            await this.dataContext.PinBoards.AddAsync(pinBoardEntity);
            await this.dataContext.SaveChangesAsync();

            var result = await this.pinBoardRepository.GetPinBoardByIdAsync(Guid.NewGuid());

            Assert.Null(result);
        }
    }
}
