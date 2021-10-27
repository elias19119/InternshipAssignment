namespace ImageSourceStorage.Tests
{
    using System;
    using System.Threading.Tasks;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.DataAccessLayer.Models;
    using ImageSourcesStorage.Validators;
    using Moq;
    using Xunit;

    /// <summary>
    /// This class is to validate Get board by Id validator.
    /// </summary>
    public class GetBoardByIdValidatorTest
    {
        private readonly Mock<IBoardRepository> boardRepository;
        private readonly GetBoardByIdValidator getBoardByIdValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetBoardByIdValidatorTest"/> class.
        /// Get Board by Id validator.
        /// </summary>
        public GetBoardByIdValidatorTest()
        {
            this.boardRepository = new Mock<IBoardRepository>();
            this.getBoardByIdValidator = new GetBoardByIdValidator(this.boardRepository.Object);
        }

        /// <summary>
        /// should return id exist.
        /// </summary>
        [Fact]
        public void IsBoardExistsAsync_should_return_true_if_id_exists()
        {
            var boardId = Guid.NewGuid();

            this.boardRepository.Setup(x => x.IsBoardExistsAsync(boardId)).ReturnsAsync(true);

            var result = this.getBoardByIdValidator.Validate(boardId);

            Assert.True(result.IsValid);
        }

        /// <summary>
        /// should return name id not exist.
        /// </summary>
        [Fact]
        public void IsBoardExistsAsync_should_return_true_if_id_does_not_exist()
        {
            var boardId = Guid.NewGuid();

            this.boardRepository.Setup(x => x.IsBoardExistsAsync(boardId)).ReturnsAsync(false);

            var result = this.getBoardByIdValidator.Validate(boardId);

            Assert.False(result.IsValid);
        }
    }
}
