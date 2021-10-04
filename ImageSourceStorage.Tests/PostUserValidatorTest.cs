namespace ImageSourceStorage.Tests
{
    using System.Threading.Tasks;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.DataAccessLayer.Models;
    using ImageSourcesStorage.Validators;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    /// <summary>
    /// This class is for testing PostUser.
    /// </summary>
    public class PostUserValidatorTest
    {
        private readonly PostUserValidator postUserValidator;
        private readonly UserRepository<User> userRepository;
        private readonly DataContext dataContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostUserValidatorTest"/> class.
        /// </summary>
        public PostUserValidatorTest()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "FakeConnectionString")
                .Options;
            this.dataContext = new DataContext(options);
            this.userRepository = new UserRepository<User>(this.dataContext);
            this.postUserValidator = new PostUserValidator(this.userRepository);
        }

        /// <summary>
        /// Should return empty.
        /// </summary>
        [Fact]
        public void Validate_should_return_false_if_Name_is_Empty()
        {
            User user = new User
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
            User user = new User
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
            string name = new string('*', 52);
            User user = new User
            {
                Name = name,
            };

            var result = this.postUserValidator.Validate(user);

            Assert.False(result.IsValid);
        }

        /// <summary>
        /// Should return empty.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task Validate_should_return_false_if_Name_is_not_unique()
        {
            User user = new User()
            {
                Name = "Elias",
            };

            await this.dataContext.AddAsync(user);
            await this.dataContext.SaveChangesAsync();
            await this.userRepository.NameExistsAsync(user.Name);

            var result = this.postUserValidator.Validate(user);

            Assert.False(result.IsValid);
        }

        /// <summary>
        /// Should return name is not unique.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task Validate_should_return_true_if_Name_is_unique()
        {
            User user = new User
            {
                Name = "testname",
            };

            await this.userRepository.NameExistsAsync(user.Name);

            var result = this.postUserValidator.Validate(user);

            Assert.True(result.IsValid);
        }
    }
}
