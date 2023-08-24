using Microsoft.AspNetCore.Mvc;
using MongoApi.DTO;
using MongoApi.Models;
using MongoApi.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MongoApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserDataController : ControllerBase
    {
        private readonly IUserDataService _user;
        private readonly IUserDataValidationService _service;

        public UserDataController(IUserDataService user, IUserDataValidationService service)
        {
            _user = user;
            _service = service;
        }

        // GET: api/<UserDataController>
        [HttpGet]
        public async Task<ActionResult<List<UserData>>> GetUser()
        {
            var user = await _user.GetUser();
            return Ok(user);
        }

        // GET api/<UserDataController>/id
        [HttpPost("id")]
        public async Task<ActionResult<UserData>> GetUserById(GetUserByIdDto dto)
        {
            var validationErrors = _service.ValidateGetUserByIdDto(dto);

            if (validationErrors.Count > 0)
                return BadRequest(validationErrors);

            if (!_service.IsIdPasswordValid(dto.Id, dto.Password))
                return Unauthorized();

            var user = await _user.GetUserById(dto.Id, dto.Password);

            if (user == null)
                return NotFound($"User With Id : {dto.Id} Not Found ");
            else if (user.Status == "Deactivated")
                return NotFound($"User With Id : {dto.Id} Not Found ");
            else
                return Ok(user);
        }

        // GET api/<UserDataController>/username
        [HttpPost("username")]
        public async Task<ActionResult<UserData>> GetUserByUserName(GetUserByUserNameDto dto)
        {
            var validationErrors = _service.ValidateGetUserByUserNameDto(dto);

            if (validationErrors.Count > 0)
                return BadRequest(validationErrors);

            if (!_service.IsUserNamePasswordValid(dto.UserName, dto.Password))
                return Unauthorized();

            var user = await _user.GetUserByUserName(dto.UserName, dto.Password);

            if (user == null)
                return NotFound($"User With UserName : {dto.UserName} Not Found ");
            else if (user.Status == "Deactivated")
                return NotFound($"User With UserName : {dto.UserName} Not Found ");
            else
                return Ok(user);
        }

        // GET api/<UserDataController>/email
        [HttpPost("email")]
        public async Task<ActionResult<UserData>> GetUserByEmail(GetUserByEmailDto dto)
        {
            var validationErrors = _service.ValidateGetUserByEmailDto(dto);

            if (validationErrors.Count > 0)
                return BadRequest(validationErrors);

            if (!_service.IsEmailPasswordValid(dto.Email, dto.Password))
                return Unauthorized();
            var user = await _user.GetUserByEmail(dto.Email, dto.Password);

            if (user == null)
                return NotFound($"User With Email : {dto.Email} Not Found ");
            else if (user.Status == "Deactivated")
                return NotFound($"User With Email : {dto.Email} Not Found ");
            else
                return Ok(user);
        }

        // POST api/<UserDataController>
        [HttpPost]
        public async Task<ActionResult<UserData>> CreateUser([FromBody] UserData dto)
        {
            if (await _service.DoesUsernameExist(dto.UserName))
            {
                return Conflict("Username already exists.");
            }

            if (await _service.DoesEmailExist(dto.Email))
            {
                return Conflict("Email already exists.");
            }

            if (dto.Status is null)
                dto.Status = "Activated";
            var user = await _user.CreateUser(dto);
            if (user is null)
            {
                return BadRequest("User Not Found");
            }
            return Ok(user);
        }

        // PUT api/<UserDataController>/5
        [HttpPut]
        public async Task<ActionResult<UserData>> UpdateUser([FromBody] UserData dto)
        {
            if (await _service.DoesUsernameExist(dto.UserName))
            {
                return Conflict("Username already exists.");
            }

            if (await _service.DoesEmailExist(dto.Email))
            {
                return Conflict("Email already exists.");
            }

            var user = await _user.UpdateUser(dto.Id, dto);
            if (user == null)
                return BadRequest();
            return Ok("User Updated Successfully");
        }

        [HttpPut]
        public async Task<ActionResult<UserData>> UpdateUserPassword(UpdateUserPasswordDto dto)
        {
            if (!_service.IsIdPasswordValid(dto.Id, dto.Curr_Password))
                return Unauthorized();
            var user = await _user.UpdateUserPassword(dto.Id, dto.New_Password);
            if (user == null)
                return BadRequest();
            return Ok("User Password Updated Successfully");
        }

        [HttpPut]
        public async Task<ActionResult<UserData>> DeactivateUser(DeactivateUserDto dto)
        {
            if (!_service.IsIdPasswordValid(dto.Id, dto.Password))
                return Unauthorized();
            var user = await _user.DeactivateUser(dto.Id, "Deactivated");
            if (user == null)
                return BadRequest();
            return Ok("User Deactivated Successfully");
        }

        // DELETE api/<UserDataController>/5
        [HttpDelete]
        public async Task<ActionResult> DeleteUser(DeleteUserDto dto)
        {
            if (!_service.IsEmailPasswordValid(dto.Id, dto.Password))
                return Unauthorized();
            bool? msg = await _user.DeleteUser(dto.Id, dto.Password);
            if (msg == true)
                return Ok("User Deleted Successfully");
            else
                return BadRequest();
        }
    }
}
