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
            var user = new User()
            {
                UserId = Guid.NewGuid(),
            };
            var boardEntity = new Board()
            {
                BoardId = Guid.NewGuid(),
                UserId = user.UserId,
                Name = "cars",
            };

            await this.dataContext.AddAsync(user);
            await this.dataContext.SaveChangesAsync();
            await this.boardRepository.AddBoardToUserAsync(boardEntity.UserId, boardEntity.BoardId, boardEntity.Name);

            var board = await this.boardRepository.GetBoardByIdAsync(boardEntity.BoardId);

            Assert.Equal(board.BoardId, boardEntity.BoardId);
            Assert.Equal(board.UserId, boardEntity.UserId);
            Assert.Equal(board.Name, boardEntity.Name);
        }

        /// <summary>
        /// should return ok.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task AddBoardToUserAsync_should_not_add_board_to_context_if_data_is_not_valid()
        {
            var userId = Guid.NewGuid();
            var name = "cars";
            var boardId = Guid.NewGuid();

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
            var board = new Board()
            {
                Name = "cars",
                BoardId = Guid.NewGuid(),
            };

            await this.dataContext.AddAsync(board);
            await this.dataContext.SaveChangesAsync();

            var response = await this.boardRepository.IsNameExistsAsync(board.Name);

            Assert.True(response);
        }

        /// <summary>
        /// should return name does not exist.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task NameExistsAsync_should_return_false_if_name_does_not_exist()
        {
            var testName = "boats";
            var board = new Board()
            {
                Name = "cars",
                BoardId = Guid.NewGuid(),
            };

            await this.dataContext.AddAsync(board);
            await this.dataContext.SaveChangesAsync();

            var response = await this.boardRepository.IsNameExistsAsync(testName);

            Assert.False(response);
        }

        /// <summary>
        /// should return id exist.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task IsBoardExistsAsync_should_return_true_if_id_exists()
        {
            var board = new Board()
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
            var testId = Guid.NewGuid();
            var board = new Board()
            {
                Name = "cars",
                BoardId = Guid.NewGuid(),
            };

            await this.dataContext.AddAsync(board);
            await this.dataContext.SaveChangesAsync();

            var response = await this.boardRepository.IsBoardExistsAsync(testId);

            Assert.False(response);
        }

        /// <summary>
        /// should delete a user.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task DeleteAsync_should_return_false_if_board_is_deleted()
        {
            var board = new Board()
            {
                UserId = Guid.NewGuid(),
                BoardId = Guid.NewGuid(),
                Name = "Iphones",
            };

            await this.dataContext.AddAsync(board);
            await this.dataContext.SaveChangesAsync();

            await this.boardRepository.DeleteBoardOfUserAsync(board.UserId, board.BoardId);
            var isUserExists = this.dataContext.Boards.Any(x => x.BoardId == board.BoardId);

            Assert.False(isUserExists);
        }

        /// <summary>
        /// should delete a user.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task DeleteAsync_should_return_false_if_id_do_not_exists()
        {
            var board = new Board()
            {
                UserId = Guid.NewGuid(),
                BoardId = Guid.NewGuid(),
                Name = "Iphones",
            };

            await this.dataContext.AddAsync(board);
            await this.dataContext.SaveChangesAsync();

            var response = this.boardRepository.DeleteBoardOfUserAsync(Guid.NewGuid(), Guid.NewGuid());

            Assert.False(response.IsCompletedSuccessfully);
        }
    }
}
