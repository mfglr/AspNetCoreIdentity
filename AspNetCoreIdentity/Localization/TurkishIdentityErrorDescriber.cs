using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentity.Localization
{
    public class TurkishIdentityErrorDescriber : IdentityErrorDescriber
    {

        public override IdentityError DuplicateEmail(string email)
        {
            return new()
            {
                Code = "DuplicateEmail",
                Description = $"Email ({email}) baski bir kullanici icin tanimli!"
            };
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new()
            {
                Code = "PasswordTooShort",
                Description = "Sifre cok kisa!"
            };
        }

    }
}
