using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ImageSourcesStorage.Models;

namespace ImageSourcesStorage.Controllers
{
    [Route("api/image_sources")]
    [ApiController]
    public class ImageSourcesController : ControllerBase
    {
        private readonly ImageSourceContext _context;

        public ImageSourcesController(ImageSourceContext context)
        {
            _context = context;
        }

        // GET: api/image-sources
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImageSource>>> GetImageSourcesAsync()
        {
            return await _context.ImageSources.ToListAsync();
        }

        // GET: api/image-sources/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ImageSource>> GetImageSourceAsync(long id)
        {
            var imageSource = await _context.ImageSources.FindAsync(id);

            if (imageSource == null)
            {
                return NotFound();
            }
            return imageSource;
        }

        // PUT: api/image-sources/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImageSourceAsync(long id, ImageSource imageSource)
        {
            if (id != imageSource.Id)
            {
                return BadRequest();
            }
            _context.Entry(imageSource).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImageSourceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        // POST: api/image-sources
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ImageSource>> PostImageSourceAsync(ImageSource imageSource)
        {
            _context.ImageSources.Add(imageSource);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetImageSource", new { id = imageSource.Id }, imageSource);
        }

        // DELETE: api/image-sources/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ImageSource>> DeleteImageSourceAsync(long id)
        {
            var imageSource = await _context.ImageSources.FindAsync(id);
            if (imageSource == null)
            {
                return NotFound();
            }
            _context.ImageSources.Remove(imageSource);
            await _context.SaveChangesAsync();

            return imageSource;
        }

        private bool ImageSourceExists(long id)
        {
            return _context.ImageSources.Any(e => e.Id == id);
        }
    }
}
