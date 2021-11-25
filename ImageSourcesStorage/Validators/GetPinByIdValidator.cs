namespace ImageSourcesStorage.Validators
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.DataAccessLayer.Models;

    /// <summary>
    /// this class is to validate get pin by Id.
    /// </summary>
    public class GetPinByIdValidator : AbstractValidator<Pin>
    {
        private readonly IPinRepository<Pin> pinRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetPinByIdValidator"/> class.
        /// </summary>
        /// <param name="pinRepository"></param>
        public GetPinByIdValidator(IPinRepository<Pin> pinRepository)
        {
            this.pinRepository = pinRepository;
            this.RuleFor(x => x.PinId).MustAsync(this.IsPinExistsAsync);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetPinByIdValidator"/> class.
        /// </summary>
        public GetPinByIdValidator()
        {
            this.RuleFor(x => x.PinId).MustAsync(this.IsPinExistsAsync);
        }

        private async Task<bool> IsPinExistsAsync(Guid pinId, CancellationToken cancellation)
        {
            var isExists = await this.pinRepository.IsPinExistsAsync(pinId);

            return isExists;
        }
    }
}
