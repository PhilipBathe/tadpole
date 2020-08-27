namespace Tadpole.IntegrationTests.Data
{
    public interface IRegisterTestData
    {
        void WipeAll();
        int GetCountByEmail(string email);
    }
}
