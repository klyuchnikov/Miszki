using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace CommonLibrary
{
    public static class RSAEx
    {
        public static BigInteger EnCrypt(BigInteger data, BigInteger D, BigInteger N)
        {
            var newDatas = BigInteger.ModPow(data, D, N);
            return newDatas;
        }

        public static BigInteger DeCrypt(BigInteger data, BigInteger D, BigInteger N)
        {
            var newDatas = BigInteger.ModPow(data, D, N);
            return newDatas;
        }
    }
}
