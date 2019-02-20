// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Security.Cryptography;
using System.Text;

namespace Softeq.NetKit.Auth.Common.Utility.Hashing
{
    public static class HashHelper
    {
        public const string DefaultSalt = "ChocolateSaltyBalls";

        public static string HashWithSalt(this string toBeHashed, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(Combine(GetBytes(toBeHashed), GetBytes(salt)));
                return Encoding.Unicode.GetString(hashBytes);
            }
        }

        private static byte[] Combine(byte[] first, byte[] second)
        {
            var ret = new byte[first.Length + second.Length];

            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);

            return ret;
        }

        private static byte[] GetBytes(string salt)
        {
            return Encoding.Unicode.GetBytes(salt);
        }

        public static string SignToBase64HmacSha256(string dataToSign, string signatureKey)
        {
            return Convert.ToBase64String(GetHmacSha256(dataToSign, signatureKey));
        }

        private static byte[] GetHmacSha256(string data, string key)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            return GetHmacSha256(dataBytes, keyBytes);
        }

        private static byte[] GetHmacSha256(byte[] contents, byte[] key)
        {
            using (var hmacSha256 = HMAC.Create("HMACSHA256"))
            {
                hmacSha256.Key = key;
                byte[] returnValue = hmacSha256.ComputeHash(contents);
                return returnValue;
            }
        }
    }
}
