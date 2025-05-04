using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SecurityProject.Common
{
    public static class CommonClass
    {
        public static string registerFile = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "C:\\Users\\hp\\Documents\\VisualStudio\\School\\Storage\\Files\\users.txt"));
        public static string encryptedFile = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "C:\\Users\\hp\\Documents\\VisualStudio\\School\\Storage\\Files\\encrypted_data.txt"));

        public static RSACryptoServiceProvider RSACryptoServiceProvider { get; private set; }
        public static UnicodeEncoding ByteConverter;
        static CommonClass()
        {
            RSACryptoServiceProvider = new RSACryptoServiceProvider();
            ByteConverter= new UnicodeEncoding();
        }
    }
}
