namespace ImageSourcesStorage.DataAccessLayer.Validators
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using FluentValidation.Results;
    using ImageSourcesStorage.DataAccessLayer.Models;

    /// <summary>
    /// User Validation.
    /// </summary>
    public class CheckUserIdValidator : AbstractValidator<Guid>
    {
        private readonly IUserRepository<User> userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckUserIdValidator"/> class.
        /// Initializes a new user of the <see cref="CheckUserIdValidator"/> class.
        /// </summary>
        public CheckUserIdValidator(IUserRepository<User> userRepository)
        {
            this.userRepository = userRepository;
            this.RuleFor(x => x).MustAsync(this.IsUserExistsAsync);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckUserIdValidator"/> class.
        /// </summary>
        public CheckUserIdValidator()
        {
            this.RuleFor(x => x).MustAsync(this.IsUserExistsAsync);
        }

        private async Task<bool> IsUserExistsAsync(Guid userId, CancellationToken cancellation)
        {
            var isExists = await this.userRepository.ExistsAsync(userId);

            return isExists;
        }
    }
}
