namespace ImageSourceStorage.Tests
{
    using System;
    using System.Threading.Tasks;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.DataAccessLayer.Models;
    using ImageSourcesStorage.DataAccessLayer.Validators;
    using Moq;
    using Xunit;

    /// <summary>
    /// this class test the User validator.
    /// </summary>
    public class CheckUserIdValidatorTest
    {
        private readonly CheckUserIdValidator checkUserIdValidator;
        private readonly Mock<IUserRepository<User>> userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckUserIdValidatorTest"/> class.
        /// </summary>
        public CheckUserIdValidatorTest()
        {
            this.userRepository = new Mock<IUserRepository<User>>();
            this.checkUserIdValidator = new CheckUserIdValidator(this.userRepository.Object);
        }

        /// <summary>
        /// should return id does not exists .
        /// </summary>
        [Fact]
        public void Validate_should_return_false_if_id_does_not_exists()
        {
            var userId = Guid.NewGuid();

            this.userRepository.Setup(x => x.ExistsAsync(userId)).ReturnsAsync(false);

            var result = this.checkUserIdValidator.Validate(userId);

            Assert.False(result.IsValid);
        }

        /// <summary>
        /// should return id exists .
        /// </summary>
        [Fact]
        public void Validate_should_return_true_if_id_exists()
        {
            var userId = Guid.NewGuid();

            this.userRepository.Setup(x => x.ExistsAsync(userId)).ReturnsAsync(true);

            var result = this.checkUserIdValidator.Validate(userId);

            Assert.True(result.IsValid);
        }
    }
}
