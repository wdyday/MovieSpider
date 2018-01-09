using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace MovieSpider.Core.Crypto
{
    public abstract class SymAlgo
    {
        protected SymmetricAlgorithm _algo;

        public SymAlgo(SymmetricAlgorithm algo, byte[] key)
        {
            _algo = algo;
            _algo.Key = key;
        }

        public byte[] Encrypt(byte[] source)
        {
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, _algo.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(source, 0, source.Length);
            cs.FlushFinalBlock();

            byte[] bytesCipher = ms.ToArray();

            byte[] bytesOutput = new byte[_algo.IV.Length + bytesCipher.Length];
            _algo.IV.CopyTo(bytesOutput, 0);
            bytesCipher.CopyTo(bytesOutput, _algo.IV.Length);

            ms.Close();
            cs.Close();

            return bytesOutput;
        }

        public string Encrypt(string source)
        {
            byte[] bytesOutput = Encrypt(Encoding.UTF8.GetBytes(source));
            string cipher = Convert.ToBase64String(bytesOutput);

            return cipher;
        }

        public byte[] Decrypt(byte[] cipher)
        {
            _algo.IV = cipher.Take(_algo.IV.Length).ToArray();

            byte[] bytesCipher = cipher.Skip(_algo.IV.Length).ToArray();
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, _algo.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(bytesCipher, 0, bytesCipher.Length);
            cs.FlushFinalBlock();

            byte[] bytesPlain = ms.ToArray();

            ms.Close();
            cs.Close();

            return bytesPlain;
        }

        public string Decrypt(string cipher)
        {
            byte[] bytesPlain = Decrypt(Convert.FromBase64String(cipher));
            string source = Encoding.UTF8.GetString(bytesPlain);

            return source;
        }
    }
}
