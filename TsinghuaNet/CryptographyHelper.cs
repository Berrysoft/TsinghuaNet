using System;
using System.Security.Cryptography;
using System.Text;
using Berrysoft.XXTea;

namespace TsinghuaNet
{
    internal static class CryptographyHelper
    {
        private static string GetHexString(byte[] data)
        {
            StringBuilder sBuilder = new StringBuilder(data.Length * 2);
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

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

        private static readonly XXTeaCryptor XXTea = new XXTeaCryptor();
        public static byte[] XXTeaEncrypt(string data, string key)
        {
            XXTea.ConsumeKey(key);
            return XXTea.EncryptString(data);
        }

        private static readonly string Base64N = "LVoJPiCN2R8G90yg+hmFHuacZ1OWMnrsSTXkYpUq/3dlbfKwv6xztjI7DeBE45QA";
        public unsafe static string Base64Encode(byte[] t)
        {
            int a = t.Length;
            int len = (a + 2) / 3 * 4;
#if NETCOREAPP3_0 || NETSTANDARD2_1
            return string.Create(len, t, Base64EncodeInternal);
#else
            Span<char> u = stackalloc char[len];
            Base64EncodeInternal(u, t);
            return u.ToString();
#endif
        }

        private static void Base64EncodeInternal(Span<char> u, byte[] t)
        {
            int a = t.Length;
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
        }

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