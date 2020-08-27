namespace Tadpole.Web.Services
{
    public interface IRegister
    {
        void Save(RegistrationUser user);
        bool IsEmailAlreadyRegistered(string email);
    }
}
