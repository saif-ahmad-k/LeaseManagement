using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace WebApplication1
{
    public static class HelperClass
    {
        public static string Encrypt(this string clearText)
        {

            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = System.Text.Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }

            }
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(clearText);
            char[] padding = { '=' };
            string returnValue = System.Convert.ToBase64String(toEncodeAsBytes).TrimEnd(padding).Replace('+', '-').Replace('/', '_');
            return returnValue;
        }
        public static string Decrypt(this string cipherText)
        {

            string incoming = cipherText
.Replace('_', '/').Replace('-', '+');
            switch (cipherText.Length % 4)
            {
                case 2: incoming += "=="; break;
                case 3: incoming += "="; break;
            }
            byte[] bytes = Convert.FromBase64String(incoming);
            string originalText = Encoding.ASCII.GetString(bytes);

            string EncryptionKey = "MAKV2SPBNI99212";
            originalText = originalText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(originalText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    originalText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return originalText;
        }
        public static string ToShortTimeSafe(this TimeSpan timeSpan)
        {
            return new DateTime().Add(timeSpan).ToShortTimeString();
        }

        public static string ToShortTimeSafe(this TimeSpan? timeSpan)
        {
            return timeSpan == null ? string.Empty : timeSpan.Value.ToShortTimeSafe();
        }
    }
}