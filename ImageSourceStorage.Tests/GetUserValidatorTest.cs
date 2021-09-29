namespace ImageSourceStorage.Tests
{
    using System;
    using System.Threading.Tasks;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.DataAccessLayer.Models;
    using ImageSourcesStorage.DataAccessLayer.Validators;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    /// <summary>
    /// this class test the User validator.
    /// </summary>
    public class GetUserValidatorTest
    {
        private readonly GetUserValidator getUserValidator;
        private readonly UserRepository<User> userRepository;
        private readonly DataContext dataContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserValidatorTest"/> class.
        /// </summary>
        public GetUserValidatorTest()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "FakeConnectionString")
                .Options;
            this.dataContext = new DataContext(options);
            this.userRepository = new UserRepository<User>(this.dataContext);
            this.getUserValidator = new GetUserValidator(this.userRepository);
        }

        /// <summary>
        /// should return id does not exists .
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task Validate_should_return_true_if_id_does_not_exists()
        {
            User user = new User()
            {
                UserId = Guid.NewGuid(),
            };

            await this.userRepository.GetByIdAsync(Guid.NewGuid());

            var result = this.getUserValidator.Validate(user);

            Assert.True(!result.IsValid);
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
            await this.dataContext.AddAsync(user);
            await this.dataContext.SaveChangesAsync();

            await this.userRepository.GetByIdAsync(user.UserId);

            var result = this.getUserValidator.Validate(user);

            Assert.True(result.IsValid);
        }
    }
}
