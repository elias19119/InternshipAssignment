namespace ImageSourcesStorage.Validators
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.DataAccessLayer.Models;

    /// <summary>
    /// to validate the Get User Board method.
    /// </summary>
    public class GetUserBoardValidator : AbstractValidator<BoardModelDetails>
    {
          private readonly IUserRepository userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserBoardValidator"/> class.
        /// </summary>
        /// <param name="userRepository"></param>
          public GetUserBoardValidator(IUserRepository userRepository)
          {
            this.userRepository = userRepository;
            this.RuleFor(x => x.UserId).MustAsync(this.IsUserExistsAsync);
          }

          private async Task<bool> IsUserExistsAsync(Guid userId, CancellationToken cancellation)
          {
            var isExists = await this.userRepository.ExistsAsync(userId);

            return isExists;
          }
    }
}
