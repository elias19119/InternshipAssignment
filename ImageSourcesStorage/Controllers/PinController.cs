namespace ImageSourcesStorage.Controllers
{
    using System.Threading.Tasks;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.Models;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/pins")]
    [ApiController]
    public class PinController : ControllerBase
    {
        private readonly IPinRepository pinRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PinController"/> class.
        /// </summary>
        /// <param name="pinRepository"></param>
        public PinController(IPinRepository pinRepository)
        {
            this.pinRepository = pinRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPinsAsync()
        {
            var result = await this.pinRepository.GetAllPinsAsync();
            var response = new GetPinsResponse(result);

            return this.Ok(response);
        }
    }
}
