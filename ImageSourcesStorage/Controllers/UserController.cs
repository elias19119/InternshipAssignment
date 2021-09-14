using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageSourcesStorage.DataAccessLayer;
using ImageSourcesStorage.DataAccessLayer.Models;
using ImageSourcesStorage.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ImageSourcesStorage.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserRepository<User> _userRepository;

        public UserController(IUserRepository<User> userRepository)
        {
            this._userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersAsync()
        {
            var Result = await _userRepository.GetAllAsync();
            return Ok(Result);
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> GetUserAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user); //200
        }

        [HttpPost]
        public async Task<IActionResult> PostUserAsync(CreateUserRequest request)
        {
            var user = new User
            {
                Name = request.Name
            };
            await _userRepository.InsertAsync(user);
            return CreatedAtAction("GetUsers", new { id = user.UserId }, request);
        }

        [HttpPut]
        public async Task<IActionResult> PutUserAsync(UpdateUserRequest request, Guid userId)
        {
            var user = new User
            {
                Name = request.Name,
                Score = request.Score
            };
            await _userRepository.UpdateAsync(user);

            if (!await _userRepository.ExistsAsync(userId))
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete]
        [Route("{userId}")]
        public async Task <IActionResult> DeleteUserAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId).ConfigureAwait(false);
            if (user == null)
            {
                return NotFound($"User with Id = {userId} not found");
            }
            await _userRepository.DeleteAsync(userId);
            return NoContent();
        }
    }
}

