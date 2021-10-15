namespace ImageSourceStorage.Tests
{
    using System.Threading.Tasks;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.DataAccessLayer.Models;
    using ImageSourcesStorage.Validators;
    using Moq;
    using Xunit;

    /// <summary>
    ///  this class is for testing the Put method.
    /// </summary>
    public class PutUserValidatorTest
    {
        private readonly Mock<IUserRepository<User>> userRepository;
        private readonly PutUserValidator putUserValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="PutUserValidatorTest"/> class.
        /// </summary>
        public PutUserValidatorTest()
        {
            this.userRepository = new Mock<IUserRepository<User>>();
            this.putUserValidator = new PutUserValidator(this.userRepository.Object);
        }

        /// <summary>
        /// Should return score is not empty.
        /// </summary>
        [Fact]
        public void Validate_should_return_true_if_score_is_not_empty()
        {
            var user = new User
            {
                Name = "reneh",
                Score = 20,
            };

            this.userRepository.Setup(x => x.InsertAsync(user)).Returns(Task.CompletedTask);
            this.userRepository.Setup(x => x.UpdateAsync(user.UserId, user.Name, user.Score)).Returns(Task.CompletedTask);
            this.userRepository.Setup(x => x.ExistsAsync(user.UserId)).ReturnsAsync(true);

            var result = this.putUserValidator.Validate(user);

            Assert.True(result.IsValid);
        }

        /// <summary>
        /// Should return score is empty.
        /// </summary>
        [Fact]
        public void Validate_should_return_false_if_score_is_empty()
        {
            var user = new User
            {
                Name = "reneh",
            };

            this.userRepository.Setup(x => x.UpdateAsync(user.UserId, user.Name, user.Score)).Returns(Task.CompletedTask);
            this.userRepository.Setup(x => x.ExistsAsync(user.UserId)).ReturnsAsync(true);

            var result = this.putUserValidator.Validate(user);

            Assert.False(result.IsValid);
        }

        /// <summary>
        /// Should return score is negative.
        /// </summary>
        [Fact]
        public void Validate_should_return_false_if_score_is_negative()
        {
            var user = new User
            {
                Name = "reneh",
                Score = -2,
            };

            this.userRepository.Setup(x => x.UpdateAsync(user.UserId, user.Name, user.Score)).Returns(Task.CompletedTask);
            this.userRepository.Setup(x => x.ExistsAsync(user.UserId)).ReturnsAsync(true);

            var result = this.putUserValidator.Validate(user);

            Assert.False(result.IsValid);
        }

        /// <summary>
        /// Should return score is not negative.
        /// </summary>
        [Fact]
        public void Validate_should_return_true_if_score_is_not_negative()
        {
            var user = new User
            {
                Name = "reneh",
                Score = 30,
            };

            this.userRepository.Setup(x => x.UpdateAsync(user.UserId, user.Name, user.Score)).Returns(Task.CompletedTask);
            this.userRepository.Setup(x => x.ExistsAsync(user.UserId)).ReturnsAsync(true);

            var result = this.putUserValidator.Validate(user);

            Assert.True(result.IsValid);
        }
    }
}
