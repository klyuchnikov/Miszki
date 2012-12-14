using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace CommonLibrary
{
    // Miller-Rabin primality test as an extension method on the BigInteger type.
    // Based on the Ruby implementation on this page.
    public static class BigIntegerExtensions
    {
        public static bool IsProbablePrime(this BigInteger source, int certainty)
        {
            if (source == 2 || source == 3)
                return true;
            if (source < 2 || source % 2 == 0)
                return false;

            BigInteger t = source - 1;
            int s = 0;

            while (t % 2 == 0)
            {
                t /= 2;
                s += 1;
            }

            // There is no built-in method for generating random BigInteger values.
            // Instead, random BigIntegers are constructed from randomly generated
            // byte arrays of the same length as the source.
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            byte[] bytes = new byte[source.ToByteArray().LongLength];
            BigInteger a;

            for (int i = 0; i < certainty; i++)
            {
                do
                {
                    rng.GetBytes(bytes);
                    a = new BigInteger(bytes);
                } while (a < 2 || a >= source - 2);

                BigInteger x = BigInteger.ModPow(a, t, source);
                if (x == 1 || x == source - 1)
                    continue;

                for (int r = 1; r < s; r++)
                {
                    x = BigInteger.ModPow(x, 2, source);
                    if (x == 1)
                        return false;
                    if (x == source - 1)
                        break;
                }

                if (x != source - 1)
                    return false;
            }

            return true;
        }

        public static BigInteger GenPrimeBigInteger(byte length)
        {
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            byte[] bytes = new byte[length];
            BigInteger a;
            int i = 0;
            do
            {
                i++;
                rng.GetBytes(bytes);
                a = new BigInteger(bytes);

            } while (!a.IsProbablePrime(1));
            return a;
        }

        public static BigInteger GenBigInteger(byte length)
        {
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            byte[] bytes = new byte[length];
            BigInteger a;
            do
            {
                rng.GetBytes(bytes);
                a = new BigInteger(bytes);
            } while (a.Sign != 1);
            return a;
        }

        public static int gcd(int a, int b)
        {
            while (b != 0)
                b = a % (a = b);
            return a;
        }

        public static BigInteger gcd(BigInteger a, BigInteger b)
        {
            var aa = new BigInteger();
            aa = BigInteger.Add(aa, a);
            var bb = new BigInteger();
            bb = BigInteger.Add(bb, b);
            while (bb != 0)
            {
                var temp = BigInteger.ModPow(aa, 1, bb);
                aa = bb;
                bb = temp;
            }
            return aa;
        }

        public static BigInteger Pow(BigInteger x, BigInteger y)
        {
            if (y.CompareTo(BigInteger.Zero) < 0)
                throw new ArgumentException();
            BigInteger z = x; // z will successively become x^2, x^4, x^8, x^16, x^32...  
            BigInteger result = BigInteger.One;
            byte[] bytes = y.ToByteArray();
            for (int i = bytes.Length - 1; i >= 0; i--)
            {
                byte bits = bytes[i];
                for (int j = 0; j < 8; j++)
                {
                    if ((bits & 1) != 0)
                        result = BigInteger.Multiply(result, z);
                    // short cut out if there are no more bits to handle:  
                    if ((bits >>= 1) == 0 && i == 0)
                        return result;
                    z = BigInteger.Multiply(z, z);
                }
            }
            return result;
        }

        public static Tuple<BigInteger, BigInteger, BigInteger, BigInteger> GenRSAValues(byte lengthKey)
        {
            var P = BigIntegerExtensions.GenPrimeBigInteger(lengthKey);
            var Q = BigIntegerExtensions.GenPrimeBigInteger(lengthKey);

            var N = BigInteger.Multiply(P, Q);

            var M = BigInteger.Multiply(P - 1, Q - 1);
            var a = 8;
            var b = 19;
            var r = BigIntegerExtensions.gcd(a, b);
            BigInteger D = BigInteger.One;
            BigInteger E = BigInteger.One;
            BigInteger X = BigInteger.One;

            int i = 2000000;
            do
            {
                if (i < 1000000)
                    X = (i++) * M + 1;
                else
                {
                    do
                    {
                        D = BigIntegerExtensions.GenBigInteger((byte)(3));
                    } while (BigIntegerExtensions.gcd(M, D) != 1);
                    i = 2;
                }
            } while (BigInteger.ModPow(X, 1, D) != 0);

            E = X / D;
            return new Tuple<BigInteger, BigInteger, BigInteger, BigInteger>(D, N, E, N);
        }

    }
}
