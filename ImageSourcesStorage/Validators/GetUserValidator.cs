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
    public class GetUserValidator : AbstractValidator<User>
    {
        private readonly IUserRepository<User> userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserValidator"/> class.
        /// Initializes a new user of the <see cref="GetUserValidator"/> class.
        /// </summary>
        public GetUserValidator(IUserRepository<User> userRepository)
        {
            this.userRepository = userRepository;
            this.RuleFor(x => x.UserId).MustAsync(this.UniqueId);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserValidator"/> class.
        /// </summary>
        public GetUserValidator()
        {
            this.RuleFor(x => x.UserId).MustAsync(this.UniqueId);
        }

        /// <summary>
        /// rule for checking if User Exist.
        /// </summary>
        /// &lt;param name = "name" &gt;&lt;/ param &gt;
        /// <returns> true. </returns>
        public override ValidationResult Validate(ValidationContext<User> user)
        {
            return (user.InstanceToValidate == null)
                ? new ValidationResult(new[] { new ValidationFailure("Property", "Error Message") })
                : base.Validate(user);
        }

        private async Task<bool> UniqueId(Guid userId, CancellationToken cancellation)
        {
            var isExists = await this.userRepository.ExistsAsync(userId);

            return isExists;
        }
    }
}
