namespace ImageSourcesStorage.Validators
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.DataAccessLayer.Models;
    using ImageSourcesStorage.Models;

    public class UploadImageValidator : AbstractValidator<AddPinToBoard>
    {
        private readonly IBoardRepository boardRepository;
        private readonly IUserRepository userRepository;
        private readonly IPinRepository pinRepository;
        private readonly IPinBoardRepository pinBoardRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UploadImageValidator"/> class.
        /// </summary>
        /// <param name="userRepository"></param>
        /// <param name="boardRepository"></param>
        public UploadImageValidator(IUserRepository userRepository, IBoardRepository boardRepository, IPinRepository pinRepository , IPinBoardRepository pinBoardRepository)
        {
            this.userRepository = userRepository;
            this.boardRepository = boardRepository;
            this.pinRepository = pinRepository;
            this.pinBoardRepository = pinBoardRepository;
            this.RuleFor(x => x.BoardId).MustAsync(this.IsBoardExistsAsync);
            this.RuleFor(x => x.UserId).MustAsync(this.IsUserExistsAsync);
            this.RuleFor(x => x).MustAsync(this.IsPinDoesNotBelongToBoardAsync);
            this.RuleFor(x => x).MustAsync(this.IsBoardBelongToUserAsync);
            this.RuleFor(x => x.PinId).MustAsync(this.IsPinExistsAsync).When(x => x.PinId != Guid.Empty);
        }

        private async Task<bool> IsUserExistsAsync(Guid userId, CancellationToken cancellation)
        {
            var isExists = await this.userRepository.ExistsAsync(userId);

            return isExists;
        }

        private async Task<bool> IsBoardExistsAsync(Guid boardId, CancellationToken cancellation)
        {
            var isExists = await this.boardRepository.IsBoardExistsAsync(boardId);

            return isExists;
        }

        private Task<bool> IsBoardBelongToUserAsync(AddPinToBoard board, CancellationToken cancellation)
        {
            var isBelong = this.boardRepository.IsBoardBelongToUserAsync(board.BoardId, board.UserId);

            return isBelong;
        }

        private async Task<bool> IsPinDoesNotBelongToBoardAsync(AddPinToBoard pin, CancellationToken cancellation)
        {
            var isBelongs = await this.pinBoardRepository.IsPinBelongToBoardAsync(pin.BoardId, pin.PinId);

            return !isBelongs;
        }

        public async Task<bool> IsPinExistsAsync(Guid pinId, CancellationToken cancellation)
        {
            var isExists = await this.pinRepository.IsPinExistsAsync(pinId);

            return isExists;
        }
    }
}
