using System.Security.Claims;

namespace s4dServer.Auth
{
    public class JwtUtilities
    {

        public static string GetUserRole(HttpContext context)
        {
            ClaimsPrincipal user = context.User;
            Claim roleClaim = user?.FindFirst("Role");
            return roleClaim?.Value;
        }

        public static int? GetUserIdFromToken(HttpContext context)
        {
            ClaimsPrincipal user = context.User;
            Claim userIdClaim = user?.FindFirst("UserId");

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }

            return null;
        }

        public static string? GetUserNameFromToken(HttpContext context)
        {
            ClaimsPrincipal user = context.User;
            Claim userNameClaim = user?.FindFirst("Username");

            if (userNameClaim != null)
            {
                return userNameClaim.Value.ToString();
            }

            return null;
        }
    }
}
