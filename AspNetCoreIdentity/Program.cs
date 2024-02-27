using AspNetCoreIdentity.CalimProvider;
using AspNetCoreIdentity.Configurations;
using AspNetCoreIdentity.Extentions;
using AspNetCoreIdentity.Models;
using AspNetCoreIdentity.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<EmailServiceSettings>(builder.Configuration.GetSection("EmailServiceSettings"));
builder.Services.AddSingleton<IEmailServiceSettings>(
    sp => sp.GetRequiredService<IOptions<EmailServiceSettings>>().Value
);
builder.Services.AddSingleton(
    sp => {
        var settings = sp.GetRequiredService<IEmailServiceSettings>();
        return new SmtpClient()
        {
            Host = settings.Host,
            Port = settings.Port,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(settings.SenderMail, settings.Password),
            EnableSsl = true
        };
    }
);
builder.Services.AddSingleton<IEmailService, EmailService>();

builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));

builder.Services.AddScoped<IClaimsTransformation, UserClaimProvider>();

builder.Services.AddControllersWithViews();
builder.Services
    .AddDbContext<AppDbContext>(
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"))
    )
    .AddCustomIdentity();

builder.Services.AddAuthentication().AddFacebook(facebookOptions =>
 {
     facebookOptions.AppId = builder.Configuration["Authentication:Facebook:AppId"]!;
     facebookOptions.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"]!;
 });

builder.Services.ConfigureApplicationCookie(
    opt =>
    {

        opt.Cookie = new CookieBuilder()
        {
            Name = "IdentityCookie"
        };
        opt.LoginPath = "/Home/SignIn";
        opt.LogoutPath = "/Member/Logout";
        opt.AccessDeniedPath = "/Member/AccessDenied";
        opt.ExpireTimeSpan = TimeSpan.FromDays(60);
        opt.SlidingExpiration = true;
        
    }
);

builder.Services.AddPolicies();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();



app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
