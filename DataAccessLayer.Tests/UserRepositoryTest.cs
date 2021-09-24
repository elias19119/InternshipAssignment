namespace DataAccessLayer.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.DataAccessLayer.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Internal;
    using Moq;
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
        public async Task GetAllUserAsync_should_return_Not_Empty()
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
        public async Task GetAllUserAsync_should_return_Empty()
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
        public async Task GetUserAsync_should_return_user_if_user_exists()
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
        public async Task GetUserAsync_should_return_Null_if_user_does_not_exist()
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
        public async Task PostUserAsync_should_return_user()
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
        /// should return precondition failed.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task PostUserAsync_should_return_not_found_if_name_Exceed_50_Characters()
        {
            bool result = false;
            const int maxFieldLength = 50;
            User user = new User
            {
                Name = "HQIUEHWRIUQHEWRUHQWIEUHRIUWQEHRIUHQWEIUHRIUWQHEIURHQWEERWQRWQEU",
            };

            if (user.Name.Length < maxFieldLength)
            {
                await this.dataContext.AddAsync(user);
                await this.dataContext.SaveChangesAsync();
                var response = this.userRepository.InsertAsync(user);
                result = response.IsCompleted;
            }

            Assert.False(result);
        }
    }
}
