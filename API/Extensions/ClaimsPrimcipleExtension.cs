using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Extensions
{
    public static class ClaimsPrimcipleExtension
    {
        // public static string GetUsername(this ClaimsPrincipal user)
        // {
        //     return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        // }

        public static string GetUsername(this ClaimsPrincipal user)
{
    // Attempt to find the claim with the name identifier.
    Claim nameIdentifierClaim = user.FindFirst(ClaimTypes.NameIdentifier);

    // Check if the claim exists and has a non-null or non-empty value.
    if (nameIdentifierClaim != null && !string.IsNullOrEmpty(nameIdentifierClaim.Value))
    {
        return nameIdentifierClaim.Value;
    }

    // Handle the case where the claim is not found or has no value.
    // You can return a default value or throw an exception as needed.
    // return "Unknown"; 
      throw new Exception("Username claim not found.");
}
    }
}