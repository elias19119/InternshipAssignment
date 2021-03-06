namespace ImageSourcesStorage.Validators
{
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.DataAccessLayer.Models;

    /// <summary>
    /// this class is used to validate Post end point.
    /// </summary>
    public class PostUserValidator : AbstractValidator<User>
    {
        private readonly IUserRepository userRepository;
        private const int MaxFieldLength = 50;
        private const int MinFieldLength = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostUserValidator"/> class.
        /// Initializes a new user of the <see cref="PostUserValidator"/> class.
        /// </summary>
        public PostUserValidator(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
            this.RuleFor(x => x.Name).MustAsync(this.IsNameUniqueAsync);
            this.RuleFor(x => x.Name).Length(MinFieldLength, MaxFieldLength);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PostUserValidator"/> class.
        /// </summary>
        public PostUserValidator()
        {
            this.RuleFor(x => x.Name).Length(MinFieldLength, MaxFieldLength).NotNull();
            this.RuleFor(x => x.Name).MustAsync(this.IsNameUniqueAsync);
        }

        /// <summary>
        /// rule for checking unique Name.
        /// </summary>
        // / <param name = "name" ></ param >
        // / < returns > true. </ returns >
        private async Task<bool> IsNameUniqueAsync(string name, CancellationToken cancellation)
        {
            var isExists = await this.userRepository.NameExistsAsync(name);

            return !isExists;
        }
    }
}
