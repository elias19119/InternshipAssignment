namespace ImageSourceStorage.Tests
{
    using System;
    using System.Threading.Tasks;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.DataAccessLayer.Models;
    using ImageSourcesStorage.Validators;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    /// <summary>
    /// this class is to test the validator.
    /// </summary>
    public class AddBoardtoUserValidatorTest
    {
        private readonly AddBoardtoUserValidator addBoardtoUserValidator;
        private readonly IBoardRepository boardRepository;
        private readonly UserRepository<User> userRepository;
        private readonly DataContext dataContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddBoardtoUserValidatorTest"/> class.
        /// </summary>
        public AddBoardtoUserValidatorTest()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
              .UseInMemoryDatabase(databaseName: "FakeConnectionString")
              .Options;
            this.dataContext = new DataContext(options);
            this.boardRepository = new BoardRepository(this.dataContext);
            this.userRepository = new UserRepository<User>(this.dataContext);
            this.addBoardtoUserValidator = new AddBoardtoUserValidator(this.boardRepository);
        }

        /// <summary>
        /// Should return empty.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task Validate_should_return_false_if_Name_is_not_unique()
        {
            Board board = new Board
            {
                Name = "cars",
            };

            await this.dataContext.AddAsync(board);
            await this.dataContext.SaveChangesAsync();
            await this.boardRepository.NameExistsAsync(board.Name);

            var result = this.addBoardtoUserValidator.Validate(board);

            Assert.False(result.IsValid);
        }

        /// <summary>
        /// Should return name is not unique.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task Validate_should_return_true_if_Name_is_unique()
        {
            Board board = new Board
            {
                Name = "Iphones",
            };

            await this.boardRepository.NameExistsAsync(board.Name);

            var result = this.addBoardtoUserValidator.Validate(board);

            Assert.True(result.IsValid);
        }
    }
}
