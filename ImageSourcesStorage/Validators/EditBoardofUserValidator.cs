namespace ImageSourcesStorage.Validators
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.DataAccessLayer.Models;

    /// <summary>
    /// this class is to validate the edit board of user request.
    /// </summary>
    public class EditBoardofUserValidator : AbstractValidator<Board>
    {
        private const int MaxFieldLength = 50;
        private const int MinFieldLength = 1;
        private readonly IBoardRepository boardRepository;
        private readonly IUserRepository<User> userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditBoardofUserValidator"/> class.
        /// </summary>
        public EditBoardofUserValidator(IUserRepository<User> userRepository, IBoardRepository boardRepository)
        {
            this.boardRepository = boardRepository;
            this.userRepository = userRepository;
            this.RuleFor(x => x.UserId).MustAsync(this.IsUserExistsAsync);
            this.RuleFor(x => x.BoardId).MustAsync(this.IsBoardExistsAsync);
            this.RuleFor(x => x.Name).MustAsync(this.IsNameUniqueAsync).NotEmpty().Length(MinFieldLength, MaxFieldLength);
            this.RuleFor(x => x).MustAsync(this.IsBoardBelongToUserAsync);
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

        private async Task<bool> IsNameUniqueAsync(string name, CancellationToken cancellation)
        {
            var isExists = await this.boardRepository.IsNameExistsAsync(name);

            return !isExists;
        }

        private async Task<bool> IsBoardBelongToUserAsync(Board board, CancellationToken cancellation)
        {
            var isBelong = await this.boardRepository.IsBoardBelongToUserAsync(board.BoardId, board.UserId);

            return isBelong;
        }
    }
}
