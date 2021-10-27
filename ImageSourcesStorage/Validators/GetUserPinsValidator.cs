namespace ImageSourcesStorage.Validators
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.DataAccessLayer.Models;

    /// <summary>
    /// this class is to validate get user pins request.
    /// </summary>
    public class GetUserPinsValidator : AbstractValidator<Guid>
    {
        private readonly IUserRepository<User> userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserPinsValidator"/> class.
        /// </summary>
        /// <param name="userRepository"></param>
        public GetUserPinsValidator(IUserRepository<User> userRepository)
        {
            this.userRepository = userRepository;
            this.RuleFor(x => x).MustAsync(this.IsUserExistsAsync);
        }

        private async Task<bool> IsUserExistsAsync(Guid userId, CancellationToken cancellation)
        {
            var isExists = await this.userRepository.ExistsAsync(userId);

            return isExists;
        }
    }
}
