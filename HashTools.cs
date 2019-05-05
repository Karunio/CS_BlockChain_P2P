using System.Text;
using System.Security.Cryptography;
using System;

namespace VSCODE_PR
{
    static class HashTools
    {
        private static SHA256 sha256 = SHA256.Create();

        public static byte[] ToBytesHash(byte[] inputBytes) => sha256.ComputeHash(inputBytes);

        public static byte[] ToBytesHash(String inputString) => sha256.ComputeHash(Encoding.UTF8.GetBytes(inputString));

        public static String ToBase64Hash(byte[] inputBytes) => Convert.ToBase64String(ToBytesHash(inputBytes));

        public static String ToBase64Hash(String inputString) => Convert.ToBase64String(ToBytesHash(inputString));
    }
}
