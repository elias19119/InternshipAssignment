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
    [Route("api/Pin")]
    [ApiController]
    public class PinController : ControllerBase
    {
        private readonly IPinRepository _imageSourceRepository;

        public PinController(IPinRepository imageSourceRepository)
        {
            this._imageSourceRepository = imageSourceRepository;
        }

        // GET: api/image-sources
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pin>>> GetImageSourcesAsync()
        {
            var result = await _imageSourceRepository.GetAllAsync();
            return Ok(result);
        }

        // GET: api/image-sources/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pin>> GetImageSourceAsync(Guid id)
        {
            var imageSource = await _imageSourceRepository.GetByIdAsync(id);
            if (imageSource == null)
            {
                return NotFound();
            }
            return Ok(imageSource); //200
        }

        // PUT: api/image-sources/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImageSourceAsync(Guid id, Pin pin)
        {
            if (id != pin.Id)
            {
                return BadRequest();
            }
            try
            {
                await _imageSourceRepository.UpdateAsync(pin);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _imageSourceRepository.ExistsAsync(id))
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
        public async Task<ActionResult<Pin>> PostImageSourceAsync(Pin pin)
        {
            await _imageSourceRepository.InsertAsync(pin);
            return CreatedAtAction("GetImageSource", new { id = pin.Id }, pin); //201
        }

        // DELETE: api/image-sources/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Pin>> DeleteImageSourceAsync(Guid id)
        {
            if (!await _imageSourceRepository.ExistsAsync(id))
            {
                return NotFound();
            }
            await _imageSourceRepository.DeleteAsync(id);
            return NoContent();
        }

    }
}
