using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageSourcesStorage.DataAccessLayer;
using ImageSourcesStorage.DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ImageSourcesStorage.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        // GET: api/image-sources
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUserAsync()
        {
            var result = await _userRepository.GetAllAsync();
            return Ok(result);
        }

        // GET: api/image-sources/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user); //200
        }

        // PUT: api/image-sources/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserAsync(Guid id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }
            try
            {
                await _userRepository.UpdateAsync(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _userRepository.ExistsAsync(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent(); //202
        }

        // POST: api/image-sources
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<User>> PostUserAsync(User user)
        {
            await _userRepository.InsertAsync(user);
            return CreatedAtAction("GetUser", new { id = user.UserId }, user); //201
        }

        // DELETE: api/image-sources/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUserAsync(Guid id)
        {
            if (!await _userRepository.ExistsAsync(id))
            {
                return NotFound();
            }
            await _userRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
