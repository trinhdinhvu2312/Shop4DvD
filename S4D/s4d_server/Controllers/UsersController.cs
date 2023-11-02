using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using s4dServer.DTOs.Request;
using s4dServer.DTOs.Response;
using s4dServer.Models;
using s4dServer.Services;

namespace s4dServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IAuthService _authService;

        public UsersController(IUserService userService, IAuthService authService)
        {
            this.userService = userService;
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public Task<ActionResult> Login([FromBody] LoginRequestDTO model)
        {
            return _authService.Login(model);
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public Task<ActionResult> Register([FromBody] RegisterRequestDTO model)
        {
            return _authService.Register(model);
        }

        // GET: api/Users
        [HttpGet("[action]")]
        public async Task<ActionResult<ServiceResponse<List<UserResponseDTO>>>> GetUsers()
        {
            var response = new ServiceResponse<List<UserResponseDTO>>();
            var users = await userService.GetAllActiveUsers();

            if (users == null)
            {
                response.Status = false;
                response.ErrorCode = 400;
                response.Message = "Cannot load users!";
            }

            if (response.Status == false)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = response.ErrorCode,
                    Title = response.Message
                });
            }

            response.Data = users;

            return response;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<ServiceResponse<PagingModel<UserResponseDTO>>>> SearchAllUsers(int page, int pageSize, string? name, string? status)
        {
            var response = new ServiceResponse<PagingModel<UserResponseDTO>>();
            var User = await userService.SearchAllUsers(page + 1, pageSize, name, status);

            if (User == null)
            {
                response.Status = false;
                response.ErrorCode = 400;
                response.Message = "Cannot load users!";
            }

            if (response.Status == false)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = response.ErrorCode,
                    Title = response.Message
                });
            }

            response.Data = User;

            return response;
        }

        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<UserResponseDTO>>> AddUser([FromBody] UserRequestDTO UserRequestDTO)
        {
            var response = new ServiceResponse<UserResponseDTO>();
            var User = await userService.AddUser(UserRequestDTO);

            if (User == null)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = 500,
                    Title = "Add User fail!"
                });
            }
            response.Data = User;
            return response;
        }

        [HttpPut("[action]")]
        public async Task<ActionResult<ServiceResponse<UserResponseDTO>>> ChangeUserPasswordById(int userId, string oldPwd, string newPwd)
        {
            var response = new ServiceResponse<UserResponseDTO>();
            var User = await userService.ChangeUserPasswordById(userId, oldPwd, newPwd);

            if (User == null)
            {
                return NotFound(new ProblemDetails
                {
                    Status = 500,
                    Title = "Change User Password fail!"
                });
            }
            response.Data = User;
            return response;
        }

        [HttpPut("[action]")]
        public async Task<ActionResult<ServiceResponse<UserResponseDTO>>> ResetUserPasswordById(int userId, string newPwd)
        {
            var response = new ServiceResponse<UserResponseDTO>();
            var User = await userService.ResetUserPasswordById(userId, newPwd);

            if (User == null)
            {
                return NotFound(new ProblemDetails
                {
                    Status = 500,
                    Title = "Reset User Password fail!"
                });
            }
            response.Data = User;
            return response;
        }

        [HttpPut("[action]")]
        public async Task<ActionResult<ServiceResponse<UserResponseDTO>>> UpdateUser([FromBody] UserRequestDTO UserRequestDTO)
        {
            var response = new ServiceResponse<UserResponseDTO>();
            var User = await userService.UpdateUser(UserRequestDTO);

            if (User == null)
            {
                return NotFound(new ProblemDetails
                {
                    Status = 500,
                    Title = "Update User fail!"
                });
            }
            response.Data = User;
            return response;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<ServiceResponse<UserResponseDTO>>> GetUserById(int userId)
        {
            var response = new ServiceResponse<UserResponseDTO>();
            var User = await userService.GetUserById(userId);

            if (User == null)
            {
                return NotFound(new ProblemDetails
                {
                    Status = 500,
                    Title = "User does not exist!"
                });
            }

            response.Status = true;
            response.ErrorCode = 200;
            response.Message = User.UserID != 0 ? "This User is existed" : "This User does not existed";
            response.Data = User;

            return response;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<ServiceResponse<bool>>> CheckExistUserName(string name)
        {
            var response = new ServiceResponse<bool>();
            bool isExist = await userService.CheckExistUserName(name);

            response.Status = true;
            response.ErrorCode = 200;
            response.Message = isExist == true ? "This User Name is existed" : "This User Name does not existed";
            response.Data = isExist;

            return response;
        }

        [HttpDelete("[action]")]
        public async Task<ActionResult<ServiceResponse<UserResponseDTO>>> ChangeStatusUserById(int userId)
        {
            var response = new ServiceResponse<UserResponseDTO>();
            var User = await userService.ChangeStatusUserById(userId);
            if (User == null)
            {
                return NotFound(new ProblemDetails
                {
                    Status = 500,
                    Title = "User does not exist!"
                });
            }
            response.Status = true;
            response.ErrorCode = 200;
            response.Message = "Delete User successfully!";
            response.Data = User;
            return response;
        }
    }
}
