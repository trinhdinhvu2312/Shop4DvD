using Microsoft.AspNetCore.Mvc;
using s4dServer.DTOs.Request;

namespace s4dServer.Services
{
    public interface IAuthService
    {
        Task<ActionResult> Login(LoginRequestDTO model);
        Task<ActionResult> Register(RegisterRequestDTO model);
    }
}
