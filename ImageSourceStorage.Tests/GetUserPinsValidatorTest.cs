namespace ImageSourceStorage.Tests
{
    using System;
    using System.Threading.Tasks;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.DataAccessLayer.Models;
    using ImageSourcesStorage.DataAccessLayer.Validators;
    using ImageSourcesStorage.Validators;
    using Moq;
    using Xunit;

    public class GetUserPinsValidatorTest
    {
        private readonly GetUserPinsValidator getUserPinsValidator;
        private readonly Mock<IUserRepository<User>> userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserPinsValidatorTest"/> class.
        /// </summary>
        public GetUserPinsValidatorTest()
        {
            this.userRepository = new Mock<IUserRepository<User>>();
            this.getUserPinsValidator = new GetUserPinsValidator(this.userRepository.Object);
        }

        /// <summary>
        /// should return id does not exists .
        /// </summary>
        [Fact]
        public void Validate_should_return_false_if_id_does_not_exists()
        {
            Guid userId = Guid.NewGuid();

            this.userRepository.Setup(x => x.ExistsAsync(userId)).ReturnsAsync(false);

            var result = this.getUserPinsValidator.Validate(userId);

            Assert.False(result.IsValid);
        }

        /// <summary>
        /// should return id exists .
        /// </summary>
        [Fact]
        public void Validate_should_return_true_if_id_exists()
        {
            Guid userId = Guid.NewGuid();

            this.userRepository.Setup(x => x.ExistsAsync(userId)).ReturnsAsync(true);

            var result = this.getUserPinsValidator.Validate(userId);

            Assert.True(result.IsValid);
        }
    }
}
