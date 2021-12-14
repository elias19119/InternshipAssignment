namespace ImageSourcesStorage.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.DataAccessLayer.Models;
    using ImageSourcesStorage.Models;
    using ImageSourcesStorage.Validators;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PinController : ControllerBase
    {
        private readonly IPinRepository pinRepository;
        private readonly IUserRepository userRepository;
        private readonly IBoardRepository boardRepository;
        private readonly IPinBoardRepository pinBoardRepository;
        private readonly GetPinByIdValidator getPinByIdValidator;
        private readonly UploadImageValidator uploadImageValidator;
        private readonly EditPinValidator editPinValidator;
        private readonly IStorage storage;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PinController"/> class.
        /// </summary>
        /// <param name="pinRepository"></param>
        public PinController(IPinRepository pinRepository, IUserRepository userRepository, IStorage storage , IBoardRepository boardRepository, IPinBoardRepository pinBoardRepository, IMapper mapper)
        {
            this.pinRepository = pinRepository;
            this.userRepository = userRepository;
            this.boardRepository = boardRepository;
            this.pinBoardRepository = pinBoardRepository;
            this.storage = storage;
            this.mapper = mapper;
            this.getPinByIdValidator = new GetPinByIdValidator(pinRepository);
            this.editPinValidator = new EditPinValidator(pinRepository, userRepository);
            this.uploadImageValidator = new UploadImageValidator(userRepository, boardRepository,pinRepository , pinBoardRepository);
        }

        /// <summary>
        /// Gets all pins in the system.
        /// </summary>
        /// <returns>200.</returns>
        [HttpGet]
        [Route("api/pins")]
        [ProducesResponseType(typeof(List<Pin>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPinsAsync()
        {
            var result = await this.pinRepository.GetAllPinsAsync();

            List<PinsModel> pins = this.mapper.Map<List<PinsModel>>(result);

            return this.Ok(pins);
        }

        /// <summary>
        /// Gets a pin by id.
        /// </summary>
        /// <param name="pinId"></param>
        /// <returns>200.</returns>
        /// <response code="404"> pinId Not Found.</response>
        [HttpGet]
        [Route("api/pins/{pinId}")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPinByIdAsync(Guid pinId)
        {
            var pin = new Pin() { PinId = pinId };

            var isPinValid = this.getPinByIdValidator.Validate(pin);

            if (!isPinValid.IsValid)
            {
                return this.NotFound();
            }

            var result = await this.pinRepository.GetPinByIdAsync(pinId);
            var response = new GetPinByIdResponse(result);

            return this.Ok(response);
        }

        /// <summary>
        /// Upload an image or add an existing pin.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <param name="boardId"></param>
        /// <returns>404.</returns>
        /// <response code="404">Not Found.</response>
        [HttpPost]
        [Route("api/users/{userId}/boards/{boardId}/pins")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> AddPinToTheBoardAsync([FromForm] UploadImageRequest request, Guid userId, Guid boardId)
        {
            if (request.PinId is null)
            {
                request.PinId = Guid.Empty;
            }

            var addPin = new AddPinToBoard { BoardId = boardId, UserId = userId, PinId = (Guid)request.PinId };

            var result = this.uploadImageValidator.Validate(addPin);

            if (!result.IsValid)
            {
                return this.NotFound();
            }

            if (request.File is null && request.PinId.Equals(Guid.Empty))
            {
                return this.NotFound();
            }

            if (request.PinId.Equals(Guid.Empty))
            {
                var pinId = Guid.NewGuid();
                this.storage.Upload(request.File);
                await this.pinRepository.InsertPinAsync(pinId, userId, request.File.FileName, request.Description);
                await this.pinBoardRepository.InsertPinBoard(boardId, pinId);

                var uploadImageResponse = new UploadImageResponse(pinId);
                return this.Ok(uploadImageResponse);
            }
            else
            {
                await this.pinBoardRepository.InsertPinBoard(boardId, (Guid)request.PinId);
            }

            return this.NoContent();
        }

        /// <summary>
        /// Deletes a board by userid.
        /// </summary>
        /// <param name="pinId"></param>
        /// <returns>204.</returns>
        /// <response code="404"> userId Not Found.</response>
        [HttpDelete]
        [Route("api/pins/{pinId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteBoardOfUserAsync(Guid pinId)
        {
            var pin = new Pin { PinId = pinId };

            var result = this.getPinByIdValidator.Validate(pin);

            if (!result.IsValid)
            {
                return this.NotFound();
            }

            await this.pinRepository.DeletePinAsync(pinId);
            return this.NoContent();
        }

        /// <summary>
        /// Updates a pin name and description.
        /// </summary>
        /// <param name="pinId"></param>
        /// <param name="request"></param>
        /// <returns>204.</returns>
        /// <response code="400"> Bad Request.</response>
        [HttpPut]
        [Route("api/pins/{pinId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> EditPinAsync(Guid pinId, EditPinRequest request)
        {
            var pin = new Pin
            {
                PinId = pinId,
                Name = request.Name,
                Description = request.Description,
            };

            var result = this.editPinValidator.Validate(pin);

            if (!result.IsValid)
            {
                return this.BadRequest();
            }

            await this.pinRepository.EditPinAsync(pinId, request.Description, request.Name);
            return this.NoContent();
        }
    }
}
