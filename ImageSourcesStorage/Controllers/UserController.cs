namespace ImageSourcesStorage.Controllers
{
    using System;
    using System.Threading.Tasks;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.DataAccessLayer.Models;
    using ImageSourcesStorage.Models;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository<User> userRepository;

        public UserController(IUserRepository<User> userRepository)
        {
            this.userRepository = userRepository;
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
            var user = await this.userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return this.NotFound();
            }

            return this.Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> PostUserAsync(CreateUserRequest request)
        {
            var user = new User
            {
                Name = request.Name,
            };
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
