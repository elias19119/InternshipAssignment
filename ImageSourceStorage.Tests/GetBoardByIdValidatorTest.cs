﻿namespace ImageSourceStorage.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Text;
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
            User user = new User()
            {
                UserId = Guid.NewGuid(),
            };
            Board board = new Board()
            {
                Name = "cars",
                BoardId = Guid.NewGuid(),
                UserId = user.UserId,
            };

            this.boardRepository.Setup(x => x.AddBoardToUserAsync(board.UserId, board.BoardId, board.Name));
            this.boardRepository.Setup(x => x.IsBoardExistsAsync(board.BoardId)).ReturnsAsync(true);

            var result = this.getBoardByIdValidator.Validate(board);

            Assert.True(result.IsValid);
        }

        /// <summary>
        /// should return name id not exist.
        /// </summary>
        [Fact]
        public void IsBoardExistsAsync_should_return_true_if_id_does_not_exist()
        {
            User user = new User()
            {
                UserId = Guid.NewGuid(),
            };
            Board board = new Board()
            {
                Name = "cars",
                BoardId = Guid.NewGuid(),
                UserId = user.UserId,
            };

            this.boardRepository.Setup(x => x.AddBoardToUserAsync(board.UserId, Guid.NewGuid(), board.Name));
            this.boardRepository.Setup(x => x.IsBoardExistsAsync(board.BoardId));

            var result = this.getBoardByIdValidator.Validate(board);

            Assert.False(result.IsValid);
        }
    }
}
