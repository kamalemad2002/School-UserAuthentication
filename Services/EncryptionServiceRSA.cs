using SecurityProject.Common;
using School.Storage;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace School.Services
{
    public class EncryptionServiceRSA
    {
        
         static RSAParameters pubkey;
         static RSAParameters privkey;
        static EncryptionServiceRSA()
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                pubkey = rsa.ExportParameters(false);
                privkey = rsa.ExportParameters(true);
            }
        }

        public static string RSAEncrypt(string inputText, bool doPadding, string email)
        {
            try
            {
                if (string.IsNullOrEmpty(inputText))
                    return null;

                var keyPair = FileManager.LoadUserKey(email);
                if (keyPair == null)
                {

                    EncryptionServiceRSA.GenerateAndSaveKeys(email);
                    keyPair = FileManager.LoadUserKey(email);
                    if(keyPair== null)
                    {
                        Console.WriteLine("Failed to generate keys.");
                        return null;
                    }
                }

                var pubkey = keyPair.Value.publicKey;
                byte[] dataToEncrypt = CommonClass.ByteConverter.GetBytes(inputText);

                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    rsa.ImportParameters(pubkey);
                    byte[] encryptedData = rsa.Encrypt(dataToEncrypt, doPadding);
                    string cipherBase64 = Convert.ToBase64String(encryptedData);
                    FileManager.SaveEncryptedText(email, inputText, cipherBase64);
                    return cipherBase64;
                }
            }
            catch (CryptographicException)
            {
                return null;
            }
        }


        public static List<string> RSADecrypt(string email, bool doPadding)
        {
            var plainTexts = new List<string>();
            var entries = FileManager.LoadAllEncryptedTexts(email);

            var keyPair = FileManager.LoadUserKey(email);
            if (keyPair == null)
            {
                Console.WriteLine("No key found for user.");
                return plainTexts;
            }

            var privkey = keyPair.Value.privateKey;

            foreach (var (_, cipherBase64) in entries)
            {
                try
                {
                    byte[] cipherBytes = Convert.FromBase64String(cipherBase64);

                    using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                    {
                        rsa.ImportParameters(privkey);
                        byte[] decryptedBytes = rsa.Decrypt(cipherBytes, doPadding);
                        string plainText = CommonClass.ByteConverter.GetString(decryptedBytes);
                        Console.WriteLine($"Decrypted Text: {plainText}");
                        plainTexts.Add(plainText);
                    }
                }
                catch (Exception)
                {
                    // Handle or log decryption failure
                }
            }

            return plainTexts;
        }


        public static void GenerateAndSaveKeys(string email)
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                RSAParameters pubkey = rsa.ExportParameters(false);
                RSAParameters privkey = rsa.ExportParameters(true);
                FileManager.SaveUserKey(email, pubkey, privkey);
            }
        }

    }
}
