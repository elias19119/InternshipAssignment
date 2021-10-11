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

    /// <summary>
    /// this class is to validate the post board request.
    /// </summary>
    public class AddBoardtoUserValidator : AbstractValidator<Board>
    {
        private const int MaxFieldLength = 50;
        private const int MinFieldLength = 1;
        private readonly IBoardRepository boardRepository;
        private readonly IUserRepository<User> userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddBoardtoUserValidator"/> class.
        /// </summary>
        public AddBoardtoUserValidator(IUserRepository<User> userRepository , IBoardRepository boardRepository)
        {
            this.boardRepository = boardRepository;
            this.userRepository = userRepository;
            this.RuleFor(x => x.UserId).MustAsync(this.IsUserExistsAsync);
            this.RuleFor(x => x.Name).MustAsync(this.IsNameUniqueAsync).NotEmpty().Length(MinFieldLength, MaxFieldLength);
        }

        private async Task<bool> IsUserExistsAsync(Guid userId, CancellationToken cancellation)
        {
            var isExists = await this.userRepository.ExistsAsync(userId);

            return isExists;
        }

        private async Task<bool> IsNameUniqueAsync(string name, CancellationToken cancellation)
        {
            var isExists = await this.boardRepository.NameExistsAsync(name);

            return !isExists;
        }
    }
}
