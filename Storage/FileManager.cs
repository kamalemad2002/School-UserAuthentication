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

        public static void SaveUser(RegisterModel user, RSAParameters publicKey, RSAParameters privateKey)
        {
            try
            {
                string publicKeyXml = RSAParametersToXml(publicKey,false);
                string privateKeyXml = RSAParametersToXml(privateKey,true);

                using (StreamWriter writer = new StreamWriter(SecurityProject.Common.CommonClass.registerFile, append: true))
                {
                    writer.WriteLine($"{user.Email},{user.Password},{publicKeyXml},{privateKeyXml}");
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
                    var parts = line.Split(',');
                    if (parts.Length == 4)
                    {
                        string email = parts[0];
                        string password = parts[1];
                        string publicKeyXml = parts[2];
                        string privateKeyXml = parts[3];

                        RSAParameters publicKey = XmlToRSAParameters(publicKeyXml,false);
                        RSAParameters privateKey = XmlToRSAParameters(privateKeyXml,true);

                        users.Add(new RegisterModel
                        {
                            Email = email,
                            Password = password,
                            PublicKey = publicKey, 
                            PrivateKey = privateKey  
                        });
                    }
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
        
        public static RSAParameters XmlToRSAParameters(string xml, bool includePrivateParameters)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(xml);
                return rsa.ExportParameters(includePrivateParameters);
            }
        }

        public static string RSAParametersToXml(RSAParameters rsaParameters, bool includePrivate)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(rsaParameters);
                return rsa.ToXmlString(includePrivate);
            }
        }
        public static void InitializeFiles()
        {
            if (!File.Exists(CommonClass.registerFile))
                using (FileStream fs = File.Create(CommonClass.registerFile)) { }

            if (!Directory.Exists(CommonClass.encryptedUsersFolder))
                Directory.CreateDirectory(CommonClass.encryptedUsersFolder);
        }

    }
}
