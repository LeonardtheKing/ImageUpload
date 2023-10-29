using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Extensions
{
  public static class ClaimsPrincipleExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            //The ClaimType.Name is used to get the UniqueName in the TokenService class
            return user.FindFirst(ClaimTypes.Name)?.Value;

        }

        public static Guid GetUserId(this ClaimsPrincipal user)
        {
           return Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
    }
}