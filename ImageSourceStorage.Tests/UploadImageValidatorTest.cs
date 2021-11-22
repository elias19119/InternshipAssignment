namespace ImageSourceStorage.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.DataAccessLayer.Models;
    using ImageSourcesStorage.Models;
    using ImageSourcesStorage.Validators;
    using Microsoft.AspNetCore.Http;
    using Moq;
    using Xunit;

    public class UploadImageValidatorTest
    {
        private readonly Mock<IUserRepository<User>> userRepository;
        private readonly Mock<IBoardRepository> boardRepository;
        private readonly Mock<IPinRepository> pinRepository;
        private readonly Mock<IPinBoardRepository<PinBoard>> pinBoardRepository;
        private readonly UploadImageValidator uploadImageValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="UploadImageValidatorTest"/> class.
        /// </summary>
        public UploadImageValidatorTest()
        {
            this.userRepository = new Mock<IUserRepository<User>>();
            this.pinRepository = new Mock<IPinRepository>();
            this.boardRepository = new Mock<IBoardRepository>();
            this.pinBoardRepository = new Mock<IPinBoardRepository<PinBoard>>();
            this.uploadImageValidator = new UploadImageValidator(this.userRepository.Object , this.boardRepository.Object , this.pinRepository.Object, this.pinBoardRepository.Object);
        }

        /// <summary>
        /// should return true.
        /// </summary>
        [Fact]
        public void Validate_Should_return_true_if_data_are_valid_and_if_pin_is_inserted()
        {
            var boardId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var pinId = Guid.NewGuid();

            var addPin = new AddPinToBoard { BoardId = boardId, UserId = userId, PinId = pinId };

            this.userRepository.Setup(x => x.ExistsAsync(userId)).ReturnsAsync(true);
            this.boardRepository.Setup(x => x.IsBoardExistsAsync(boardId)).ReturnsAsync(true);
            this.boardRepository.Setup(x => x.IsBoardBelongToUserAsync(boardId, userId)).ReturnsAsync(true);
            this.pinBoardRepository.Setup(x => x.IsPinBelongToBoardAsync(boardId, pinId)).ReturnsAsync(false);
            this.pinBoardRepository.Setup(x => x.InsertPinBoard(boardId, pinId)).Returns(Task.CompletedTask);

            var result = this.uploadImageValidator.Validate(addPin);

            Assert.True(result.IsValid);
        }

        /// <summary>
        /// should return true.
        /// </summary>
        [Fact]
        public void Validate_Should_upload_a_file_if_data_are_valid_and_if_file_is_Uploaded()
        {
            var fileMock = new Mock<IFormFile>();

            var content = "this is a test file";
            var fileName = "test.pdf";
            var memoryStream = new MemoryStream();
            var writer = new StreamWriter(memoryStream);
            writer.Write(content);
            writer.Flush();
            memoryStream.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(memoryStream);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(memoryStream.Length);

            var file = fileMock.Object;
            var boardId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var pinId = Guid.Empty;

            var addPin = new AddPinToBoard { BoardId = boardId, UserId = userId, PinId = pinId };

            this.userRepository.Setup(x => x.ExistsAsync(userId)).ReturnsAsync(true);
            this.boardRepository.Setup(x => x.IsBoardExistsAsync(boardId)).ReturnsAsync(true);
            this.boardRepository.Setup(x => x.IsBoardBelongToUserAsync(boardId, userId)).ReturnsAsync(true);
            this.pinBoardRepository.Setup(x => x.IsPinBelongToBoardAsync(boardId, pinId)).ReturnsAsync(false);
            this.pinBoardRepository.Setup(x => x.InsertPinBoard(boardId, pinId)).Returns(Task.CompletedTask);

            var result = this.uploadImageValidator.Validate(addPin);

            Assert.True(result.IsValid);
        }

        /// <summary>
        /// should return false.
        /// </summary>
        [Fact]
        public void Validate_Should_return_false_if_user_does_not_exists()
        {
            var boardId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var pinId = Guid.NewGuid();

            var addPin = new AddPinToBoard { BoardId = boardId, UserId = userId, PinId = pinId };

            this.userRepository.Setup(x => x.ExistsAsync(userId)).ReturnsAsync(false);
            this.boardRepository.Setup(x => x.IsBoardExistsAsync(boardId)).ReturnsAsync(true);
            this.boardRepository.Setup(x => x.IsBoardBelongToUserAsync(boardId, userId)).ReturnsAsync(true);
            this.pinBoardRepository.Setup(x => x.IsPinBelongToBoardAsync(boardId, pinId)).ReturnsAsync(true);
            this.pinBoardRepository.Setup(x => x.InsertPinBoard(boardId, pinId)).Returns(Task.CompletedTask);

            var result = this.uploadImageValidator.Validate(addPin);

            Assert.False(result.IsValid);
        }

        /// <summary>
        /// should return false.
        /// </summary>
        [Fact]
        public void Validate_Should_return_false_if_board_does_not_exists()
        {
            var boardId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var pinId = Guid.NewGuid();

            var addPin = new AddPinToBoard { BoardId = boardId, UserId = userId, PinId = pinId };

            this.userRepository.Setup(x => x.ExistsAsync(userId)).ReturnsAsync(true);
            this.boardRepository.Setup(x => x.IsBoardExistsAsync(boardId)).ReturnsAsync(false);
            this.boardRepository.Setup(x => x.IsBoardBelongToUserAsync(boardId, userId)).ReturnsAsync(true);
            this.pinBoardRepository.Setup(x => x.IsPinBelongToBoardAsync(boardId, pinId)).ReturnsAsync(true);
            this.pinBoardRepository.Setup(x => x.InsertPinBoard(boardId, pinId)).Returns(Task.CompletedTask);

            var result = this.uploadImageValidator.Validate(addPin);

            Assert.False(result.IsValid);
        }

        /// <summary>
        /// should return false.
        /// </summary>
        [Fact]
        public void Validate_Should_return_false_if_board_does_not_belongs_to_a_user()
        {
            var boardId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var pinId = Guid.NewGuid();

            var addPin = new AddPinToBoard { BoardId = boardId, UserId = userId, PinId = pinId };

            this.userRepository.Setup(x => x.ExistsAsync(userId)).ReturnsAsync(true);
            this.boardRepository.Setup(x => x.IsBoardExistsAsync(boardId)).ReturnsAsync(true);
            this.boardRepository.Setup(x => x.IsBoardBelongToUserAsync(boardId, userId)).ReturnsAsync(false);
            this.pinBoardRepository.Setup(x => x.IsPinBelongToBoardAsync(boardId, pinId)).ReturnsAsync(true);
            this.pinBoardRepository.Setup(x => x.InsertPinBoard(boardId, pinId)).Returns(Task.CompletedTask);

            var result = this.uploadImageValidator.Validate(addPin);

            Assert.False(result.IsValid);
        }

        /// <summary>
        /// should return false.
        /// </summary>
        [Fact]
        public void Validate_Should_return_true_if_pin_does_not_belongs_to_a_board()
        {
            var boardId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var pinId = Guid.NewGuid();

            var addPin = new AddPinToBoard { BoardId = boardId, UserId = userId, PinId = pinId };

            this.userRepository.Setup(x => x.ExistsAsync(userId)).ReturnsAsync(true);
            this.boardRepository.Setup(x => x.IsBoardExistsAsync(boardId)).ReturnsAsync(true);
            this.boardRepository.Setup(x => x.IsBoardBelongToUserAsync(boardId, userId)).ReturnsAsync(true);
            this.pinBoardRepository.Setup(x => x.IsPinBelongToBoardAsync(boardId, pinId)).ReturnsAsync(false);
            this.pinBoardRepository.Setup(x => x.InsertPinBoard(boardId, pinId)).Returns(Task.CompletedTask);

            var result = this.uploadImageValidator.Validate(addPin);

            Assert.True(result.IsValid);
        }
    }
}
