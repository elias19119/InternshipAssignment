namespace ImageSourcesStorage.Validators
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using ImageSourcesStorage.DataAccessLayer;

    /// <summary>
    /// this class is to validate get pin by Id.
    /// </summary>
    public class GetPinByIdValidator : AbstractValidator<Guid>
    {
        private readonly IPinRepository pinRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetPinByIdValidator"/> class.
        /// </summary>
        /// <param name="pinRepository"></param>
        public GetPinByIdValidator(IPinRepository pinRepository)
        {
            this.pinRepository = pinRepository;
            this.RuleFor(x => x).MustAsync(this.IsPinExistsAsync);
        }

        private async Task<bool> IsPinExistsAsync(Guid pinId, CancellationToken cancellation)
        {
            var isExists = await this.pinRepository.IsPinExistsAsync(pinId);

            return isExists;
        }
    }
}
