using School.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecurityProject.Common;

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
                    var parts = line.Split(',');   
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
                File.AppendAllText(CommonClass.encryptedFile, $"{email},{plainText},{cipherText}\n");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"File write error: {ex.Message}");
            }
        }

        public static List<(string plainText, string cipherText)> LoadAllEncryptedTexts(string email)
        {
            var results = new List<(string, string)>();

            using (StreamReader reader = new StreamReader(CommonClass.encryptedFile))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(',');
                    results.Add((parts[1], parts[2]));
                }
            }

            return results;
        }


        public static void InitializeFiles()
        {
            if (!File.Exists(CommonClass.registerFile))
                using (FileStream fs = File.Create(CommonClass.registerFile)) { }

            if (!File.Exists(CommonClass.encryptedFile))
                using (FileStream fs = File.Create(CommonClass.encryptedFile)) { }
        }

    }
}
