using SecurityProject.Common;
using School.Storage;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace School.Services
{
    public class EncryptionServiceRSA
    {
        //static RSAParameters pubkey;
        //static RSAParameters privkey;

        //static EncryptionServiceRSA()
        //{
        //    using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
        //    {
        //        pubkey = rsa.ExportParameters(false);
        //        privkey = rsa.ExportParameters(true);
        //    }
        //}
        public static string RSAEncrypt(string inputText, bool doPadding, string email)
        {
            try
            {
                var users = FileManager.LoadUsers();
                var user = users.FirstOrDefault(u => u.Email == email); 
                if (user != null)
                {
                    byte[] dataToEncrypt = CommonClass.ByteConverter.GetBytes(inputText);

                    using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                    {
                        rsa.ImportParameters(user.PublicKey);
                        byte[] encryptedData = rsa.Encrypt(dataToEncrypt, doPadding);
                        string cipherBase64 = Convert.ToBase64String(encryptedData);
                        FileManager.SaveEncryptedText(email, inputText, cipherBase64); 
                        return cipherBase64; 
                    }
                }
                else
                {
                    Console.WriteLine("User not found.");
                    return null;
                }
            }
            catch (CryptographicException ex)
            {
                Console.WriteLine($"Cryptographic error during encryption: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during encryption: {ex.Message}");
                return null;
            }
        }
        public static List<string> RSADecrypt(string email, bool doPadding)
        {
            var plainTexts = new List<string>();
            var entries = FileManager.LoadAllEncryptedTexts(email);  
            var users = FileManager.LoadUsers();
            var user = users.FirstOrDefault(u => u.Email == email); 
            if (user == null)
            {
                Console.WriteLine("User not found.");
                return plainTexts;  
            }
            foreach (var (_, cipherBase64) in entries)
            {
                try
                {
                    byte[] cipherBytes = Convert.FromBase64String(cipherBase64);

                    using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                    {
                        rsa.ImportParameters(user.PrivateKey);
                        byte[] decryptedBytes = rsa.Decrypt(cipherBytes, doPadding);
                        string plainText = CommonClass.ByteConverter.GetString(decryptedBytes);
                        Console.WriteLine($"Decrypted Text: {plainText}");
                        plainTexts.Add(plainText);
                    }
                }
                catch (CryptographicException ex)
                {
                    Console.WriteLine($"Decryption error: {ex.Message}");
                }
                catch (FormatException ex)
                {
                    Console.WriteLine($"Invalid Base64 format: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error decrypting text: {ex.Message}");
                }
            }

            return plainTexts;
        }
    }
}
