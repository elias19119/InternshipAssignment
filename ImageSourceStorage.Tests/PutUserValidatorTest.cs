namespace ImageSourceStorage.Tests
{
    using System.Threading.Tasks;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.DataAccessLayer.Models;
    using ImageSourcesStorage.Models;
    using ImageSourcesStorage.Validators;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    /// <summary>
    ///  this class is for testing the Put method.
    /// </summary>
    public class PutUserValidatorTest
    {
        private readonly UserRepository<User> userRepository;
        private readonly DataContext dataContext;
        private readonly PutUserValidator putuserValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="PutUserValidatorTest"/> class.
        /// </summary>
        public PutUserValidatorTest()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
               .UseInMemoryDatabase(databaseName: "FakeConnectionString")
               .Options;
            this.dataContext = new DataContext(options);
            this.userRepository = new UserRepository<User>(this.dataContext);
            this.putuserValidator = new PutUserValidator();
        }

        /// <summary>
        /// Should return score is not empty.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task Validate_should_return_true_if_score_is_not_empty()
        {
            UpdateUserRequest userRequest = new UpdateUserRequest()
            {
                Name = "reneh",
                Score = 20,
            };

            User user = new User
            {
                Name = userRequest.Name,
                Score = userRequest.Score,
            };

            await this.userRepository.UpdateAsync(user);

            var result = this.putuserValidator.Validate(user);

            Assert.True(result.IsValid);
        }

        /// <summary>
        /// Should return score is empty.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task Validate_should_return_false_if_score_is_empty()
        {
            UpdateUserRequest userRequest = new UpdateUserRequest()
            {
                Name = "reneh",
            };

            User user = new User
            {
                Name = userRequest.Name,
                Score = userRequest.Score,
            };

            await this.userRepository.UpdateAsync(user);

            var result = this.putuserValidator.Validate(user);

            Assert.False(result.IsValid);
        }

        /// <summary>
        /// Should return score is negative.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task Validate_should_return_false_if_score_is_negative()
        {
            UpdateUserRequest userRequest = new UpdateUserRequest()
            {
                Name = "reneh",
                Score = -2,
            };

            User user = new User
            {
                Name = userRequest.Name,
                Score = userRequest.Score,
            };

            await this.userRepository.UpdateAsync(user);

            var result = this.putuserValidator.Validate(user);

            Assert.False(result.IsValid);
        }

        /// <summary>
        /// Should return score is not negative.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task Validate_should_return_true_if_score_is_not_negative()
        {
            UpdateUserRequest userRequest = new UpdateUserRequest()
            {
                Name = "reneh",
                Score = 30,
            };

            User user = new User
            {
                Name = userRequest.Name,
                Score = userRequest.Score,
            };

            await this.userRepository.UpdateAsync(user);

            var result = this.putuserValidator.Validate(user);

            Assert.True(result.IsValid);
        }
    }
}
