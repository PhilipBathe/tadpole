namespace Tadpole.Web.Services
{
    interface IEncryption
    {
        EncryptionResult Encrypt(string valueToEncrypt);
    }
}
