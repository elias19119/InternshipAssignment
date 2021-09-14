using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageSourcesStorage.DataAccessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace ImageSourcesStorage.Controllers
{
    [Route("api/pins")]
    [ApiController]
    public class PinController : ControllerBase
    {
        private readonly IPinRepository _pinRepository;

        public PinController(IPinRepository pinRepository)
        {
            this._pinRepository = pinRepository;
        }

        // GET: api/pins
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pin>>> GetImageSourcesAsync()
        {
            var result = await _pinRepository.GetAllAsync();
            return Ok(result);
        }

        // GET: api/pins/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetImageSourceAsync(Guid id)
        {
            var imageSource = await _pinRepository.GetByIdAsync(id);
            if (imageSource == null)
            {
                return NotFound();
            }
            return Ok(imageSource); //200
        }

        // PUT: api/pins/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImageSourceAsync(Guid id, Pin pin)
        {
            if (id != pin.PinId)
            {
                return BadRequest();
            }
            try
            {
                await _pinRepository.UpdateAsync(pin);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _pinRepository.ExistsAsync(id))
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

        // POST: api/pins
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Pin>> PostImageSourceAsync(Pin pin)
        {
            await _pinRepository.InsertAsync(pin);
            return CreatedAtAction("GetImageSource", new { id = pin.PinId }, pin); //201
        }

        // DELETE: api/pins/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Pin>> DeleteImageSourceAsync(Guid id)
        {
            if (!await _pinRepository.ExistsAsync(id))
            {
                return NotFound();
            }
            await _pinRepository.DeleteAsync(id);
            return NoContent();
        }

    }
}
