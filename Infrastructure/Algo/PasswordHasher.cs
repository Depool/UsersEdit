using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Infrastructure.Algo
{
    public static class PasswordHasher
    {
        private const int saltSize = 0;

        public static Passphrase Hash(string password)
        {
            if (password == null)
                throw new ArgumentNullException("Password to hash must be non-null");

            byte[] passwordBytes = Encoding.Unicode.GetBytes(password);
            byte[] saltBytes = CreateRandomSalt();
            string hashedPassword = ComputeHash(passwordBytes, saltBytes);

            return new Passphrase
            {
                Hash = hashedPassword,
                Salt = Convert.ToBase64String(saltBytes)
            };
        }


        public static bool Equals(string password, string salt, string hash)
        {
            return String.CompareOrdinal(hash, Hash(password, salt)) == 0;
        }

        public static string GenerateRandomSalt(int size = saltSize)
        {
            return Convert.ToBase64String(CreateRandomSalt(size));
        }

        private static string ComputeHash(byte[] password, byte[] salt)
        {
            var passwordAndSalt = new byte[password.Length + salt.Length];

            Buffer.BlockCopy(salt, 0, passwordAndSalt, 0, salt.Length);
            Buffer.BlockCopy(password, 0, passwordAndSalt, salt.Length, password.Length);

            byte[] computedHash;

            using(HashAlgorithm algorithm = new SHA256Managed())
            {
                computedHash = algorithm.ComputeHash(passwordAndSalt);
            }

            return Convert.ToBase64String(computedHash);
        }


        private static string Hash(string password, string salt)
        {
            return ComputeHash(Encoding.Unicode.GetBytes(password), Convert.FromBase64String(salt));
        }

        private static byte[] CreateRandomSalt(int size = saltSize)
        {
            if (size < 0)
                throw new ArgumentException("Size must be not less than zero");
            var saltBytes = new byte[size];

            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            return saltBytes;
        }
    }

    public class Passphrase
    {
        public string Hash { get; set; }
        public string Salt { get; set; }
    }
}