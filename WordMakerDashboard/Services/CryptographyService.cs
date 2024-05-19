using System.Security.Cryptography;
using System.Text;

namespace WordMakerDashboard.Services
{
    /// <summary>
    /// Class for cryptographing the passwords.
    /// </summary>
    internal class CryptographyService
    {
        /// <summary>
        /// Converts a string to MD5 hash byte.
        /// </summary>
        /// <param name="input">The string to be converted.</param>
        /// <returns>returns the md5 hash for the string</returns>
        public string ConvertToMd5(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }

                return sb.ToString();
            }
        }
    }
}