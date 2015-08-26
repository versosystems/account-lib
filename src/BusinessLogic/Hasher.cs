using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Generic.BusinessLogic.User
{
    /// <summary>
    /// Hasher class used to hash and re-hash strings
    /// </summary>
    /// Sample Usage: 
    /// var hasher = new Hasher ();
    /// var salt = hasher.GetSalt (user.PasswordHash);
    /// var hashedPassword = hasher.Hash (password, salt, HashAlgorithm.Create ("SHA256"));
    public class Hasher
    {
        /// <summary>
        /// Creates a Hash for a new password. Static utility method provided for convienance to the developer.
        /// </summary>
        /// <returns>The hash.</returns>
        /// <param name="passwordToHash">Password to hash.</param>
        public static string CreateHash(string passwordToHash)
        {
            var hasher = new Hasher();
            var hashedPassword =
                hasher.CreateHashUsingDefaultAlgorithm(passwordToHash);
            return hashedPassword;
        }

        /// <summary>
        /// Gets a hash value for an existing password. Static utility method provided for convienance to the developer.
        /// </summary>
        /// <returns>The hash.</returns>
        /// <param name="plainTextPassword">Plain text password.</param>
        /// <param name="existingHashedValue">Existing hashed value.</param>
        public static string GetHash(string plainTextPassword, string existingHashedValue)
        {
            var hasher = new Hasher();
            var salt = hasher.GetSalt(existingHashedValue);
            var hashedPassword =
                hasher.VerifyHashUsingDefaultAlgorithm(plainTextPassword, salt);
            return hashedPassword;
        }

        /// <summary>
        /// The size of the salt value.
        /// </summary>
        public const int SaltValueSize = 4;

        /// <summary>
        /// Hashes a plain text string given the provided hash algorithm.
        /// </summary>
        /// <returns>The hashed string.</returns>
        /// <param name="stringToHash">The string to hash.</param>
        /// <param name="hash">The hash algorithm to use.</param>
        public string Hash(string stringToHash, HashAlgorithm hashAlgorithm)
        {
            return Hash(stringToHash, null, hashAlgorithm);
        }

        /// <summary>
        /// Hashes a plain text string using a default hash algorithm of SHA256 and salt.
        /// If a salt is provided (not null), the hash will use the provided salt in it's 
        /// computation.  This is used to check existing hashes. Used when verifying an existing user account.
        /// </summary>
        /// <returns>The hashed string.</returns>
        /// <param name="stringToHash">The string to hash.</param>
        /// <param name="saltValue">A salt value.</param>
        public string VerifyHashUsingDefaultAlgorithm(string stringToHash, string saltValue)
        {
            HashAlgorithm algorithm = HashAlgorithm.Create("SHA256");
            return Hash(stringToHash, saltValue, algorithm);
        }

        /// <summary>
        /// Hashes a plain text string using a default hash algorithm of SHA256.
        /// This is used to create a new hash. Used when creating a new user account.
        /// </summary>
        /// <returns>The hashed string.</returns>
        /// <param name="stringToHash">The string to hash.</param>
        public string CreateHashUsingDefaultAlgorithm(string stringToHash)
        {
            HashAlgorithm algorithm = HashAlgorithm.Create("SHA256");
            return Hash(stringToHash, algorithm);
        }

        /// <summary>
        /// Hashes a plain text string given the provided hash algorithm and salt.
        /// If a salt is provided (not null), the hash will use the provided salt in it's 
        /// computation.  This is used to check existing hashes.
        /// </summary>
        /// <returns>The hashed string.</returns>
        /// <param name="stringToHash">The string to hash.</param>
        /// <param name="saltValue">A salt value.</param>
        /// <param name="hashAlgorithm">The hash algorithm to use.</param>
        public string Hash(string stringToHash, string saltValue, HashAlgorithm hashAlgorithm)
        {
            return ComputeHash(stringToHash, saltValue, hashAlgorithm);
        }

        /// <summary>
        /// Gets the salt from a given hashed string, assuming the salt is of the same length
        /// as in the constant.
        /// </summary>
        /// <returns>The salt.</returns>
        /// <param name="hashedString">The hashed string.</param>
        public string GetSalt(string hashedString)
        {
            int saltLength = SaltValueSize * UnicodeEncoding.CharSize;
            var salt = hashedString.Substring(0, saltLength);

            return salt;
        }

        /// <summary>
        /// Computes the hash. Given the salt value (if any) and the provided hash algorithm.
        /// </summary>
        /// <returns>The hash string.</returns>
        /// <param name="stringToHash">The string to hash.</param>
        /// <param name="saltValue">A salt value.</param>
        /// <param name="hashAlgorithm">The hash algorithm to use.</param>
        private string ComputeHash(string stringToHash, string saltValue, HashAlgorithm hashAlgorithm)
        {
            UnicodeEncoding encoding = new UnicodeEncoding();
            byte[] hashedBytes;
            byte[] salt = null;
            byte[] dataBuffer;
            byte[] stringToHashBytes;
            StringBuilder builder = new StringBuilder();

            if (saltValue == null)
                salt = GenerateSalt();
            else
                salt = GetSaltFromString(saltValue);

            dataBuffer = new byte[encoding.GetByteCount(stringToHash) + SaltValueSize];
            stringToHashBytes = encoding.GetBytes(stringToHash);

            salt.CopyTo(dataBuffer, 0);
            stringToHashBytes.CopyTo(dataBuffer, SaltValueSize);

            hashedBytes = hashAlgorithm.ComputeHash(dataBuffer);

            foreach (byte outputByte in salt)
                builder.Append(outputByte.ToString("x2").ToUpper());
            foreach (byte outputByte in hashedBytes)
                builder.Append(outputByte.ToString("x2").ToUpper());

            return builder.ToString();
        }

        /// <summary>
        /// Generates the salt byte array.
        /// </summary>
        /// <returns>The salt.</returns>
        private byte[] GenerateSalt()
        {
            byte[] saltBytes = new byte[SaltValueSize];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetNonZeroBytes(saltBytes);

            return saltBytes;
        }

        /// <summary>
        /// Gets the salt byte array from the provided string value.
        /// </summary>
        /// <returns>The salt.</returns>
        /// <param name="saltValue">The string representation of the salt value.</param>
        private byte[] GetSaltFromString(string saltValue)
        {
            byte[] saltBytes = new byte[SaltValueSize];

            saltBytes[0] = byte.Parse(saltValue.Substring(0, 2), System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat);
            saltBytes[1] = byte.Parse(saltValue.Substring(2, 2), System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat);
            saltBytes[2] = byte.Parse(saltValue.Substring(4, 2), System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat);
            saltBytes[3] = byte.Parse(saltValue.Substring(6, 2), System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat);

            return saltBytes;
        }
    }
}
