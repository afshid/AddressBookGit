using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace AddressBook
{
    public class Helpers
    {
        public static string Encode(string value)
        {
            var hash = System.Security.Cryptography.SHA256.Create();
            var encoder = new System.Text.ASCIIEncoding();
            var combined = encoder.GetBytes(value ?? "");
            return BitConverter.ToString(hash.ComputeHash(combined)).ToLower().Replace("-", "");
        }


        public static string Encrypt_AES256(string Plain_Text, byte[] Key, byte[] IV)
        {

            RijndaelManaged Crypto = null;
            MemoryStream MemStream = null;
            ICryptoTransform Encryptor = null;
            CryptoStream Crypto_Stream = null;
            System.Text.UTF8Encoding UTF = new System.Text.UTF8Encoding();

            System.Text.UTF8Encoding Byte_Transform = new System.Text.UTF8Encoding();

            byte[] PlainBytes = Byte_Transform.GetBytes(Plain_Text);

            try
            {
                Crypto = new RijndaelManaged();
                Crypto.Key = Key;
                Crypto.IV = IV;

                MemStream = new MemoryStream();

                Encryptor = Crypto.CreateEncryptor(Crypto.Key, Crypto.IV);
                Crypto_Stream = new CryptoStream(MemStream, Encryptor, CryptoStreamMode.Write);
                Crypto_Stream.Write(PlainBytes, 0, PlainBytes.Length);
            }
            finally
            {
                if (Crypto != null)
                    Crypto.Clear();
                Crypto_Stream.Close();
            }
            return UTF.GetString(MemStream.ToArray());
        }

        public static string Dencrypt_AES256(byte[] Cipher_Text, byte[] Key, byte[] IV)
        {
            RijndaelManaged Crypto = null;
            MemoryStream MemStream = null;
            ICryptoTransform Decryptor = null;
            CryptoStream Crypto_Stream = null;
            StreamReader Stream_Read = null;
            string Plain_Text;

            try
            {
                Crypto = new RijndaelManaged();
                Crypto.Key = Key;
                Crypto.IV = IV;

                MemStream = new MemoryStream(Cipher_Text);

                Decryptor = Crypto.CreateDecryptor(Crypto.Key, Crypto.IV);
                Crypto_Stream = new CryptoStream(MemStream, Decryptor, CryptoStreamMode.Read);
                Stream_Read = new StreamReader(Crypto_Stream);
                Plain_Text = Stream_Read.ReadToEnd();
            }
            finally
            {
                if (Crypto != null)
                    Crypto.Clear();

                MemStream.Flush();
                MemStream.Close();

            }
            return Plain_Text;
        }
    }
}