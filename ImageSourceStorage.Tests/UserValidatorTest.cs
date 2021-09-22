namespace ImageSourceStorage.Tests
{
    using ImageSourcesStorage.DataAccessLayer.Models;
    using ImageSourcesStorage.DataAccessLayer.Validators;
    using Xunit;

    /// <summary>
    /// this class test the User validator.
    /// </summary>
    public class UserValidatorTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserValidatorTest"/> class.
        /// </summary>
        public UserValidatorTest()
        {
            this.userValidator = new UserValidator();
        }

        private UserValidator userValidator { get; }

        /// <summary>
        /// Should return empty.
        /// </summary>
        [Fact]
        public void Should_have_error_when_Name_is_Empty()
        {
            var user = new User()
            {
                Name = string.Empty,
            };

            var result = this.userValidator.Validate(user);

            Assert.True(!result.IsValid);
        }

        /// <summary>
        /// Should return error.
        /// </summary>
        [Fact]
        public void Should_have_error_when_Name_Is_Longer_than_50_Characters()
        {
            var user = new User()
            {
                Name = "eqwqerewrewrqrewqwqfsasdDDSADSADASDSDDSASDDSADSADSADSDSDSADSAASDDSADSDSA",
            };

            var result = this.userValidator.Validate(user);

            Assert.True(!result.IsValid);
        }
    }
}
