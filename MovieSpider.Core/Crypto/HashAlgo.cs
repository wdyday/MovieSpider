using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace MovieSpider.Core.Crypto
{
    public abstract class HashAlgo
    {
        protected HashAlgorithm _algo;

        public HashAlgo(HashAlgorithm algo)
        {
            _algo = algo;
        }

        public string ComputeHash(string source)
        {
            byte[] bytesSource = Encoding.UTF8.GetBytes(source);
            byte[] bytesHash = _algo.ComputeHash(bytesSource);

            return Convert.ToBase64String(bytesHash);
        }
    }
}
