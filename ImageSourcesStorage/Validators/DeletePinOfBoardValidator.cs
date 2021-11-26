namespace ImageSourcesStorage.Validators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.DataAccessLayer.Models;

    public class DeletePinOfBoardValidator : AbstractValidator<PinBoard>
    {
        private readonly IPinRepository pinRepository;
        private readonly IBoardRepository boardRepository;
        private readonly IPinBoardRepository pinBoardRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeletePinOfBoardValidator"/> class.
        /// </summary>
        /// <param name="pinRepository"></param>
        /// <param name="boardRepository"></param>
        public DeletePinOfBoardValidator(IPinRepository pinRepository, IBoardRepository boardRepository, IPinBoardRepository pinBoardRepository)
        {
            this.boardRepository = boardRepository;
            this.pinRepository = pinRepository;
            this.pinBoardRepository = pinBoardRepository;
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

        private async Task<bool> IsPinBelongToBoardAsync(PinBoard pin, CancellationToken cancellation)
        {
            var isBelongs = await this.pinBoardRepository.IsPinBelongToBoardAsync(pin.BoardId, pin.PinId);

            return isBelongs;
        }
    }
}
