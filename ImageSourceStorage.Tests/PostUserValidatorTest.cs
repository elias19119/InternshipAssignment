namespace ImageSourceStorage.Tests
{
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.DataAccessLayer.Models;
    using ImageSourcesStorage.Validators;
    using Moq;
    using Xunit;

    /// <summary>
    /// This class is for testing PostUser.
    /// </summary>
    public class PostUserValidatorTest
    {
        private readonly PostUserValidator postUserValidator;
        private readonly Mock<IUserRepository> userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostUserValidatorTest"/> class.
        /// </summary>
        public PostUserValidatorTest()
        {
            this.userRepository = new Mock<IUserRepository>();
            this.postUserValidator = new PostUserValidator(this.userRepository.Object);
        }

        /// <summary>
        /// Should return empty.
        /// </summary>
        [Fact]
        public void Validate_should_return_false_if_Name_is_Empty()
        {
            var user = new User
            {
                Name = string.Empty,
            };

            var result = this.postUserValidator.Validate(user);

            Assert.False(result.IsValid);
        }

        /// <summary>
        /// Should return empty.
        /// </summary>
        [Fact]
        public void Validate_should_return_true_if_Name_is_valid()
        {
            var user = new User
            {
                Name = "Carla",
            };

            var result = this.postUserValidator.Validate(user);

            Assert.True(result.IsValid);
        }

        /// <summary>
        /// Should return error.
        /// </summary>
        [Fact]
        public void Validate_should_return_false_if_name_Exceed_50_Characters()
        {
            var name = new string('*', 52);

            var user = new User
            {
                Name = name,
            };

            var result = this.postUserValidator.Validate(user);

            Assert.False(result.IsValid);
        }

        /// <summary>
        /// Should return empty.
        /// </summary>
        [Fact]
        public void Validate_should_return_false_if_Name_is_not_unique()
        {
            var user = new User()
            {
                Name = "Elias",
            };

            this.userRepository.Setup(x => x.NameExistsAsync(user.Name)).ReturnsAsync(true);

            var result = this.postUserValidator.Validate(user);

            Assert.False(result.IsValid);
        }

        /// <summary>
        /// Should return name is not unique.
        /// </summary>
        [Fact]
        public void Validate_should_return_true_if_Name_is_unique()
        {
            var user = new User
            {
                Name = "testname",
            };

            this.userRepository.Setup(x => x.NameExistsAsync(user.Name)).ReturnsAsync(false);

            var result = this.postUserValidator.Validate(user);

            Assert.True(result.IsValid);
        }
    }
}
