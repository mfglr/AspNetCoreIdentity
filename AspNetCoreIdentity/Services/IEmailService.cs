namespace AspNetCoreIdentity.Services
{
    public interface IEmailService
    {
        Task SendResetPasswordEmail(string link, string toEmail);
    }
}
