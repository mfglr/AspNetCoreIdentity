namespace AspNetCoreIdentity.Configurations
{
    public class EmailServiceSettings : IEmailServiceSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string SenderMail { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
    }
}
