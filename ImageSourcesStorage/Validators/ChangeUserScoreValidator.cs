namespace ImageSourcesStorage.Validators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.DataAccessLayer.Models;
    using ImageSourcesStorage.Models;

    /// <summary>
    /// this class is to validate change user score.
    /// </summary>
    public class ChangeUserScoreValidator : AbstractValidator<Guid>
    {
        private readonly IUserRepository<User> userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeUserScoreValidator"/> class.
        /// Initializes a new user of the <see cref="ChangeUserScoreValidator"/> class.
        /// </summary>
        public ChangeUserScoreValidator(IUserRepository<User> userRepository)
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
