using AspNetCoreIdentity.Models;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentity.CustomValidators
{
    public class UserValidator : IUserValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user)
        {

            var errors = new List<IdentityError>();
            var isNumeric = int.TryParse(user.UserName?[0] + "", out _);

            if (isNumeric)
                errors.Add(
                    new()
                    {
                        Code = "StartingWithNumericCharacter",
                        Description = "Username must not start with a number!"
                    }
                );

            if(errors.Any())
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            return Task.FromResult(IdentityResult.Success);
        }
    }
}
