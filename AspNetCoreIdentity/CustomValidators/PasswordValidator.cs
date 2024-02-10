using AspNetCoreIdentity.Models;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentity.CustomValidators
{
    public class PasswordValidator : IPasswordValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string? password)
        {
            var errors = new List<IdentityError>();
            if (string.IsNullOrEmpty(password))
                errors.Add(
                    new()
                    {
                        Code = "NullOrEmptyPassword",
                        Description = "Password is required!"
                    }
                );
            
            if (password!.ToLower().Contains(user.UserName!.ToLower()))
            {
                errors.Add(
                    new()
                    {
                        Code = "PasswordContainsUserName",
                        Description = "Password must not contain user name"
                    }
                );
            }

            if (errors.Any())
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            return Task.FromResult(IdentityResult.Success);
            

        }
    }
}
