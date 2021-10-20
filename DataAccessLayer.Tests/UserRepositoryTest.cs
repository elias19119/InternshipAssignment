namespace DataAccessLayer.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.DataAccessLayer.Models;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    /// <summary>
    /// Create Test units for Users Controller<see cref="UserRepositoryTest"/> class.
    /// </summary>
    public class UserRepositoryTest
    {
        private DataContext dataContext;
        private UserRepository<User> userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepositoryTest"/> class.
        /// </summary>
        public UserRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "FakeConnectionString")
                .Options;
            this.dataContext = new DataContext(options);
            this.userRepository = new UserRepository<User>(this.dataContext);
        }

        /// <summary>
        /// should return An OK Result.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task GetAllAsync_should_return_list_of_users_if_Users_exists()
        {
            var users = new List<User>
            {
                new User
                {
                    UserId = Guid.NewGuid(),
                    Name = "elias",
                    Score = 20,
                },
                new User
                {
                    UserId = Guid.NewGuid(),
                    Name = "larisa",
                    Score = 20,
                },
            };

            await this.dataContext.AddRangeAsync(users);
            await this.dataContext.SaveChangesAsync();

            var result = await this.userRepository.GetAllAsync();

            Assert.NotEmpty(result);
            Assert.Equal(users.Count, result.Count());
        }

        /// <summary>
        /// should return An OK Result.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task GetAllAsync_should_return_empty_list_if_Users_do_not_exist()
        {
            var users = new List<User>
            {
            };

            await this.dataContext.AddRangeAsync(users);
            await this.dataContext.SaveChangesAsync();

            var result = await this.userRepository.GetAllAsync();

            Assert.Empty(result);
            Assert.Equal(users.Count, result.Count());
        }

        /// <summary>
        /// should return a user.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task GetByIdAsync_should_return_user_if_user_exists()
        {
            User user = new User(Guid.NewGuid(), "elias", 20);

            await this.dataContext.AddAsync(user);
            await this.dataContext.SaveChangesAsync();

            var result = await this.userRepository.GetByIdAsync(user.UserId);

            Assert.NotNull(result);
            Assert.Equal(user.UserId, result.UserId);
        }

        /// <summary>
        /// should not return a user.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task GetByIdAsync_should_return_Null_if_user_does_not_exist()
        {
            User user = new User(Guid.NewGuid(), "elias", 20);

            await this.dataContext.AddAsync(user);
            await this.dataContext.SaveChangesAsync();

            var result = await this.userRepository.GetByIdAsync(Guid.NewGuid());

            Assert.Null(result);
        }

        /// <summary>
        /// should return A created user.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task InsertAsync_should_return_user_if_is_valid()
        {
            User user = new User()
            {
                Name = "Hanna",
            };

            await this.dataContext.AddAsync(user);
            await this.dataContext.SaveChangesAsync();

            var response = this.userRepository.InsertAsync(user);

            Assert.NotNull(response);
        }

        /// <summary>
        /// should return name exist.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task NameExistsAsync_should_return_true_if_name_exists()
        {
            User user = new User()
            {
                Name = "Hanna",
            };

            await this.dataContext.AddAsync(user);
            await this.dataContext.SaveChangesAsync();

            var response = this.userRepository.NameExistsAsync(user.Name);

            Assert.True(response.Result);
        }

        /// <summary>
        /// should return name does not exist.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task NameExistsAsync_should_return_false_if_name_does_not_exists()
        {
            string testname = "rami";
            User user = new User()
            {
                Name = "Hanna",
            };

            await this.dataContext.AddAsync(user);
            await this.dataContext.SaveChangesAsync();

            var response = this.userRepository.NameExistsAsync(testname);

            Assert.False(response.Result);
        }

        /// <summary>
        /// should return name exist.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task ExistsAsync_should_return_true_if_id_exists()
        {
            User user = new User()
            {
               UserId = Guid.NewGuid(),
            };

            await this.dataContext.AddAsync(user);
            await this.dataContext.SaveChangesAsync();

            var response = this.userRepository.ExistsAsync(user.UserId);

            Assert.True(response.Result);
        }

        /// <summary>
        /// should return id does not exist.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task ExistsAsync_should_return_false_if_id_does_not_exists()
        {
            User user = new User()
            {
                UserId = Guid.NewGuid(),
            };

            await this.dataContext.AddAsync(user);
            await this.dataContext.SaveChangesAsync();

            var response = this.userRepository.ExistsAsync(Guid.NewGuid());

            Assert.False(response.Result);
        }

        /// <summary>
        /// should delete a user.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task DeleteAsync_should_return_false_if_user_is_deleted()
        {
            User user = new User()
            {
                UserId = Guid.NewGuid(),
            };

            await this.dataContext.AddAsync(user);
            await this.dataContext.SaveChangesAsync();

            var response = this.userRepository.DeleteAsync(user.UserId);
            var isUserExists = this.dataContext.Users.Any(x => x.UserId == user.UserId);

            Assert.False(isUserExists);
        }

        /// <summary>
        /// should delete a user.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task DeleteAsync_should_return_false_if_id_do_not_exists()
        {
            User user = new User()
            {
                UserId = Guid.NewGuid(),
            };

            await this.dataContext.AddAsync(user);
            await this.dataContext.SaveChangesAsync();

            var response = this.userRepository.DeleteAsync(Guid.NewGuid());

            Assert.False(response.IsCompletedSuccessfully);
        }

        /// <summary>
        /// should return true if user is valid.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task UpdateAsync_should_return_true_if_user_is_valid()
        {
            User user = new User()
            {
                Name = "Reneh",
                Score = 50,
            };

            await this.dataContext.AddAsync(user);
            await this.dataContext.SaveChangesAsync();

            var response = this.userRepository.UpdateAsync(user.UserId, user.Name, user.Score);
            var isUserExists = this.dataContext.Users.Any(x => x.UserId == user.UserId);

            Assert.True(isUserExists);
        }

        /// <summary>
        ///  should return true if id exists.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task ChangeUserScoreAsync_should_change_score_if_user_is_valid()
        {
            User userEntity = new User()
            {
                Name = "Reneh",
                Score = 50,
            };

            await this.dataContext.Users.AddAsync(userEntity);
            await this.dataContext.SaveChangesAsync();

            await this.userRepository.ChangeUserScore(userEntity.UserId, ChangeScoreOptions.Decrease);

            var isUserExists = this.dataContext.Users.Any(x => x.UserId == userEntity.UserId);
            var user = await this.userRepository.GetByIdAsync(userEntity.UserId);

            Assert.True(isUserExists);
            Assert.Equal(userEntity.Score, user.Score);
        }

        /// <summary>
        ///  should return null if id does not exists.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task ChangeUserScoreAsync_should_not_change_score_if_user_is_not_valid()
        {
            User userEntity = new User()
            {
                Name = "Reneh",
                Score = 50,
            };

            await this.dataContext.Users.AddAsync(userEntity);
            await this.dataContext.SaveChangesAsync();

            var response = this.userRepository.ChangeUserScore(userEntity.UserId, ChangeScoreOptions.Decrease);

            var isUserExists = this.dataContext.Users.Any(x => x.UserId == Guid.NewGuid());
            var user = await this.userRepository.GetByIdAsync(Guid.NewGuid());

            Assert.False(isUserExists);
            Assert.Null(user);
        }
    }
}
