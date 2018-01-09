using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace MovieSpider.Core.Crypto
{
    public class DESAlgo : SymAlgo
    {
        public DESAlgo(byte[] key)
            : base(new DESCryptoServiceProvider(), key) { }
    }
}
