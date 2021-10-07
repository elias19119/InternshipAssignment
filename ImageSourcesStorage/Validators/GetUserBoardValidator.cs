namespace ImageSourcesStorage.Validators
{
    using FluentValidation;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.DataAccessLayer.Models;
    using System;
    using System.Threading;
    using System.Threading.Tasks;


    /// <summary>
    /// to validate the Get User Board method.
    /// </summary>
    public class GetUserBoardValidator : AbstractValidator<Board>
    {
        private readonly IBoardRepository boardRepositroy;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserBoardValidator"/> class.
        /// </summary>
        /// <param name="boardRepositroy"></param>
        public GetUserBoardValidator(IBoardRepository boardRepositroy)
        {
            this.boardRepositroy = boardRepositroy;
            this.RuleFor(x => x.UserId).MustAsync(this.IsUserExistsAsync);
        }

        private async Task<bool> IsUserExistsAsync(Guid userId, CancellationToken cancellation)
        {
            var isExists = await this.boardRepositroy.UserIdExistsAsync(userId);

            return isExists;
        }
    }
}
