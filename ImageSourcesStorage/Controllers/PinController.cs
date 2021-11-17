namespace ImageSourcesStorage.Controllers
{
    using System;
    using System.Threading.Tasks;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.DataAccessLayer.Models;
    using ImageSourcesStorage.Models;
    using ImageSourcesStorage.Validators;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/pins")]
    [ApiController]
    public class PinController : ControllerBase
    {
        private readonly IPinRepository pinRepository;
        private readonly IUserRepository<User> userRepository;
        private readonly IBoardRepository boardRepository;
        private readonly IPinBoardRepository<PinBoard> pinBoardRepository;
        private readonly GetPinByIdValidator getPinByIdValidator;
        private readonly UploadImageValidator uploadImageValidator;
        private readonly IStorage storage;

        /// <summary>
        /// Initializes a new instance of the <see cref="PinController"/> class.
        /// </summary>
        /// <param name="pinRepository"></param>
        public PinController(IPinRepository pinRepository, IUserRepository<User> userRepository, IStorage storage , IBoardRepository boardRepository, IPinBoardRepository<PinBoard> pinBoardRepository)
        {
            this.pinRepository = pinRepository;
            this.userRepository = userRepository;
            this.boardRepository = boardRepository;
            this.pinBoardRepository = pinBoardRepository;
            this.storage = storage;
            this.getPinByIdValidator = new GetPinByIdValidator(pinRepository);
            this.uploadImageValidator = new UploadImageValidator(userRepository, boardRepository,pinRepository , pinBoardRepository);
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

        [HttpPost]
        [Route("api/users/{userId}/boards/{boardId}/pins")]
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
                return this.BadRequest();
            }

            if (request.File is null && request.PinId.Equals(Guid.Empty))
            {
                return this.BadRequest();
            }

            if (request.PinId.Equals(Guid.Empty))
            {
                var pinId = Guid.NewGuid();
                this.storage.Upload(request.File);
                await this.pinRepository.InsertPinAsync(pinId, userId, request.File.FileName);
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
    }
}
