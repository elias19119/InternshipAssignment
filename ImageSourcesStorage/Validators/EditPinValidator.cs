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

    public class EditPinValidator : AbstractValidator<Pin>
    {
        private readonly IPinRepository pinRepository;
        private readonly IUserRepository userRepository;
        private const int MaxFieldLength = 50;
        private const int MinFieldLength = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditPinValidator"/> class.
        /// </summary>
        /// <param name="pinRepository"></param>
        public EditPinValidator(IPinRepository pinRepository, IUserRepository userRepository)
        {
            this.pinRepository = pinRepository;
            this.userRepository = userRepository;
            this.RuleFor(x => x.PinId).MustAsync(this.IsPinExistsAsync);
            this.RuleFor(x => x.UserId).MustAsync(this.IsUserExistsAsync);
            this.RuleFor(x => x.Name).NotEmpty().Length(MinFieldLength, MaxFieldLength);
            this.RuleFor(x => x.Description).NotEmpty().Length(MinFieldLength, MaxFieldLength);
            this.RuleFor(x => x).MustAsync(this.IsPinBelongToUserAsync);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditPinValidator"/> class.
        /// </summary>
        public EditPinValidator()
        {
            this.RuleFor(x => x.PinId).MustAsync(this.IsPinExistsAsync);
        }

        private async Task<bool> IsPinExistsAsync(Guid pinId, CancellationToken cancellation)
        {
            var isExists = await this.pinRepository.IsPinExistsAsync(pinId);

            return isExists;
        }

        private async Task<bool> IsUserExistsAsync(Guid userId, CancellationToken cancellation)
        {
            var isExists = await this.userRepository.ExistsAsync(userId);

            return isExists;
        }

        public async Task<bool> IsPinBelongToUserAsync(Pin pin , CancellationToken cancellation)
        {
            var isBelongs = await this.pinRepository.IsPinBelongToUserAsync(pin.PinId, pin.UserId);

            return isBelongs;
        }
    }
}