using s4dServer.DTOs.Request;
using s4dServer.DTOs.Response;
using s4dServer.Models;

namespace s4dServer.Services
{
    public interface IUserService
    {
        public Task<List<UserResponseDTO>> GetAllActiveUsers();

        public Task<PagingModel<UserResponseDTO>> SearchAllUsers(int page, int pageSize, string? userName, string? status);

        public Task<bool> CheckExistUserName(string userName);

        public Task<UserResponseDTO> GetUserById(int userId);

        public Task<UserResponseDTO> ChangeStatusUserById(int userId);

        public Task<UserResponseDTO> ChangeUserPasswordById(int userId, string oldPassword, string newPassword);

        public Task<UserResponseDTO> ResetUserPasswordById(int userId, string newPassword);

        public Task<UserResponseDTO?> AddUser(UserRequestDTO userRequestDTO);
        public Task<UserResponseDTO?> UpdateUser(UserRequestDTO restaurantUserRequestDTO);
    }
}
