namespace ImageSourcesStorage.DataAccessLayer.Validators
{
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using FluentValidation.Results;
    using ImageSourcesStorage.DataAccessLayer.Models;

    /// <summary>
    /// User Validation.
    /// </summary>
    public class UserValidator : AbstractValidator<User>
    {
        private IUserRepository<User> userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserValidator"/> class.
        /// Initializes a new user of the <see cref="UserValidator"/> class.
        /// </summary>
        public UserValidator(IUserRepository<User> userRepository)
        {
            this.userRepository = userRepository;
            this.RuleFor(x => x.Name).MustAsync(this.UniqueName);
            this.RuleFor(x => x.Name).Length(1, 50);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserValidator"/> class.
        /// </summary>
        public UserValidator()
        {
        }

        /// <summary>
        /// rule for checking if User Exist.
        /// </summary>
        /// <param name="user"></param>
        /// <returns> true. </returns>
        public override ValidationResult Validate(ValidationContext<User> user)
        {
            return (user.InstanceToValidate == null)
                ? new ValidationResult(new[] { new ValidationFailure("Property", "Error Message") })
                : base.Validate(user);
        }

        /// <summary>
        /// rule for checking unique Name.
        /// </summary>
        // / <param name = "name" ></ param >
        // / < returns > true. </ returns >
        private async Task<bool> UniqueName(string name, CancellationToken cancellation)
        {
            var user = await this.userRepository.ExistsNameAsync(name);

            return !user;
        }
    }
}
