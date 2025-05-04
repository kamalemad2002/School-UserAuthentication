using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace School.Helpers
{
    public class HashedPasswordSHA256
    {
        static UnicodeEncoding ByteConverter = new UnicodeEncoding();

        public static string HashPassword(string password)
        {
            if (password == null)
                throw new ArgumentNullException(nameof(password), "Password cannot be null.");

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(ByteConverter.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

    }
}
