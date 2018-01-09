using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace MovieSpider.Core.Crypto
{
    public class SHA1Algo : HashAlgo
    {
        public SHA1Algo(byte[] key)
            : base(new HMACSHA1(key)) { }
    }
}
