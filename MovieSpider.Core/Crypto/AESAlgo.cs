using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace MovieSpider.Core.Crypto
{
    public class AESAlgo : SymAlgo
    {
        public AESAlgo(byte[] key)
            : base(new RijndaelManaged(), key) { }
    }
}
