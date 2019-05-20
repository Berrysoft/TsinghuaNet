using System;
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
            //StringBuilder is 5 times faster than stack array and array.
            StringBuilder sBuilder = new StringBuilder();
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
        private static unsafe uint[] S(string a, bool b)
        {
            int c = a.Length;
            int n = c / 4;
            n += c % 4 != 0 ? 1 : 0;
            //Array is 30 times faster than stack array and Encoding.GetBytes().
            uint[] v;
            if (b)
            {
                v = new uint[n + 1];
                v[n] = (uint)c;
            }
            else
            {
                v = new uint[n >= 4 ? n : 4];
            }
            fixed (uint* pv = v)
            {
                byte* pb = (byte*)pv;
                for (int i = 0; i < c; i++)
                {
                    pb[i] = (byte)a[i];
                }
            }
            return v;
        }
        /// <summary>
        /// Decode a <see cref="string"/> from its UTF-8 form.
        /// </summary>
        /// <param name="a">A <see cref="uint"/> array contains the encoded string.</param>
        /// <param name="b">Whether the length of the original string is in the end.</param>
        /// <returns>Decoded string.</returns>
        private static unsafe string L(uint[] a, bool b)
        {
            int d = a.Length;
            uint c = ((uint)(d - 1)) << 2;
            if (b)
            {
                uint m = a[d - 1];
                if (m < c - 3 || m > c)
                {
                    return null;
                }
                c = m;
            }
            fixed (uint* pa = a)
            {
                byte* pb = (byte*)pa;
                int n = d << 2;
                //When the return string needs subtracted, stack array is a little faster than array;
                //otherwise, array is 1.2 times faster than stack array.
                char[] aa = new char[n];
                for (int i = 0; i < n; i++)
                {
                    aa[i] = (char)pb[i];
                }
                if (b)
                {
                    return new string(aa, 0, (int)c);
                }
                else
                {
                    return new string(aa);
                }
            }
        }
        /// <summary>
        /// Encode a string by a special TEA algorithm.
        /// </summary>
        /// <param name="str">String to be encoded.</param>
        /// <param name="key">Key to encode.</param>
        /// <returns>Encoded string.</returns>
        public static string XEncode(string str, string key)
        {
            if (str.Length == 0)
            {
                return string.Empty;
            }
            uint[] v = S(str, true);
            uint[] k = S(key, false);
            int n = v.Length - 1;
            uint z = v[n];
            uint y = v[0];
            int q = 6 + 52 / (n + 1);
            uint d = 0;
            while (q-- > 0)
            {
                d += 0x9E3779B9;
                uint e = (d >> 2) & 3;
                for (int p = 0; p <= n; p++)
                {
                    y = v[p == n ? 0 : p + 1];
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
        public unsafe static string Base64Encode(string t)
        {
            int a = t.Length;
            int len = a / 3 * 4;
            len += a % 3 != 0 ? 4 : 0;
            //Stack array is 30 times faster than array, StringBuilder and Converter.ToBase64String().
            char* u = stackalloc char[len];
            char r = '=';
            int h = 0;
            byte* p = (byte*)&h;
            int ui = 0;
            for (int o = 0; o < a; o += 3)
            {
                p[2] = (byte)t[o];
                p[1] = (byte)(o + 1 < a ? t[o + 1] : 0);
                p[0] = (byte)(o + 2 < a ? t[o + 2] : 0);
                for (int i = 0; i < 4; i += 1)
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
