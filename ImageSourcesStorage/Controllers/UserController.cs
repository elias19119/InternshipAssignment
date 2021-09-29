namespace ImageSourcesStorage.Controllers
{
    using System;
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
        private readonly GetUserValidator getUserValidator;
        private readonly PostUserValidator postUserValidator;

        public UserController(IUserRepository<User> userRepository)
        {
            this.userRepository = userRepository;
            this.getUserValidator = new GetUserValidator(userRepository);
            this.postUserValidator = new PostUserValidator(userRepository);
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
            var User = new User { UserId = userId };
            var result = this.getUserValidator.Validate(User);

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
        public async Task<IActionResult> PutUserAsync(UpdateUserRequest request, Guid userId)
        {
            var user = new User
            {
                Name = request.Name,
                Score = request.Score,
            };
            await this.userRepository.UpdateAsync(user);

            if (!await this.userRepository.ExistsAsync(userId))
            {
                return this.NotFound();
            }

            return this.NoContent();
        }

        [HttpDelete]
        [Route("{userId}")]
        public async Task<IActionResult> DeleteUserAsync(Guid userId)
        {
            var user = await this.userRepository.GetByIdAsync(userId).ConfigureAwait(false);
            if (user == null)
            {
                return this.NotFound($"Users with Id = {userId} not found");
            }

            await this.userRepository.DeleteAsync(userId);
            return this.NoContent();
        }
    }
}
