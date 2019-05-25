using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace TsinghuaNet
{
    /// <summary>
    /// Methods of cryptography.
    /// </summary>
    internal static class CryptographyHelper
    {
        /// <summary>
        /// Get hex string of a byte array.
        /// </summary>
        /// <param name="data">A <see cref="byte"/> array contains data.</param>
        /// <returns>A hex string.</returns>
        private static string GetHexString(byte[] data)
        {
            StringBuilder sBuilder = new StringBuilder(data.Length * 2);
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
        /// <summary>
        /// Get MD5 encoded string created by <see cref="MD5"/>.
        /// </summary>
        /// <param name="input">Original string.</param>
        /// <returns>An encoded string.</returns>
        /// <remarks>The <paramref name="input"/> string is encoded into <see cref="byte"/> array by UTF-8.</remarks>
        public static string GetMD5(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                return GetHexString(data);
            }
        }
        /// <summary>
        /// Get MD5 encoded string created by <see cref="SHA1"/>.
        /// </summary>
        /// <param name="input">Original string.</param>
        /// <returns>An encoded string.</returns>
        /// <remarks>The <paramref name="input"/> string is encoded into <see cref="byte"/> array by UTF-8.</remarks>
        public static string GetSHA1(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] data = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                return GetHexString(data);
            }
        }
        /// <summary>
        /// Encode a <see cref="string"/> to its UTF-8 form.
        /// </summary>
        /// <param name="a">String to be encoded.</param>
        /// <param name="b">Whether to add the length of the string in the end.</param>
        /// <returns>A <see cref="uint"/> array contains encoded string.</returns>
        private static uint[] S(byte[] a, bool b)
        {
            int c = a.Length;
            int n = (c + 3) / 4;
            uint[] v;
            if (b)
            {
                v = new uint[n + 1];
                v[n] = (uint)c;
            }
            else
            {
                v = new uint[Math.Max(n, 4)];
            }
            Unsafe.CopyBlock(ref Unsafe.As<uint, byte>(ref v[0]), ref a[0], (uint)c);
            return v;
        }
        /// <summary>
        /// Decode a <see cref="string"/> from its UTF-8 form.
        /// </summary>
        /// <param name="a">A <see cref="uint"/> array contains the encoded string.</param>
        /// <param name="b">Whether the length of the original string is in the end.</param>
        /// <returns>Decoded string.</returns>
        private static byte[] L(uint[] a, bool b)
        {
            int d = a.Length;
            uint c = ((uint)(d - 1)) << 2;
            if (b)
            {
                uint m = a[d - 1];
                if (m < c - 3 || m > c)
                {
                    return Array.Empty<byte>();
                }
                c = m;
            }
            uint n = b ? c : (uint)(d << 2);
            byte[] aa = new byte[n];
            Unsafe.CopyBlock(ref aa[0], ref Unsafe.As<uint, byte>(ref a[0]), n);
            return aa;
        }
        /// <summary>
        /// Encode a string by a special TEA algorithm.
        /// </summary>
        /// <param name="str">String to be encoded.</param>
        /// <param name="key">Key to encode.</param>
        /// <returns>Encoded string.</returns>
        public static byte[] XEncode(string str, string key)
        {
            if (str.Length == 0)
            {
                return Array.Empty<byte>();
            }
            uint[] v = S(Encoding.UTF8.GetBytes(str), true);
            uint[] k = S(Encoding.UTF8.GetBytes(key), false);
            int n = v.Length - 1;
            uint z = v[n];
            uint y;
            int q = 6 + 52 / (n + 1);
            uint d = 0;
            while (q-- > 0)
            {
                d += 0x9E3779B9;
                uint e = (d >> 2) & 3;
                for (int p = 0; p <= n; p++)
                {
                    y = v[(p + 1) % (n + 1)];
                    uint m = (z >> 5) ^ (y << 2);
                    m += (y >> 3) ^ (z << 4) ^ (d ^ y);
                    m += k[(p & 3) ^ (int)e] ^ z;
                    z = v[p] += m;
                }
            }
            return L(v, false);
        }

        private static readonly string Base64N = "LVoJPiCN2R8G90yg+hmFHuacZ1OWMnrsSTXkYpUq/3dlbfKwv6xztjI7DeBE45QA";
        /// <summary>
        /// Encode a string to base64 in a special way.
        /// </summary>
        /// <param name="t">String to be encoded.</param>
        /// <returns>Encoded string.</returns>
        public unsafe static string Base64Encode(byte[] t)
        {
            int a = t.Length;
            int len = (a + 2) / 3 * 4;
            char* u = stackalloc char[len];
            char r = '=';
            int ui = 0;
            for (int o = 0; o < a; o += 3)
            {
                int h = (t[o] << 16) + (o + 1 < a ? (t[o + 1] << 8) : 0) + (o + 2 < a ? t[o + 2] : 0);
                for (int i = 0; i < 4; i++)
                {
                    if (o * 8 + i * 6 > a * 8)
                    {
                        u[ui++] = r;
                    }
                    else
                    {
                        u[ui++] = Base64N[h >> 6 * (3 - i) & 0x3F];
                    }
                }
            }
            return new string(u, 0, len);
        }
        /// <summary>
        /// Get HMAC-MD5 computed string with a key and an empty string by <see cref="HMACMD5"/>.
        /// </summary>
        /// <param name="key">Original key.</param>
        /// <returns>An encoded string.</returns>
        public static string GetHMACMD5(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                key = string.Empty;
            }
            using (HMACMD5 hash = new HMACMD5(Encoding.UTF8.GetBytes(key)))
            {
                return GetHexString(hash.ComputeHash(Array.Empty<byte>()));
            }
        }
    }
}
