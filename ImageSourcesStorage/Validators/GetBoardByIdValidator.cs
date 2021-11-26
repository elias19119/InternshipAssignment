namespace ImageSourcesStorage.Validators
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.DataAccessLayer.Models;

    /// <summary>
    /// this class is to validate Get Board By Id request.
    /// </summary>
    public class GetBoardByIdValidator : AbstractValidator<Board>
    {
        private readonly IBoardRepository boardRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetBoardByIdValidator"/> class.
        /// </summary>
        public GetBoardByIdValidator(IBoardRepository boardRepository)
        {
            this.boardRepository = boardRepository;
            this.RuleFor(x => x.BoardId).MustAsync(this.IsBoardExistsAsync);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetBoardByIdValidator"/> class.
        /// </summary>
        public GetBoardByIdValidator()
        {
            this.RuleFor(x => x.BoardId).MustAsync(this.IsBoardExistsAsync);
        }

        private async Task<bool> IsBoardExistsAsync(Guid boardId, CancellationToken cancellation)
        {
            var isExists = await this.boardRepository.IsBoardExistsAsync(boardId);

            return isExists;
        }
    }
}
