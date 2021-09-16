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
                    UserId = new Guid("46cc6856-dc27-43b0-b508-01bbddf9b1a8"),
                    Name = "elias",
                    Score = 20,
                },
                new User
                {
                    UserId = new Guid("46cc6856-dc27-43b0-b508-01bbddf9b1a7"),
                    Name = "larisa",
                    Score = 20,
                },
            };

            await this.dataContext.AddRangeAsync(users);

            await this.dataContext.SaveChangesAsync();

            this.userRepository = new UserRepository<User>(this.dataContext);

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

            this.userRepository = new UserRepository<User>(this.dataContext);

            var result = await this.userRepository.GetAllAsync();
            Assert.Empty(result);
            Assert.Equal(users.Count, result.Count());
        }
    }
}
