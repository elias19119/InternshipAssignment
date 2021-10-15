namespace ImageSourcesStorage.Validators
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.DataAccessLayer.Models;

    public class DeleteBoardOfUserValidator : AbstractValidator<Board>
    {
        private readonly IBoardRepository boardRepository;
        private readonly IUserRepository<User> userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteBoardOfUserValidator"/> class.
        /// </summary>
        public DeleteBoardOfUserValidator(IUserRepository<User> userRepository, IBoardRepository boardRepository)
        {
            this.boardRepository = boardRepository;
            this.userRepository = userRepository;
            this.RuleFor(x => x.BoardId).MustAsync(this.IsBoardExistsAsync);
            this.RuleFor(x => x.UserId).MustAsync(this.IsUserExistsAsync);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteBoardOfUserValidator"/> class.
        /// </summary>
        public DeleteBoardOfUserValidator(IBoardRepository boardRepository)
        {
            this.boardRepository = boardRepository;
            this.RuleFor(x => x.BoardId).MustAsync(this.IsBoardExistsAsync);
        }

        private async Task<bool> IsBoardExistsAsync(Guid boardId, CancellationToken cancellation)
        {
            var isExists = await this.boardRepository.IsBoardExistsAsync(boardId);

            return isExists;
        }

        private async Task<bool> IsUserExistsAsync(Guid userId, CancellationToken cancellation)
        {
            var isExists = await this.userRepository.ExistsAsync(userId);

            return isExists;
        }
    }
}
