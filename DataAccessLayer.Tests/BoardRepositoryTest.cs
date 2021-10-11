namespace DataAccessLayer.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.DataAccessLayer.Models;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    /// <summary>
    /// This class is to test the Board Repository.
    /// </summary>
    public class BoardRepositoryTest
    {
        private readonly DataContext dataContext;
        private readonly BoardRepository boardRepository;
        private readonly Guid userId;

        /// <summary>
        /// Initializes a new instance of the <see cref="BoardRepositoryTest"/> class.
        /// </summary>
        public BoardRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "FakeConnectionString")
                .Options;
            this.dataContext = new DataContext(options);
            this.boardRepository = new BoardRepository(this.dataContext);
            this.userId = Guid.NewGuid();
        }

        /// <summary>
        /// should return An OK Result.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task GetUserBoardAsync_should_return_list_of_boards_of_users_if_User_exists()
        {
            var boards = new List<Board>
            {
                new Board
                {
                    UserId = this.userId,
                    Name = "nature",
                },
                new Board
                {
                    UserId = this.userId,
                    Name = "cars",
                },
            };

            await this.dataContext.AddRangeAsync(boards);
            await this.dataContext.SaveChangesAsync();

            var result = await this.boardRepository.GetUserBoardAsync(this.userId);

            Assert.NotEmpty(result);
            Assert.Equal(boards.Count, result.Count);
        }

        /// <summary>
        /// should return not found.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task GetUserBoardAsync_should_return_empty_list_of_boards_of_user_if_User_does_not_exists()
        {
            var boards = new List<Board>();

            await this.dataContext.AddRangeAsync(boards);
            await this.dataContext.SaveChangesAsync();

            var result = await this.boardRepository.GetUserBoardAsync(this.userId);

            Assert.Empty(result);
        }

        /// <summary>
        /// should return ok.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task PostBoardtoUserAsync_should_return_true_if_User_exists()
        {
            var board = new Board();

            await this.dataContext.AddAsync(board);
            await this.dataContext.SaveChangesAsync();

            var response = this.boardRepository.PostBoardtoUserAsync(this.userId, board);

            Assert.NotNull(response);
        }

        /// <summary>
        /// should return ok.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task PostBoardtoUserAsync_should_return_false_if_User_does_not_exists()
        {
            var board = new Board()
            {
                UserId = Guid.NewGuid(),
            };

            await this.dataContext.AddAsync(board);
            await this.dataContext.SaveChangesAsync();

            var response = this.boardRepository.PostBoardtoUserAsync(Guid.NewGuid(), board);

            Assert.False(response.IsCompletedSuccessfully);
        }

        /// <summary>
        /// should return name exist.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task NameExistsAsync_should_return_true_if_name_exists()
        {
            Board board = new Board()
            {
                Name = "cars",
            };

            await this.dataContext.AddAsync(board);
            await this.dataContext.SaveChangesAsync();

            var response = this.boardRepository.NameExistsAsync(board.Name);

            Assert.True(response.Result);
        }

        /// <summary>
        /// should return name does not exist.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task NameExistsAsync_should_return_false_if_name_does_not_exists()
        {
            string testname = "boats";
            Board board = new Board()
            {
                Name = "cars",
            };

            await this.dataContext.AddAsync(board);
            await this.dataContext.SaveChangesAsync();

            var response = this.boardRepository.NameExistsAsync(testname);

            Assert.False(response.Result);
        }
    }
}
