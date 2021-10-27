namespace ImageSourcesStorage.Controllers
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.DataAccessLayer.Models;
    using ImageSourcesStorage.DataAccessLayer.Validators;
    using ImageSourcesStorage.Models;
    using ImageSourcesStorage.Validators;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository<User> userRepository;
        private readonly CheckUserIdValidator checkUserIdValidator;
        private readonly PostUserValidator postUserValidator;
        private readonly PutUserValidator putUserValidator;
        private readonly ChangeUserScoreValidator changeScoreValidator;
        private readonly GetUserPinsValidator getUserPinsValidator;

        public UserController(IUserRepository<User> userRepository)
        {
            this.userRepository = userRepository;
            this.checkUserIdValidator = new CheckUserIdValidator(userRepository);
            this.postUserValidator = new PostUserValidator(userRepository);
            this.putUserValidator = new PutUserValidator(userRepository);
            this.changeScoreValidator = new ChangeUserScoreValidator(userRepository);
            this.getUserPinsValidator = new GetUserPinsValidator(userRepository);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersAsync()
        {
            var result = await this.userRepository.GetAllAsync();
            return this.Ok(result);
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> GetUserAsync(Guid userId)
        {
            var result = this.checkUserIdValidator.Validate(userId);

            return result.IsValid ? this.Ok(await this.userRepository.GetByIdAsync(userId)) : (ActionResult)this.NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> PostUserAsync(CreateUserRequest request)
        {
            var user = new User
            {
                Name = request.Name,
            };

            var result = this.postUserValidator.Validate(user);

            if (!result.IsValid)
            {
                return this.BadRequest();
            }

            await this.userRepository.InsertAsync(user);
            return this.CreatedAtAction("GetUsers", new { id = user.UserId }, request);
        }

        [HttpPut]
        [Route("{userId}")]
        public async Task<IActionResult> PutUserAsync(UpdateUserRequest request, Guid userId)
        {
            var user = new User
            {
                Name = request.Name,
                Score = request.Score,
                UserId = userId,
            };

            var result = this.putUserValidator.Validate(user);

            if (!result.IsValid)
            {
                return this.BadRequest();
            }

            await this.userRepository.UpdateAsync(user.UserId, user.Name, user.Score);
            return this.NoContent();
        }

        [HttpDelete]
        [Route("{userId}")]
        public async Task<IActionResult> DeleteUserAsync(Guid userId)
        {
            var result = this.checkUserIdValidator.Validate(userId);

            if (!result.IsValid)
            {
                return this.NotFound();
            }

            await this.userRepository.DeleteAsync(userId);
            return this.NoContent();
        }

        [HttpPost]
        [Route("{userId}/scores")]
        public async Task<IActionResult> ChangeUserScoreAsync(Guid userId, [Required]ChangeScoreOptions changeScoreOptions)
        {
            var result = this.changeScoreValidator.Validate(userId);

            if (!result.IsValid)
            {
                return this.BadRequest();
            }

            await this.userRepository.ChangeUserScore(userId, changeScoreOptions);

            return this.NoContent();
        }

        [HttpGet]
        [Route("{userId}/pins")]
        public async Task<IActionResult> GetUserPinsAsync(Guid userId)
        {
            var result = this.getUserPinsValidator.Validate(userId);

            if (!result.IsValid)
            {
                return this.NotFound();
            }

            var pins = await this.userRepository.GetUserPinsAsync(userId);

            var response = new GetUserPinsResponse(pins);

            return this.Ok(response);
        }
    }
}
