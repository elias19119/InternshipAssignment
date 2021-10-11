namespace ImageSourceStorage.Tests
{
    using System;
    using System.Threading.Tasks;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.DataAccessLayer.Models;
    using ImageSourcesStorage.Validators;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    /// <summary>
    /// test class for get user board.
    /// </summary>
    public class GetUserBoardValidatorTest
    {
        private readonly GetUserBoardValidator getUserBoardValidator;
        private readonly UserRepository<User> userRepository;
        private readonly DataContext dataContext;

        public GetUserBoardValidatorTest()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
              .UseInMemoryDatabase(databaseName: "FakeConnectionString")
              .Options;
            this.dataContext = new DataContext(options);
            this.userRepository = new UserRepository<User>(this.dataContext);
            this.getUserBoardValidator = new GetUserBoardValidator(this.userRepository);
        }

        /// <summary>
        /// should return id does not exists .
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task Validate_should_return_false_if_id_does_not_exists()
        {
            Board board = new Board()
            {
                UserId = Guid.NewGuid(),
            };

            await this.userRepository.GetByIdAsync(Guid.NewGuid());

            var result = this.getUserBoardValidator.Validate(board);

            Assert.False(result.IsValid);
        }

        /// <summary>
        /// should return id exists .
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task Validate_should_return_true_if_id_exists()
        {

            User user = new User()
            {
                UserId = Guid.NewGuid(),
            };

            Board board = new Board()
            {
                UserId = user.UserId,
            };

            await this.dataContext.AddAsync(user);
            await this.dataContext.SaveChangesAsync();

            await this.userRepository.GetByIdAsync(board.UserId);

            var result = this.getUserBoardValidator.Validate(board);

            Assert.True(result.IsValid);
        }
    }
}
