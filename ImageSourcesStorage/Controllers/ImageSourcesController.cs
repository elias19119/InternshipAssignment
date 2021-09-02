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
    [Route("api/image_sources")]
    [ApiController]
    public class ImageSourcesController : ControllerBase
    {
        private readonly IImageSourceRepository _imageSourceRepository;


        public ImageSourcesController(IImageSourceRepository imageSourceRepository)
        {
            this._imageSourceRepository = imageSourceRepository;
        }

        // GET: api/image-sources
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImageSource>>> GetImageSourcesAsync()
        {
            var result =  await _imageSourceRepository.GetAllAsync();
            return Ok(result);
        }

        // GET: api/image-sources/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ImageSource>> GetImageSourceAsync(int id)
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
        public async Task<IActionResult> PutImageSourceAsync(int id, ImageSource imageSource)
        {
            if (id != imageSource.Id)
            {
                return BadRequest();
            }
            await _imageSourceRepository.UpdateAsync(imageSource);
           try
            {
                await _imageSourceRepository.SaveAsync();
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
        public async Task<ActionResult<ImageSource>> PostImageSourceAsync(ImageSource imageSource)
        {
            
            await _imageSourceRepository.InsertAsync(imageSource);
            await _imageSourceRepository.SaveAsync();

            return CreatedAtAction("GetImageSource", new { id = imageSource.Id }, imageSource); //201
        }

        // DELETE: api/image-sources/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ImageSource>> DeleteImageSourceAsync(int id)
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
