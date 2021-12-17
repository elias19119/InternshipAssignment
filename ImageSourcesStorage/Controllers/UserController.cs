namespace ImageSourcesStorage.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.DataAccessLayer.Models;
    using ImageSourcesStorage.DataAccessLayer.Validators;
    using ImageSourcesStorage.Models;
    using ImageSourcesStorage.Validators;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/users")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly CheckUserIdValidator checkUserIdValidator;
        private readonly PostUserValidator postUserValidator;
        private readonly PutUserValidator putUserValidator;
        private readonly ChangeUserScoreValidator changeScoreValidator;
        private readonly GetUserPinsValidator getUserPinsValidator;

        public UserController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
            this.checkUserIdValidator = new CheckUserIdValidator(userRepository);
            this.postUserValidator = new PostUserValidator(userRepository);
            this.putUserValidator = new PutUserValidator(userRepository);
            this.changeScoreValidator = new ChangeUserScoreValidator(userRepository);
            this.getUserPinsValidator = new GetUserPinsValidator(userRepository);
        }

        /// <summary>
        /// Gets all the users in the system.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<User>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var result = await this.userRepository.GetAllAsync();
            return this.Ok(result);
        }

        /// <summary>
        /// Gets a User by id.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>200.</returns>
        /// <response code="404"> userId Not Found.</response>
        [HttpGet]
        [Route("{userId}")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserAsync(Guid userId)
        {
            var user = new User { UserId = userId };
            var result = this.checkUserIdValidator.Validate(user);

            return result.IsValid ? this.Ok(await this.userRepository.GetByIdAsync(userId)) : (ActionResult)this.NotFound();
        }

        /// <summary>
        /// Adds a new user.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>201.</returns>
        /// <response code="400"> Bad Request.</response>
        [HttpPost]
        [ProducesResponseType(typeof(AddUserResponse), StatusCodes.Status201Created)]
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

            var response = new AddUserResponse(user.UserId);

            return this.CreatedAtAction("GetUserAsync", new { user.UserId }, response);
        }

        /// <summary>
        /// Updates a user name and score.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns>204.</returns>
        /// <response code="400"> Bad Request.</response>
        [HttpPut]
        [Route("{userId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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

        /// <summary>
        /// Delete a user by id.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>204.</returns>
        /// <response code="404"> userId Not Found.</response>
        [HttpDelete]
        [Route("{userId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteUserAsync(Guid userId)
        {
            var user = new User { UserId = userId };

            var result = this.checkUserIdValidator.Validate(user);

            if (!result.IsValid)
            {
                return this.NotFound();
            }

            await this.userRepository.DeleteAsync(userId);
            return this.NoContent();
        }

        /// <summary>
        /// Decrease or increase a user score.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="changeScoreOptions"></param>
        /// <returns>204.</returns>
        /// <response code="400">Bad Request.</response>
        [HttpPost]
        [Route("{userId}/scores")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> ChangeUserScoreAsync(Guid userId, [Required] ChangeScoreOptions changeScoreOptions)
        {
            var user = new User
            {
                UserId = userId,
            };

            var result = this.changeScoreValidator.Validate(user);

            if (!result.IsValid)
            {
                return this.BadRequest();
            }

            await this.userRepository.ChangeUserScore(user.UserId, changeScoreOptions);

            return this.NoContent();
        }

        /// <summary>
        /// Gets user pins by id.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>200.</returns>
        /// <response code="404"> userId Not Found.</response>
        [HttpGet]
        [Route("{userId}/pins")]
        [ProducesResponseType(typeof(List<Pin>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserPinsAsync(Guid userId)
        {
            var pin = new Pin() { UserId = userId };

            var result = this.getUserPinsValidator.Validate(pin);

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
