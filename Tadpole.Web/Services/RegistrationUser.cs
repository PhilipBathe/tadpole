namespace Tadpole.Web.Services
{
    public class RegistrationUser
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
    }
}
