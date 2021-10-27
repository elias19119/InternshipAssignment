namespace ImageSourceStorage.Tests
{
    using System;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.Validators;
    using Moq;
    using Xunit;

    public class GetPinByIdValidatorTest
    {
        private readonly GetPinByIdValidator getPinByIdValidator;
        private readonly Mock<IPinRepository> pinRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetPinByIdValidatorTest"/> class.
        /// </summary>
        public GetPinByIdValidatorTest()
        {
            this.pinRepository = new Mock<IPinRepository>();
            this.getPinByIdValidator = new GetPinByIdValidator(this.pinRepository.Object);
        }

        /// <summary>
        /// should return id does not exists .
        /// </summary>
        [Fact]
        public void Validate_should_return_false_if_id_does_not_exists()
        {
            var pinId = Guid.NewGuid();

            this.pinRepository.Setup(x => x.IsPinExistsAsync(pinId)).ReturnsAsync(false);

            var result = this.getPinByIdValidator.Validate(pinId);

            Assert.False(result.IsValid);
        }

        /// <summary>
        /// should return id exists .
        /// </summary>
        [Fact]
        public void Validate_should_return_true_if_id_exists()
        {
            var pinId = Guid.NewGuid();

            this.pinRepository.Setup(x => x.IsPinExistsAsync(pinId)).ReturnsAsync(true);

            var result = this.getPinByIdValidator.Validate(pinId);

            Assert.True(result.IsValid);
        }
    }
}
