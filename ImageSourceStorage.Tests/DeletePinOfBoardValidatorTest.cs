﻿namespace ImageSourceStorage.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.Validators;
    using Moq;
    using Xunit;

    public class DeletePinOfBoardValidatorTest
    {
        private readonly Mock<IBoardRepository> boardRepository;
        private readonly Mock<IPinRepository> pinRepository;
        private readonly DeletePinOfBoardValidator deletePinOfBoardValidator;

        public DeletePinOfBoardValidatorTest()
        {
            this.boardRepository = new Mock<IBoardRepository>();
            this.pinRepository = new Mock<IPinRepository>();
            this.deletePinOfBoardValidator = new DeletePinOfBoardValidator(this.pinRepository.Object, this.boardRepository.Object);
        }

        /// <summary>
        /// should return id does not exists .
        /// </summary>
        [Fact]
        public void Validate_should_return_false_if_pinId_and_boardId_does_not_exists_and_if_pin_does_not_belongs_to_the_board()
        {
            var pin = new Pin()
            {
                PinId = Guid.NewGuid(),
                BoardId = Guid.NewGuid(),
            };

            this.pinRepository.Setup(x => x.IsPinBelongToBoardAsync(pin.BoardId, pin.PinId)).ReturnsAsync(false);
            this.boardRepository.Setup(x => x.IsBoardExistsAsync(pin.BoardId)).ReturnsAsync(false);
            this.pinRepository.Setup(x => x.IsPinExistsAsync(pin.PinId)).ReturnsAsync(false);

            var result = this.deletePinOfBoardValidator.Validate(pin);

            Assert.False(result.IsValid);
        }

        /// <summary>
        /// should return id does not exists .
        /// </summary>
        [Fact]
        public void Validate_should_return_false_if_pin_does_not_belongs_to_the_board()
        {
            var pin = new Pin()
            {
                PinId = Guid.NewGuid(),
                BoardId = Guid.NewGuid(),
            };

            this.pinRepository.Setup(x => x.IsPinBelongToBoardAsync(pin.BoardId, pin.PinId)).ReturnsAsync(false);
            this.boardRepository.Setup(x => x.IsBoardExistsAsync(pin.BoardId)).ReturnsAsync(true);
            this.pinRepository.Setup(x => x.IsPinExistsAsync(pin.PinId)).ReturnsAsync(true);

            var result = this.deletePinOfBoardValidator.Validate(pin);

            Assert.False(result.IsValid);
        }

        /// <summary>
        /// should return id does not exists .
        /// </summary>
        [Fact]
        public void Validate_should_return_false_if_pinId_does_not_exists()
        {
            var pin = new Pin()
            {
                PinId = Guid.NewGuid(),
                BoardId = Guid.NewGuid(),
            };

            this.pinRepository.Setup(x => x.IsPinBelongToBoardAsync(pin.BoardId, pin.PinId)).ReturnsAsync(true);
            this.boardRepository.Setup(x => x.IsBoardExistsAsync(pin.BoardId)).ReturnsAsync(true);
            this.pinRepository.Setup(x => x.IsPinExistsAsync(pin.PinId)).ReturnsAsync(false);

            var result = this.deletePinOfBoardValidator.Validate(pin);

            Assert.False(result.IsValid);
        }

        /// <summary>
        /// should return id does not exists .
        /// </summary>
        [Fact]
        public void Validate_should_return_false_if_boardId_does_not_exists()
        {
            var pin = new Pin()
            {
                PinId = Guid.NewGuid(),
                BoardId = Guid.NewGuid(),
            };

            this.pinRepository.Setup(x => x.IsPinBelongToBoardAsync(pin.BoardId, pin.PinId)).ReturnsAsync(true);
            this.boardRepository.Setup(x => x.IsBoardExistsAsync(pin.BoardId)).ReturnsAsync(false);
            this.pinRepository.Setup(x => x.IsPinExistsAsync(pin.PinId)).ReturnsAsync(true);

            var result = this.deletePinOfBoardValidator.Validate(pin);

            Assert.False(result.IsValid);
        }

        /// <summary>
        /// should return id exists .
        /// </summary>
        [Fact]
        public void Validate_should_return_true_if_id_exists()
        {
            var pin = new Pin()
            {
                PinId = Guid.NewGuid(),
                BoardId = Guid.NewGuid(),
            };

            this.pinRepository.Setup(x => x.IsPinBelongToBoardAsync(pin.BoardId, pin.PinId)).ReturnsAsync(true);
            this.boardRepository.Setup(x => x.IsBoardExistsAsync(pin.BoardId)).ReturnsAsync(true);
            this.pinRepository.Setup(x => x.IsPinExistsAsync(pin.PinId)).ReturnsAsync(true);

            var result = this.deletePinOfBoardValidator.Validate(pin);

            Assert.True(result.IsValid);
        }
    }
}
