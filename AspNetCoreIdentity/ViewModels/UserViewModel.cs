namespace AspNetCoreIdentity.ViewModels
{
    public class UserViewModel
    {
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone {  get; set; }
    }
}
