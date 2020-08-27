namespace Tadpole.Web.Services
{
    interface IEncryption
    {
        EncryptionResult Encrypt(string valueToEncrypt);
        bool Verify(string valueToVerify, string originalHash, string originalSalt);
    }
}
