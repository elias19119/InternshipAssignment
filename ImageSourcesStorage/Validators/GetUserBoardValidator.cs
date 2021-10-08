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
          private readonly IUserRepository<User> userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserBoardValidator"/> class.
        /// </summary>
        /// <param name="userRepository"></param>
        public GetUserBoardValidator(IUserRepository<User> boardRepositroy)
        {
            this.userRepository = userRepository;
            this.RuleFor(x => x.UserId).MustAsync(this.IsUserExistsAsync);
        }

        private async Task<bool> IsUserExistsAsync(Guid userId, CancellationToken cancellation)
        {
            var isExists = await this.userRepository.UserIdExistsAsync(userId);

            return isExists;
        }
    }
}
