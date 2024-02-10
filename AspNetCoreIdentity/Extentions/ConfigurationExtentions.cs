using AspNetCoreIdentity.Models;

namespace AspNetCoreIdentity.Extentions
{
    public static class ConfigurationExtentions
    {

        public static IServiceCollection AddCustomIdentity(this IServiceCollection services)
        {
            services
                .AddIdentity<AppUser, AppRole>(
                    opt =>
                    {
                        opt.User.RequireUniqueEmail = true;
                        opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz1234567890_";
                        opt.Password.RequiredLength = 9;
                        opt.Password.RequireLowercase = true;
                        opt.Password.RequireUppercase = true;
                        opt.Password.RequireNonAlphanumeric = true;
                        opt.Password.RequireDigit = false;
                    }
                )
                .AddEntityFrameworkStores<AppDbContext>();
            return services;
        }

    }
}
