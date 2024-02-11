namespace AspNetCoreIdentity.Configurations
{
    public interface IEmailServiceSettings
    {
        string Host { get; }
        int Port { get; }
        string SenderMail {  get; }
        string DisplayName { get; }
        string Password { get; }
    }
}
