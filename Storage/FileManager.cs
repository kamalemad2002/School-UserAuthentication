using School.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecurityProject.Common;
using System.Security.Cryptography;

namespace School.Storage
{
    public static class FileManager
    {

        public static void SaveUser(RegisterModel user)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(SecurityProject.Common.CommonClass.registerFile, append: true))
                {
                    writer.WriteLine($"{user.Email},{user.Password}");
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"File write error: {ex.Message}");
            }
        }


        public static List<RegisterModel> LoadUsers()
        {
            List<RegisterModel> users = new List<RegisterModel>();
            using (StreamReader reader = new StreamReader(CommonClass.registerFile))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(',');  // array of stringssss 
                    users.Add(new RegisterModel { Email = parts[0], Password = parts[1] });
                }
            }

            return users;
        }

        public static void UpdateUserPassword(string email, string newHashedPassword)
        {
            var users = LoadUsers();
            foreach (var user in users)
            {
                if (user.Email == email)
                {
                    user.Password= newHashedPassword;
                    break;
                }
            }
            File.WriteAllLines(CommonClass.registerFile, users.ConvertAll(u => $"{u.Email},{u.Password}"));
        }
        public static void SaveEncryptedText(string email, string plainText, string cipherText)
        {
            try
            {
                string filePath = Path.Combine(CommonClass.encryptedUsersFolder, $"{email}.txt");
                File.AppendAllText(filePath, $"{plainText},{cipherText}\n");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"File write error: {ex.Message}");
            }
        }

        public static List<(string plainText, string cipherText)> LoadAllEncryptedTexts(string email)
        {
            var results = new List<(string, string)>();
            string filePath = Path.Combine(CommonClass.encryptedUsersFolder, $"{email}.txt");

            if (!File.Exists(filePath))
                return results;

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(',');
                    results.Add((parts[0], parts[1]));
                }
            }

            return results;
        }
        public static void SaveUserKey(string email, RSAParameters publicKey, RSAParameters privateKey)
        {
            string filePath = Path.Combine(CommonClass.rsaKeysFolder, $"{email}_key.xml");

            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(privateKey);
                string keyXml = rsa.ToXmlString(true); // include private key
                File.WriteAllText(filePath, keyXml);
            }
        }

        public static (RSAParameters publicKey, RSAParameters privateKey)? LoadUserKey(string email)
        {
            string filePath = Path.Combine(CommonClass.rsaKeysFolder, $"{email}_key.xml");

            if (!File.Exists(filePath)) return null;

            using (var rsa = new RSACryptoServiceProvider())
            {
                string keyXml = File.ReadAllText(filePath);
                rsa.FromXmlString(keyXml);
                return (rsa.ExportParameters(false), rsa.ExportParameters(true));
            }
        }


        public static void InitializeFiles()
        {
            if (!File.Exists(CommonClass.registerFile))
                using (FileStream fs = File.Create(CommonClass.registerFile)) { }

            if (!Directory.Exists(CommonClass.encryptedUsersFolder))
                Directory.CreateDirectory(CommonClass.encryptedUsersFolder);
            if (!Directory.Exists(CommonClass.rsaKeysFolder))
                Directory.CreateDirectory(CommonClass.rsaKeysFolder);

        }

    }
}
