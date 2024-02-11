using System.Net.Mail;

namespace AspNetCoreIdentity.CustomMailMessages
{
    public class MailMessageFactory
    {

        private static string resetPasswordMailMessageBodyTemplate =
            @$"
                <h4>reset your password</h4>
                <p>
                    <a href='link'>Click here to reset your password.</a>
                </p>
            ";

        public static MailMessage CreateResetPasswordMailMessage(string from,string to,string link) {
            var body = resetPasswordMailMessageBodyTemplate.Replace("link", link);
            var message = new MailMessage()
            {
                Subject = "Reset Password Link",
                From = new MailAddress(from),
                Body = body,
                IsBodyHtml = true
            };
            message.To.Add(new MailAddress(to));
            return message;
        }

        


    }
}
