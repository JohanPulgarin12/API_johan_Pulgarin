using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.Security
{
    public class HashResult
    {
        public string Hash { get; set; }
        public byte[] Salt { get; set; }
    }
}
