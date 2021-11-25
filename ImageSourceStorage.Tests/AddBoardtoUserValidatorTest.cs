namespace ImageSourceStorage.Tests
{
    using System;
    using System.Threading.Tasks;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.DataAccessLayer.Models;
    using ImageSourcesStorage.Validators;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    /// <summary>
    /// this class is to test the validator.
    /// </summary>
    public class AddBoardtoUserValidatorTest
    {
        private readonly AddBoardtoUserValidator addBoardtoUserValidator;
        private readonly Mock<IBoardRepository<Board>> boardRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddBoardtoUserValidatorTest"/> class.
        /// </summary>
        public AddBoardtoUserValidatorTest()
        {
            this.boardRepository = new Mock<IBoardRepository<Board>>();
            this.addBoardtoUserValidator = new AddBoardtoUserValidator(this.boardRepository.Object);
        }

        /// <summary>
        /// Should return empty.
        /// </summary>
        [Fact]
        public void Validate_should_return_false_if_Name_is_not_unique()
        {
            var board = new Board
            {
                Name = "cars",
            };

            this.boardRepository.Setup(x => x.IsNameExistsAsync(board.Name)).ReturnsAsync(true);

            var result = this.addBoardtoUserValidator.Validate(board);

            Assert.False(result.IsValid);
        }

        /// <summary>
        /// Should return name is not unique.
        /// </summary>
        [Fact]
        public void Validate_should_return_true_if_Name_is_unique()
        {
            var board = new Board
            {
                Name = "Iphones",
            };

            this.boardRepository.Setup(x => x.IsNameExistsAsync(board.Name)).ReturnsAsync(false);

            var result = this.addBoardtoUserValidator.Validate(board);

            Assert.True(result.IsValid);
        }
    }
}
