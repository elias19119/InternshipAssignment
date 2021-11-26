namespace ImageSourceStorage.Tests
{
    using System;
    using System.Threading.Tasks;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.DataAccessLayer.Models;
    using ImageSourcesStorage.Validators;
    using Moq;
    using Xunit;

    /// <summary>
    /// this class is to test change user score request.
    /// </summary>
    public class ChangeUserScoreValidatorTest
    {
        private readonly ChangeUserScoreValidator changeUserScoreValidator;
        private readonly Mock<IUserRepository> userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeUserScoreValidatorTest"/> class.
        /// </summary>
        public ChangeUserScoreValidatorTest()
        {
            this.userRepository = new Mock<IUserRepository>();
            this.changeUserScoreValidator = new ChangeUserScoreValidator(this.userRepository.Object);
        }

        /// <summary>
        /// should return id does not exists .
        /// </summary>
        [Fact]
        public void Validate_should_return_false_if_id_does_not_exists()
        {
            var user = new User()
            {
                UserId = Guid.NewGuid(),
            };

            this.userRepository.Setup(x => x.NameExistsAsync(user.Name)).ReturnsAsync(false);
            this.userRepository.Setup(x => x.ExistsAsync(user.UserId)).ReturnsAsync(false);
            this.userRepository.Setup(x => x.ChangeUserScore(user.UserId, ChangeScoreOptions.Increase)).Returns(Task.CompletedTask);

            var result = this.changeUserScoreValidator.Validate(user);

            Assert.False(result.IsValid);
        }

        /// <summary>
        /// should return id exists .
        /// </summary>
        [Fact]
        public void Validate_should_return_true_if_id_exists()
        {
            var user = new User()
            {
                UserId = Guid.NewGuid(),
            };

            this.userRepository.Setup(x => x.NameExistsAsync(user.Name)).ReturnsAsync(false);
            this.userRepository.Setup(x => x.ExistsAsync(user.UserId)).ReturnsAsync(true);
            this.userRepository.Setup(x => x.ChangeUserScore(user.UserId, ChangeScoreOptions.Increase)).Returns(Task.CompletedTask);

            var result = this.changeUserScoreValidator.Validate(user);

            Assert.True(result.IsValid);
        }
    }
}
