namespace ImageSourcesStorage.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.DataAccessLayer.Models;
    using ImageSourcesStorage.DataAccessLayer.Validators;
    using ImageSourcesStorage.Models;
    using ImageSourcesStorage.Validators;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository<User> userRepository;
        private readonly IBoardRepository boardRepository;
        private readonly IPinRepository pinRepository;
        private readonly CheckUserIdValidator checkUserIdValidator;
        private readonly PostUserValidator postUserValidator;
        private readonly PutUserValidator putUserValidator;
        private readonly ChangeUserScoreValidator changeScoreValidator;
        private readonly GetUserPinsValidator getUserPinsValidator;
        private readonly UploadImageValidator uploadImageValidator;
        private readonly IStorage storage;

        public object ViewBag { get; private set; }

        public UserController(IUserRepository<User> userRepository, IStorage storage, IBoardRepository boardRepository, IPinRepository pinRepository)
        {
            this.userRepository = userRepository;
            this.boardRepository = boardRepository;
            this.pinRepository = pinRepository;
            this.storage = storage;
            this.checkUserIdValidator = new CheckUserIdValidator(userRepository);
            this.postUserValidator = new PostUserValidator(userRepository);
            this.putUserValidator = new PutUserValidator(userRepository);
            this.changeScoreValidator = new ChangeUserScoreValidator(userRepository);
            this.getUserPinsValidator = new GetUserPinsValidator(userRepository);
            this.uploadImageValidator = new UploadImageValidator(userRepository, boardRepository, pinRepository);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersAsync()
        {
            var result = await this.userRepository.GetAllAsync();
            return this.Ok(result);
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> GetUserAsync(Guid userId)
        {
            var user = new User { UserId = userId };
            var result = this.checkUserIdValidator.Validate(user);

            return result.IsValid ? this.Ok(await this.userRepository.GetByIdAsync(userId)) : (ActionResult)this.NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> PostUserAsync(CreateUserRequest request)
        {
            var user = new User
            {
                Name = request.Name,
            };

            var result = this.postUserValidator.Validate(user);

            if (!result.IsValid)
            {
                return this.BadRequest();
            }

            await this.userRepository.InsertAsync(user);
            return this.CreatedAtAction("GetUsers", new { id = user.UserId }, request);
        }

        [HttpPut]
        [Route("{userId}")]
        public async Task<IActionResult> PutUserAsync(UpdateUserRequest request, Guid userId)
        {
            var user = new User
            {
                Name = request.Name,
                Score = request.Score,
                UserId = userId,
            };

            var result = this.putUserValidator.Validate(user);

            if (!result.IsValid)
            {
                return this.BadRequest();
            }

            await this.userRepository.UpdateAsync(user.UserId, user.Name, user.Score);
            return this.NoContent();
        }

        [HttpDelete]
        [Route("{userId}")]
        public async Task<IActionResult> DeleteUserAsync(Guid userId)
        {
            var user = new User { UserId = userId };

            var result = this.checkUserIdValidator.Validate(user);

            if (!result.IsValid)
            {
                return this.NotFound();
            }

            await this.userRepository.DeleteAsync(userId);
            return this.NoContent();
        }

        [HttpPost]
        [Route("{userId}/scores")]
        public async Task<IActionResult> ChangeUserScoreAsync(Guid userId, [Required] ChangeScoreOptions changeScoreOptions)
        {
            var user = new User
            {
                UserId = userId,
            };

            var result = this.changeScoreValidator.Validate(user);

            if (!result.IsValid)
            {
                return this.BadRequest();
            }

            await this.userRepository.ChangeUserScore(user.UserId, changeScoreOptions);

            return this.NoContent();
        }

        [HttpGet]
        [Route("{userId}/pins")]
        public async Task<IActionResult> GetUserPinsAsync(Guid userId)
        {
            var pin = new Pin() { UserId = userId };

            var result = this.getUserPinsValidator.Validate(pin);

            if (!result.IsValid)
            {
                return this.NotFound();
            }

            var pins = await this.userRepository.GetUserPinsAsync(userId);

            var response = new GetUserPinsResponse(pins);

            return this.Ok(response);
        }

        [HttpPost]
        [Route("{userId}/boards/{boardId}/pins")]
        public async Task<IActionResult> UploadFileAsync([FromForm] UploadImageRequest request, Guid userId, Guid boardId)
        {
            if (request.PinId is null)
            {
                request.PinId = Guid.Empty;
            }

            var addPin = new AddPinToBoard { BoardId = boardId, UserId = userId, PinId = (Guid)request.PinId };
            var pinBoard = new PinBoard() { PinId = (Guid)request.PinId, BoardId = boardId };

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
                var pin = new Pin { BoardId = boardId, UserId = userId, ImagePath = request.File.FileName };
                this.storage.Upload(request.File);
                await this.pinRepository.InsertPinAsync(pin);

                var requestpin = await this.pinRepository.GetPinByIdAsync(pin.PinId);
                var uploadImageResponse = new UploadImageResponse(requestpin);
                return this.Ok(uploadImageResponse);
            }
            else
            {
                await this.pinRepository.InsertPinBoard(pinBoard);
            }

            return this.NoContent();
        }
    }
}
