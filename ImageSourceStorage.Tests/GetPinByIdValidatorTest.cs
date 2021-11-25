namespace ImageSourceStorage.Tests
{
    using System;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.DataAccessLayer.Models;
    using ImageSourcesStorage.Validators;
    using Moq;
    using Xunit;

    public class GetPinByIdValidatorTest
    {
        private readonly GetPinByIdValidator getPinByIdValidator;
        private readonly Mock<IPinRepository<Pin>> pinRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetPinByIdValidatorTest"/> class.
        /// </summary>
        public GetPinByIdValidatorTest()
        {
            this.pinRepository = new Mock<IPinRepository<Pin>>();
            this.getPinByIdValidator = new GetPinByIdValidator(this.pinRepository.Object);
        }

        /// <summary>
        /// should return id does not exists .
        /// </summary>
        [Fact]
        public void Validate_should_return_false_if_id_does_not_exists()
        {
            var pin = new Pin()
            {
                PinId = Guid.NewGuid(),
            };

            this.pinRepository.Setup(x => x.IsPinExistsAsync(pin.PinId)).ReturnsAsync(false);

            var result = this.getPinByIdValidator.Validate(pin);

            Assert.False(result.IsValid);
        }

        /// <summary>
        /// should return id exists .
        /// </summary>
        [Fact]
        public void Validate_should_return_true_if_id_exists()
        {
            var pin = new Pin()
            {
                PinId = Guid.NewGuid(),
            };

            this.pinRepository.Setup(x => x.IsPinExistsAsync(pin.PinId)).ReturnsAsync(true);

            var result = this.getPinByIdValidator.Validate(pin);

            Assert.True(result.IsValid);
        }
    }
}
