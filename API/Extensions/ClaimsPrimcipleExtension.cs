using System.Security.Claims;

namespace API.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            //The ClaimType.Name is used to get the UniqueName in the TokenService class
            return user.FindFirst(ClaimTypes.Name)?.Value;

        }

        public static int GetUserId(this ClaimsPrincipal user)
        {
           return int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
    }
}