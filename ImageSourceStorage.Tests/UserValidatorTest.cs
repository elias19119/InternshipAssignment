namespace ImageSourceStorage.Tests
{
    using FluentValidation;
    using ImageSourcesStorage.DataAccessLayer.Models;
    using ImageSourcesStorage.DataAccessLayer.Validators;
    using Xunit;

    /// <summary>
    /// this class test the User validator.
    /// </summary>
    public class UserValidatorTest
    {
        private UserValidator userValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserValidatorTest"/> class.
        /// </summary>
        public UserValidatorTest()
        {
            this.userValidator = new UserValidator();
        }

        /// <summary>
        /// Should return empty.
        /// </summary>
        [Fact]
        public void Should_have_error_when_Name_is_EmptyAsync()
        {
            User user = new User
            {
                Name = string.Empty,
            };

            var result = this.userValidator.Validate(user, options => options.IncludeRuleSets("Name"));

            Assert.True(!result.IsValid);
        }

        /// <summary>
        /// Should return error.
        /// </summary>
        [Fact]
        public void Should_have_error_when_Name_Is_Longer_than_50_Characters()
        {
            User user = new User
            {
                Name = "qwueihrqiuwehruqwehtiuhwqeiuthiqwuehriuwqehriuwehqiuhiuewrhiewhriuerwhiu",
            };

            var result = this.userValidator.Validate(user, options => options.IncludeRuleSets("Name"));

            Assert.True(!result.IsValid);
        }

        /// <summary>
        /// user not empty .
        /// </summary>
        [Fact]
        public void User_Not_Empty()
        {
            User user = null;

            var result = this.userValidator.Validate(user);

            Assert.True(!result.IsValid);
        }
    }
}
