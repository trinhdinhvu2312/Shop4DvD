using AutoMapper;
using Microsoft.EntityFrameworkCore;
using s4dServer.DTOs.Request;
using s4dServer.DTOs.Response;
using s4dServer.Models;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace s4dServer.Services.ServiceImpl
{
    public class UserServiceImpl : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly S4DContext _context;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserServiceImpl(IConfiguration configuration, S4DContext context, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            var mapconfig = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
            this.mapper = mapconfig.CreateMapper();
        }

        public async Task<UserResponseDTO?> AddUser(UserRequestDTO userRequestDTO)
        {
            var response = new UserResponseDTO();
            try
            {
                if (HasSpaces(userRequestDTO.Username, userRequestDTO.Email, userRequestDTO.PhoneNumber))
                {
                    response = null;
                    return response;
                }

                var username = userRequestDTO.Username ?? "".ToLower();
                if (await UsernameExists(username))
                {
                    response = null;
                    return response ;
                }

                var user = new User
                {
                    Username = username,
                    Password = HashPassword(userRequestDTO.Password ?? ""),
                    Email = userRequestDTO.Email,
                    Role = userRequestDTO.Role,
                    Status = userRequestDTO.Status,
                    Address = userRequestDTO.Address,
                    PhoneNumber = userRequestDTO.PhoneNumber
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                response.UserID = user.UserID;
                response.Username = userRequestDTO.Username;
                response.Status = userRequestDTO.Status;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return response;
        }


        private async Task<bool> UsernameExists(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username.Equals(username));
        }

        public async Task<UserResponseDTO> ChangeStatusUserById(int userId)
        {
            var userResponseDTO = new UserResponseDTO();
            try
            {
                var identity = (ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity;
                var usernameClaim = identity.FindFirst("Username");

                var modifier = usernameClaim?.Value;

                var existedUser = await _context.Users.FindAsync(userId);

                if (existedUser != null)
                {
                    if (existedUser.Status == 1)
                    {
                        existedUser.Status = 0;
                    }
                    else
                    {
                        existedUser.Status = 1;
                    }

                    await _context.SaveChangesAsync();

                    userResponseDTO.UserID = existedUser.UserID;
                    userResponseDTO.Username = existedUser.Username;
                    userResponseDTO.Status = existedUser.Status;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return userResponseDTO;
        }

        public async Task<UserResponseDTO> ChangeUserPasswordById(int userId, string oldPassword, string newPassword)
        {
            var userResponseDTO = new UserResponseDTO();
            try
            {
                var identity = (ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity;
                var usernameClaim = identity.FindFirst("Username");


                var modifier = usernameClaim?.Value;

                var existedUser = await _context.Users.FindAsync(userId);

                if (existedUser != null)
                {
                    if (existedUser.Password != HashPassword(oldPassword))
                    {
                        throw new Exception("Old password does not match the existing password.");
                    }

                    if (newPassword == oldPassword)
                    {
                        throw new Exception("New password cannot be the same as the old password.");
                    }

                    existedUser.Password = HashPassword(newPassword);
                    await _context.SaveChangesAsync();

                    userResponseDTO.UserID = existedUser.UserID;
                    userResponseDTO.Username = existedUser.Username;
                    userResponseDTO.Status = existedUser.Status;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return userResponseDTO;
        }


        public async Task<List<UserResponseDTO>> GetAllActiveUsers()
        {
            try
            {
                var users = await _context.Users
                    .Where(rt => rt.Status == 1)
                    .ToListAsync(); // Use ToListAsync to fetch data asynchronously

                var userResponseDTOs = users.Select(user => mapUserToDTO(user)).ToList();
                return userResponseDTOs;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        public async Task<UserResponseDTO> GetUserById(int userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    throw new Exception("User not found.");
                }

                var userResponseDTO = mapUserToDTO(user);
                return userResponseDTO;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<UserResponseDTO> ResetUserPasswordById(int userId, string newPassword)
        {
            try
            {
                var existedUser = await _context.Users.FindAsync(userId);
                if (existedUser == null)
                {
                    throw new Exception("User not found.");
                }

                existedUser.Password = HashPassword(newPassword);
                await _context.SaveChangesAsync();

                var userResponseDTO = mapUserToDTO(existedUser);
                return userResponseDTO;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<PagingModel<UserResponseDTO>> SearchAllUsers(int page, int pageSize, string? name, string? status)
        {
            var pagingModel = new PagingModel<UserResponseDTO>();
            try
            {
                IQueryable<User> query = _context.Users;
                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(u => u.Username.ToLower().Contains(name.ToLower()));
                }

                if (!string.IsNullOrEmpty(status))
                {
                    if (status != "All")
                    {
                        if (status == "Active")
                        {
                            query = query.Where(u => u.Status == 1);
                        }
                        else
                        {
                            query = query.Where(u => u.Status == 0);
                        }
                    }
                }

                int totalCount = await query.CountAsync();

                var users = await query
                    .OrderByDescending(u => u.UserID)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var userDTO = users.Select(user =>
                {
                    var dto = mapper.Map<UserResponseDTO>(user);
                    return dto;
                }).ToList();

                pagingModel.Data = userDTO;
                pagingModel.TotalCount = totalCount;
                pagingModel.TotalPage = (int)Math.Ceiling((double)totalCount / pageSize);

                return pagingModel;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<UserResponseDTO?> UpdateUser(UserRequestDTO userRequestDTO)
        {
            try
            {
                var user = await _context.Users.FindAsync(userRequestDTO.UserId);
                if (user == null)
                {
                    throw new Exception("User not found.");
                }

                if (HasSpaces(userRequestDTO.Username, userRequestDTO.Email, userRequestDTO.PhoneNumber))
                {
                    throw new Exception("Invalid input. User fields cannot have spaces.");
                }

                user.Username = userRequestDTO.Username ?? user.Username;
                user.Email = userRequestDTO.Email ?? user.Email;
                user.Role = userRequestDTO.Role;
                user.Status = userRequestDTO.Status;
                user.Address = userRequestDTO.Address;
                user.PhoneNumber = userRequestDTO.PhoneNumber;

                await _context.SaveChangesAsync();

                var userResponseDTO = mapUserToDTO(user);
                return userResponseDTO;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> CheckExistUserName(string name)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username.ToLower().Trim() == name.ToLower().Trim());
                return user != null;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private bool HasSpaces(params string?[] values)
        {
            foreach (var value in values)
            {
                if (!string.IsNullOrEmpty(value) && value.Contains(" "))
                {
                    return true;
                }
            }
            return false;
        }


        public UserResponseDTO mapUserToDTO(User user)
        {
            UserResponseDTO userDTO = new UserResponseDTO();
            return mapper.Map(user, userDTO);
        }

        private string HashPassword(string password)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_configuration["HashPwd:Secret"]));
            var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(passwordHash);
        }
    }
}
