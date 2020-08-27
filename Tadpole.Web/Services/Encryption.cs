using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;

namespace Tadpole.Web.Services
{
    public class Encryption : IEncryption
    {
        public EncryptionResult Encrypt(string valueToEncrypt)
        {
            //using Microsoft.AspNetCore.Cryptography.KeyDerivation - see https://docs.microsoft.com/en-us/aspnet/core/security/data-protection/consumer-apis/password-hashing?view=aspnetcore-3.1
            
            EncryptionResult result = new EncryptionResult();

            // generate a 128-bit salt using a secure PRNG
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            result.Salt = Convert.ToBase64String(salt);

            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: valueToEncrypt,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            result.Hash = hashed;

            return result;
        }

        public bool Verify(string valueToVerify, string originalHash, string originalSalt)
        {
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: valueToVerify,
                salt: System.Convert.FromBase64String(originalSalt),
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
                ));

            return originalHash == hashedPassword;
        }
    }
}
