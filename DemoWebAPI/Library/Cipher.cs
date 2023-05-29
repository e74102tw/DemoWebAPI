using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DemoWebAPI.Library
{
    public static class Cipher
    {
        private static byte[] key = { 0x43, 0x54, 0x43, 0x42, 0x49, 0x52, 0x50, 0x4a, 0x43, 0x54, 0x43, 0x42, 0x49, 0x52, 0x50, 0x4a };
        private static byte[] iv = { 0x4c, 0x44, 0x46, 0x55, 0x55, 0x44, 0x43, 0x41, 0x4c, 0x44, 0x46, 0x55, 0x55, 0x44, 0x43, 0x41 };
        public static string Encrypt(string plain_text)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream memstm = new MemoryStream())
                {
                    using (CryptoStream stm = new CryptoStream(memstm, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter writer = new StreamWriter(stm))
                        {
                            writer.Write(plain_text);
                        }
                    }

                    byte[] encrypted = memstm.ToArray();
                    return Convert.ToBase64String(encrypted);
                }
            }
        }
        public static string Decrypt(string encrypted_text)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                byte[] cipherText = Convert.FromBase64String(encrypted_text);
                using (MemoryStream memstm = new MemoryStream(cipherText))
                {
                    using (CryptoStream stm = new CryptoStream(memstm, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader reader = new StreamReader(stm))
                        {
                            string decrypted = reader.ReadToEnd();
                            return decrypted;
                        }
                    }
                }
            }
        }
    }

}