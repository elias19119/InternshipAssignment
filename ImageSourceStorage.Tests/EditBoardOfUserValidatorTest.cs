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
    /// this class is to test the edit board validator.
    /// </summary>
    public class EditBoardOfUserValidatorTest
    {
        private readonly Mock<IUserRepository<User>> userRepository;
        private readonly Mock<IBoardRepository> boardRepository;
        private readonly EditBoardofUserValidator editBoardOfUserValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditBoardOfUserValidatorTest"/> class.
        /// </summary>
        public EditBoardOfUserValidatorTest()
        {
            this.boardRepository = new Mock<IBoardRepository>();
            this.userRepository = new Mock<IUserRepository<User>>();
            this.editBoardOfUserValidator = new EditBoardofUserValidator(this.userRepository.Object, this.boardRepository.Object);
        }

        /// <summary>
        /// should return true.
        /// </summary>
        [Fact]
        public void Validate_should_return_true_if_id_exists_and_if_board_belongs_to_user()
        {
            var board = new Board()
            {
                Name = "cars",
                UserId = Guid.NewGuid(),
                BoardId = Guid.NewGuid(),
            };

            this.boardRepository.Setup(x => x.EditBoardOfUserAsync(board.UserId, board.BoardId, board.Name)).Returns(Task.CompletedTask);
            this.boardRepository.Setup(x => x.IsBoardExistsAsync(board.BoardId)).ReturnsAsync(true);
            this.boardRepository.Setup(x => x.IsNameExistsAsync(board.Name)).ReturnsAsync(false);
            this.boardRepository.Setup(x => x.IsBoardBelongToUserAsync(board.BoardId, board.UserId)).ReturnsAsync(true);
            this.userRepository.Setup(x => x.ExistsAsync(board.UserId)).ReturnsAsync(true);

            var result = this.editBoardOfUserValidator.Validate(board);

            Assert.True(result.IsValid);
        }

        /// <summary>
        /// should not return false.
        /// </summary>
        [Fact]
        public void Validate_should_return_false_if_id_does_not_exists_or_if_board_does_not_belongs_to_user()
        {
            var board = new Board()
            {
                Name = "cars",
                UserId = Guid.NewGuid(),
                BoardId = Guid.NewGuid(),
            };

            this.boardRepository.Setup(x => x.EditBoardOfUserAsync(board.UserId, board.BoardId, board.Name)).Returns(Task.CompletedTask);
            this.boardRepository.Setup(x => x.IsBoardExistsAsync(board.BoardId)).ReturnsAsync(true);
            this.boardRepository.Setup(x => x.IsNameExistsAsync(board.Name)).ReturnsAsync(false);
            this.boardRepository.Setup(x => x.IsBoardBelongToUserAsync(board.BoardId, board.UserId)).ReturnsAsync(false);
            this.userRepository.Setup(x => x.ExistsAsync(board.UserId)).ReturnsAsync(true);

            var result = this.editBoardOfUserValidator.Validate(board);

            Assert.False(result.IsValid);
        }

        /// <summary>
        /// Should name is not unique.
        /// </summary>
        [Fact]
        public void Validate_should_return_false_if_Name_is_not_unique()
        {
            var board = new Board()
            {
                Name = "cars",
                UserId = Guid.NewGuid(),
                BoardId = Guid.NewGuid(),
            };
            this.boardRepository.Setup(x => x.EditBoardOfUserAsync(board.UserId, board.BoardId, board.Name)).Returns(Task.CompletedTask);
            this.boardRepository.Setup(x => x.IsBoardExistsAsync(board.BoardId)).ReturnsAsync(true);
            this.boardRepository.Setup(x => x.IsNameExistsAsync(board.Name)).ReturnsAsync(true);
            this.boardRepository.Setup(x => x.IsBoardBelongToUserAsync(board.BoardId, board.UserId)).ReturnsAsync(true);
            this.userRepository.Setup(x => x.ExistsAsync(board.UserId)).ReturnsAsync(true);

            var result = this.editBoardOfUserValidator.Validate(board);

            Assert.False(result.IsValid);
        }

        /// <summary>
        /// Should return name is unique.
        /// </summary>
        [Fact]
        public void Validate_should_return_true_if_Name_is_unique()
        {
            var board = new Board()
            {
                Name = "cars",
                UserId = Guid.NewGuid(),
                BoardId = Guid.NewGuid(),
            };
            this.boardRepository.Setup(x => x.EditBoardOfUserAsync(board.UserId, board.BoardId, board.Name)).Returns(Task.CompletedTask);
            this.boardRepository.Setup(x => x.IsBoardExistsAsync(board.BoardId)).ReturnsAsync(true);
            this.boardRepository.Setup(x => x.IsNameExistsAsync(board.Name)).ReturnsAsync(false);
            this.boardRepository.Setup(x => x.IsBoardBelongToUserAsync(board.BoardId, board.UserId)).ReturnsAsync(true);
            this.userRepository.Setup(x => x.ExistsAsync(board.UserId)).ReturnsAsync(true);

            var result = this.editBoardOfUserValidator.Validate(board);

            Assert.True(result.IsValid);
        }

        /// <summary>
        /// Should return false.
        /// </summary>
        [Fact]
        public void Validate_should_return_false_if_board_name_Exceed_50_Characters()
        {
            var name = new string('*', 52);
            var board = new Board()
            {
                Name = name,
                UserId = Guid.NewGuid(),
                BoardId = Guid.NewGuid(),
            };

            this.boardRepository.Setup(x => x.EditBoardOfUserAsync(board.UserId, board.BoardId, board.Name)).Returns(Task.CompletedTask);
            this.boardRepository.Setup(x => x.IsBoardExistsAsync(board.BoardId)).ReturnsAsync(true);
            this.boardRepository.Setup(x => x.IsNameExistsAsync(board.Name)).ReturnsAsync(false);
            this.boardRepository.Setup(x => x.IsBoardBelongToUserAsync(board.BoardId, board.UserId)).ReturnsAsync(true);
            this.userRepository.Setup(x => x.ExistsAsync(board.UserId)).ReturnsAsync(true);

            var result = this.editBoardOfUserValidator.Validate(board);

            Assert.False(result.IsValid);
        }
    }
}
