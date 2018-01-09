using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Configuration;
using MovieSpider.Core.Configuration;

namespace MovieSpider.Core.Crypto
{
    public static class CryptoUtils
    {
        public static string BytesToString(byte[] bytes)
        {
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                strBuilder.Append(bytes[i] < 16 ? "0" + bytes[i].ToString("x") : bytes[i].ToString("x"));
            }
            return strBuilder.ToString();
        }

        public static byte[] StringToBytes(string str)
        {
            byte[] bytes = new byte[str.Length / 2];
            for (int i = 0; i < str.Length / 2; i++)
            {
                bytes[i] = Convert.ToByte(str.Substring(i * 2, 2), 16);
            }
            return bytes;
        }

        public static byte[] Encrypt(byte[] source)
        {
            return GetSymAlgo().Encrypt(source);
        }

        public static string Encrypt(string source)
        {
            return GetSymAlgo().Encrypt(source);
        }

        public static byte[] Decrypt(byte[] cipher)
        {
            return GetSymAlgo().Decrypt(cipher);
        }

        public static string Decrypt(string cipher)
        {
            return GetSymAlgo().Decrypt(cipher);
        }

        public static string ComputeHash(string source)
        {
            return GetHashAlgo().ComputeHash(source);
        }

        private static SymAlgo GetSymAlgo()
        {
            MachineKeySection config = (MachineKeySection)ConfigurationManager.GetSection("machineKey");
            switch (config.Decryption)
            {
                case "AES":
                    return new AESAlgo(StringToBytes(config.DecryptionKey));
                case "DES":
                    string key = config.DecryptionKey;
                    if (key.Length > 16) key = key.Substring(0, 16);
                    return new DESAlgo(StringToBytes(key));
                default:
                    return new TripleDESAlgo(StringToBytes(config.DecryptionKey));
            }
        }

        private static HashAlgo GetHashAlgo()
        {
            MachineKeySection config = (MachineKeySection)ConfigurationManager.GetSection("machineKey");
            switch (config.Validation)
            {
                case "MD5":
                    return new MD5Algo();
                default:
                    return new SHA1Algo(StringToBytes(config.ValidationKey));
            }
        }
    }
}
