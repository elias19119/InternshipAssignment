namespace ImageSourceStorage.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.DataAccessLayer.Models;
    using ImageSourcesStorage.Validators;
    using Moq;
    using Xunit;

    public class DeletePinOfBoardValidatorTest
    {
        private readonly Mock<IBoardRepository<Board>> boardRepository;
        private readonly Mock<IPinRepository<Pin>> pinRepository;
        private readonly Mock<IPinBoardRepository<PinBoard>> pinBoardRepository;
        private readonly DeletePinOfBoardValidator deletePinOfBoardValidator;

        public DeletePinOfBoardValidatorTest()
        {
            this.boardRepository = new Mock<IBoardRepository<Board>>();
            this.pinRepository = new Mock<IPinRepository<Pin>>();
            this.pinBoardRepository = new Mock<IPinBoardRepository<PinBoard>>();
            this.deletePinOfBoardValidator = new DeletePinOfBoardValidator(this.pinRepository.Object, this.boardRepository.Object, this.pinBoardRepository.Object);
        }

        /// <summary>
        /// should return id does not exists .
        /// </summary>
        [Fact]
        public void Validate_should_return_false_if_pinId_and_boardId_does_not_exists_and_if_pin_does_not_belongs_to_the_board()
        {
            var pinBoard = new PinBoard()
            {
                PinId = Guid.NewGuid(),
                BoardId = Guid.NewGuid(),
            };

            this.pinBoardRepository.Setup(x => x.IsPinBelongToBoardAsync(pinBoard.BoardId, pinBoard.PinId)).ReturnsAsync(false);
            this.boardRepository.Setup(x => x.IsBoardExistsAsync(pinBoard.BoardId)).ReturnsAsync(false);
            this.pinRepository.Setup(x => x.IsPinExistsAsync(pinBoard.PinId)).ReturnsAsync(false);

            var result = this.deletePinOfBoardValidator.Validate(pinBoard);

            Assert.False(result.IsValid);
        }

        /// <summary>
        /// should return id does not exists .
        /// </summary>
        [Fact]
        public void Validate_should_return_false_if_pin_does_not_belongs_to_the_board()
        {
            var pinBoard = new PinBoard()
            {
                PinId = Guid.NewGuid(),
                BoardId = Guid.NewGuid(),
            };

            this.pinBoardRepository.Setup(x => x.IsPinBelongToBoardAsync(pinBoard.BoardId, pinBoard.PinId)).ReturnsAsync(false);
            this.boardRepository.Setup(x => x.IsBoardExistsAsync(pinBoard.BoardId)).ReturnsAsync(true);
            this.pinRepository.Setup(x => x.IsPinExistsAsync(pinBoard.PinId)).ReturnsAsync(true);

            var result = this.deletePinOfBoardValidator.Validate(pinBoard);

            Assert.False(result.IsValid);
        }

        /// <summary>
        /// should return id does not exists .
        /// </summary>
        [Fact]
        public void Validate_should_return_false_if_pinId_does_not_exists()
        {
            var pinBoard = new PinBoard()
            {
                PinId = Guid.NewGuid(),
                BoardId = Guid.NewGuid(),
            };

            this.pinBoardRepository.Setup(x => x.IsPinBelongToBoardAsync(pinBoard.BoardId, pinBoard.PinId)).ReturnsAsync(true);
            this.boardRepository.Setup(x => x.IsBoardExistsAsync(pinBoard.BoardId)).ReturnsAsync(true);
            this.pinRepository.Setup(x => x.IsPinExistsAsync(pinBoard.PinId)).ReturnsAsync(false);

            var result = this.deletePinOfBoardValidator.Validate(pinBoard);

            Assert.False(result.IsValid);
        }

        /// <summary>
        /// should return id does not exists .
        /// </summary>
        [Fact]
        public void Validate_should_return_false_if_boardId_does_not_exists()
        {
            var pinBoard = new PinBoard()
            {
                PinId = Guid.NewGuid(),
                BoardId = Guid.NewGuid(),
            };

            this.pinBoardRepository.Setup(x => x.IsPinBelongToBoardAsync(pinBoard.BoardId, pinBoard.PinId)).ReturnsAsync(true);
            this.boardRepository.Setup(x => x.IsBoardExistsAsync(pinBoard.BoardId)).ReturnsAsync(false);
            this.pinRepository.Setup(x => x.IsPinExistsAsync(pinBoard.PinId)).ReturnsAsync(true);

            var result = this.deletePinOfBoardValidator.Validate(pinBoard);

            Assert.False(result.IsValid);
        }

        /// <summary>
        /// should return id exists .
        /// </summary>
        [Fact]
        public void Validate_should_return_true_if_id_exists()
        {
            var pinBoard = new PinBoard()
            {
                PinId = Guid.NewGuid(),
                BoardId = Guid.NewGuid(),
            };

            this.pinBoardRepository.Setup(x => x.IsPinBelongToBoardAsync(pinBoard.BoardId, pinBoard.PinId)).ReturnsAsync(true);
            this.boardRepository.Setup(x => x.IsBoardExistsAsync(pinBoard.BoardId)).ReturnsAsync(true);
            this.pinRepository.Setup(x => x.IsPinExistsAsync(pinBoard.PinId)).ReturnsAsync(true);

            var result = this.deletePinOfBoardValidator.Validate(pinBoard);

            Assert.True(result.IsValid);
        }
    }
}
