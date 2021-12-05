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
    public class CheckUserIdValidator : AbstractValidator<User>
    {
        private readonly IUserRepository userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckUserIdValidator"/> class.
        /// Initializes a new user of the <see cref="CheckUserIdValidator"/> class.
        /// </summary>
        public CheckUserIdValidator(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
            this.RuleFor(x => x.UserId).MustAsync(this.IsUserExistsAsync);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckUserIdValidator"/> class.
        /// </summary>
        public CheckUserIdValidator()
        {
            this.RuleFor(x => x.UserId).MustAsync(this.IsUserExistsAsync);
        }

        /// <summary>
        /// rule for checking if User Exist.
        /// </summary>
        /// <returns> true. </returns>
        public override ValidationResult Validate(ValidationContext<User> user)
        {
            return (user.InstanceToValidate == null)
                ? new ValidationResult(new[] { new ValidationFailure("Property", "Error Message") })
                : base.Validate(user);
        }

        private async Task<bool> IsUserExistsAsync(Guid userId, CancellationToken cancellation)
        {
            var isExists = await this.userRepository.ExistsAsync(userId);

            return isExists;
        }
    }
}
