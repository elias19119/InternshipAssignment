namespace ImageSourcesStorage.Controllers
{
    using System;
    using System.Threading.Tasks;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.Models;
    using ImageSourcesStorage.Validators;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/pins")]
    [ApiController]
    public class PinController : ControllerBase
    {
        private readonly IPinRepository pinRepository;
        private readonly GetPinByIdValidator getPinByIdValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="PinController"/> class.
        /// </summary>
        /// <param name="pinRepository"></param>
        public PinController(IPinRepository pinRepository)
        {
            this.pinRepository = pinRepository;
            this.getPinByIdValidator = new GetPinByIdValidator(pinRepository);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPinsAsync()
        {
            var result = await this.pinRepository.GetAllPinsAsync();
            var response = new GetPinsResponse(result);

            return this.Ok(response);
        }

        [HttpGet]
        [Route("{pinId}")]
        public async Task<IActionResult> GetPinByIdAsync(Guid pinId)
        {
            var pin = new Pin() { PinId = pinId };

            var ispinvalid = this.getPinByIdValidator.Validate(pin);

            if (!ispinvalid.IsValid)
            {
                return this.NotFound();
            }

            var result = await this.pinRepository.GetPinByIdAsync(pinId);
            var response = new GetPinByIdResponse(result);

            return this.Ok(response);
        }
    }
}
