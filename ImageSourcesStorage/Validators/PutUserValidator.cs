namespace ImageSourcesStorage.Validators
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.DataAccessLayer.Models;

    /// <summary>
    /// this is class is used to put rules for Put method.
    /// </summary>
    public class PutUserValidator : AbstractValidator<User>
    {
        private readonly IUserRepository userRepository;
        private const int MaxFieldLength = 50;
        private const int MinFieldLength = 1;
        private const int MinScoreValue = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="PutUserValidator"/> class.
        /// Initializes a new user of the <see cref="PutUserValidator"/> class.
        /// </summary>
        public PutUserValidator(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
            this.RuleFor(x => x.UserId).MustAsync(this.IsUserExistsAsync);
            this.RuleFor(x => x.Name).MustAsync(this.IsNameUniqueAsync);
            this.RuleFor(x => x.Name).Length(MinFieldLength, MaxFieldLength);
            this.RuleFor(x => x.Score).NotEmpty();
            this.RuleFor(x => x.Score).GreaterThan(MinScoreValue);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PutUserValidator"/> class.
        /// </summary>
        public PutUserValidator()
        {
            this.RuleFor(x => x.Score).NotEmpty();
            this.RuleFor(x => x.Score).GreaterThan(MinScoreValue);
        }

        private async Task<bool> IsUserExistsAsync(Guid userId, CancellationToken cancellation)
        {
            var isExists = await this.userRepository.ExistsAsync(userId);

            return isExists;
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
