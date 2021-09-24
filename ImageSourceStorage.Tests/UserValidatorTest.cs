namespace ImageSourceStorage.Tests
{
    using ImageSourcesStorage.DataAccessLayer.Models;
    using ImageSourcesStorage.DataAccessLayer.Validators;
    using ImageSourcesStorage.Validators;
    using Xunit;

    /// <summary>
    /// this class test the User validator.
    /// </summary>
    public class UserValidatorTest
    {
        private GetUserValidator getUserValidator;
        private PostUserValidator postUserValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserValidatorTest"/> class.
        /// </summary>
        public UserValidatorTest()
        {
            this.getUserValidator = new GetUserValidator();
            this.postUserValidator = new PostUserValidator();
        }

        /// <summary>
        /// Should return empty.
        /// </summary>
        [Fact]
        public void Validate_should_return_false_if_Name_is_Empty()
        {
            User user = new User
            {
                Name = string.Empty,
            };

            var result = this.postUserValidator.Validate(user);

            Assert.False(result.IsValid);
        }

        /// <summary>
        /// Should return error.
        /// </summary>
        [Fact]
        public void Validate_should_return_false_if_name_Exceed_50_Characters()
        {
            User user = new User
            {
                Name = "qwueihrqiuwehruqwehtiuhwqeiuthiqwuehriuwqehriuwehqiuhiuewrhiewhriuerwhiu",
            };

            var result = this.postUserValidator.Validate(user);

            Assert.False(result.IsValid);
        }

        /// <summary>
        /// user not empty .
        /// </summary>
        [Fact]
        public void Validate_should_return_false_if_user_is_null()
        {
            User user = new User()
            {
            };

            var result = this.postUserValidator.Validate(user);

            Assert.False(result.IsValid);
        }
    }
}
