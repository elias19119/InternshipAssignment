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
        public async Task AddBoardToUserAsync_should_add_board_to_context_if_data_is_valid()
        {
            User user = new User()
            {
                UserId = Guid.NewGuid(),
            };
            var board = new Board()
            {
                BoardId = Guid.NewGuid(),
                UserId = user.UserId,
                Name = "cars",
            };

            await this.dataContext.AddAsync(user);
            await this.dataContext.SaveChangesAsync();
            await this.boardRepository.AddBoardToUserAsync(board.UserId, board.BoardId, board.Name);

            var Board = await this.boardRepository.GetBoardByIdAsync(board.BoardId);

            Assert.Equal(board.BoardId, Board.BoardId);
        }

        /// <summary>
        /// should return ok.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task AddBoardToUserAsync_should_not_add_board_to_context_if_data_is_not_valid()
        {

            Guid userId = Guid.NewGuid();
            string name = "cars";
            Guid boardId = Guid.NewGuid();

            await this.boardRepository.AddBoardToUserAsync(userId, boardId, name);
            var board = await this.boardRepository.GetBoardByIdAsync(Guid.NewGuid());

            Assert.Null(board);
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
                BoardId = Guid.NewGuid(),
            };

            await this.dataContext.AddAsync(board);
            await this.dataContext.SaveChangesAsync();

            var response = await this.boardRepository.NameExistsAsync(board.Name);

            Assert.True(response);
        }

        /// <summary>
        /// should return name does not exist.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task NameExistsAsync_should_return_false_if_name_does_not_exist()
        {
            string testname = "boats";
            Board board = new Board()
            {
                Name = "cars",
                BoardId = Guid.NewGuid(),
            };

            await this.dataContext.AddAsync(board);
            await this.dataContext.SaveChangesAsync();

            var response = await this.boardRepository.NameExistsAsync(testname);

            Assert.False(response);
        }

        /// <summary>
        /// should return id exist.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task IsBoardExistsAsync_should_return_true_if_id_exists()
        {
            Board board = new Board()
            {
                Name = "cars",
                BoardId = Guid.NewGuid(),
            };

            await this.dataContext.AddAsync(board);
            await this.dataContext.SaveChangesAsync();

            var response = await this.boardRepository.IsBoardExistsAsync(board.BoardId);

            Assert.True(response);
        }

        /// <summary>
        /// should return name does not exist.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task IsBoardExistsAsync_should_return_true_if_id_does_not_exist()
        {
            Guid testId = Guid.NewGuid();
            Board board = new Board()
            {
                Name = "cars",
                BoardId = Guid.NewGuid(),
            };

            await this.dataContext.AddAsync(board);
            await this.dataContext.SaveChangesAsync();

            var response = await this.boardRepository.IsBoardExistsAsync(testId);

            Assert.False(response);
        }
    }
}
