namespace ImageSourcesStorage.Validators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using ImageSourcesStorage.DataAccessLayer;

    public class DeletePinOfBoardValidator : AbstractValidator<Pin>
    {
        private readonly IPinRepository pinRepository;
        private readonly IBoardRepository boardRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeletePinOfBoardValidator"/> class.
        /// </summary>
        /// <param name="pinRepository"></param>
        /// <param name="boardRepository"></param>
        public DeletePinOfBoardValidator(IPinRepository pinRepository, IBoardRepository boardRepository)
        {
            this.boardRepository = boardRepository;
            this.pinRepository = pinRepository;
            this.RuleFor(x => x).MustAsync(this.IsPinBelongToBoardAsync);
            this.RuleFor(x => x.BoardId).MustAsync(this.IsBoardExistsAsync);
            this.RuleFor(x => x.PinId).MustAsync(this.IsPinExistsAsync);
        }

        private async Task<bool> IsBoardExistsAsync(Guid boardId, CancellationToken cancellation)
        {
            var isExists = await this.boardRepository.IsBoardExistsAsync(boardId);

            return isExists;
        }

        private async Task<bool> IsPinExistsAsync(Guid pinId, CancellationToken cancellation)
        {
            var isExists = await this.pinRepository.IsPinExistsAsync(pinId);

            return isExists;
        }

        private async Task<bool> IsPinBelongToBoardAsync(Pin pin, CancellationToken cancellation)
        {
            var isBelongs = await this.pinRepository.IsPinBelongToBoardAsync(pin.BoardId, pin.PinId);

            return isBelongs;
        }
    }
}
