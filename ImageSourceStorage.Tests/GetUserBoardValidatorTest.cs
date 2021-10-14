namespace ImageSourceStorage.Tests
{
    using System;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.DataAccessLayer.Models;
    using ImageSourcesStorage.Validators;
    using Moq;
    using Xunit;

    /// <summary>
    /// test class for get user board.
    /// </summary>
    public class GetUserBoardValidatorTest
    {
        private readonly GetUserBoardValidator getUserBoardValidator;
        private readonly Mock<IUserRepository<User>> userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserBoardValidatorTest"/> class.
        /// </summary>
        public GetUserBoardValidatorTest()
        {
            this.userRepository = new Mock<IUserRepository<User>>();
            this.getUserBoardValidator = new GetUserBoardValidator(this.userRepository.Object);
        }

        /// <summary>
        /// should return id does not exists .
        /// </summary>
        [Fact]
        public void Validate_should_return_false_if_id_does_not_exists()
        {
            var board = new Board()
            {
                UserId = Guid.NewGuid(),
            };

            this.userRepository.Setup(x => x.ExistsAsync(board.UserId)).ReturnsAsync(false);

            var result = this.getUserBoardValidator.Validate(board);

            Assert.False(result.IsValid);
        }

        /// <summary>
        /// should return id exists .
        /// </summary>
        [Fact]
        public void Validate_should_return_true_if_id_exists()
        {
            var userId = Guid.NewGuid();

            var board = new Board()
            {
                UserId = userId,
            };

            this.userRepository.Setup(x => x.ExistsAsync(board.UserId)).ReturnsAsync(true);

            var result = this.getUserBoardValidator.Validate(board);

            Assert.True(result.IsValid);
        }
    }
}
