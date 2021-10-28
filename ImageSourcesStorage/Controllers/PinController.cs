namespace ImageSourcesStorage.Controllers
{
    using System;
    using System.Threading.Tasks;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.DataAccessLayer.Models;
    using ImageSourcesStorage.Models;
    using ImageSourcesStorage.Validators;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    public class PinController : ControllerBase
    {
        private readonly IPinRepository pinRepository;
        private readonly IUserRepository<User> userRepository;
        private readonly GetPinByIdValidator getPinByIdValidator;
        private readonly GetUserPinsValidator getUserPinsValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="PinController"/> class.
        /// </summary>
        /// <param name="pinRepository"></param>
        public PinController(IPinRepository pinRepository, IUserRepository<User> userRepository)
        {
            this.pinRepository = pinRepository;
            this.userRepository = userRepository;
            this.getPinByIdValidator = new GetPinByIdValidator(pinRepository);
            this.getUserPinsValidator = new GetUserPinsValidator(userRepository);
        }

        [HttpGet]
        [Route("api/pins")]
        public async Task<IActionResult> GetAllPinsAsync()
        {
            var result = await this.pinRepository.GetAllPinsAsync();
            var response = new GetPinsResponse(result);

            return this.Ok(response);
        }

        [HttpGet]
        [Route("api/pins/{pinId}")]
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

        [HttpGet]
        [Route("api/users/{userId}/pins")]
        public async Task<IActionResult> GetUserPinsAsync(Guid userId)
        {
            var pin = new Pin() { UserId = userId };

            var result = this.getUserPinsValidator.Validate(pin);

            if (!result.IsValid)
            {
                return this.NotFound();
            }

            var pins = await this.pinRepository.GetUserPinsAsync(userId);

            var response = new GetUserPinsResponse(pins);

            return this.Ok(response);
        }
    }
}
