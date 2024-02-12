using AspNetCoreIdentity.CustomValidators;
using AspNetCoreIdentity.Localization;
using AspNetCoreIdentity.Models;
using AspNetCoreIdentity.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AspNetCoreIdentity.Extentions
{
    public static class ConfigurationExtentions
    {

        public static IServiceCollection AddCustomIdentity(this IServiceCollection services)
        {

            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromHours(2);
            });
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

                        opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
                        opt.Lockout.MaxFailedAccessAttempts = 3;
                    }
                )
                .AddPasswordValidator<PasswordValidator>()
                .AddUserValidator<UserValidator>()
                .AddErrorDescriber<TurkishIdentityErrorDescriber>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }

        public static IServiceCollection AddPolicies(this IServiceCollection services)
        {
            return services
                .AddAuthorization(
                    opt =>
                    {
                        opt.AddPolicy(
                            "BursaPolicy",
                            policy => policy.RequireClaim(ClaimTypes.StateOrProvince, "Bursa")
                        );

                        opt.AddPolicy(
                            "ExpireDateOfFreeAccess",
                            policy => policy.AddRequirements(new ExpireDateRequirement())
                        );

                    }
                )
                .AddScoped<IAuthorizationHandler,ExpireDateRequirementHandler>();
                
        }

    }
}
