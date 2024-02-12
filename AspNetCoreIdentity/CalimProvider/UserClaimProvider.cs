using AspNetCoreIdentity.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AspNetCoreIdentity.CalimProvider
{
    public class UserClaimProvider : IClaimsTransformation
    {

        private readonly UserManager<AppUser> _userManager;

        public UserClaimProvider(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {

            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            AppUser? user;
            
            if (
                userId == null ||
                (user = await _userManager.FindByIdAsync(userId)) == null ||
                user.City == null ||
                principal.HasClaim(x => x.Type == ClaimTypes.StateOrProvince)
            ) return principal;

            var claimsIdentity = new ClaimsIdentity(
                principal.Identity,
                new List<Claim>() { new Claim(ClaimTypes.StateOrProvince, user.City) }
            );

            var a = new ClaimsPrincipal(claimsIdentity);

            return a;
        }
    }
}
