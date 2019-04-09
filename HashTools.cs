using System.Text;
using System.Security.Cryptography;
using System;
static class HashTools
{
    private static SHA256 sha256 = SHA256.Create();

    public static byte[] ToBytesHash(byte[] inputBytes)
    {
        return sha256.ComputeHash(inputBytes);
    }

    public static byte[] ToBytesHash(String inputString)
    {
        return sha256.ComputeHash(Encoding.UTF8.GetBytes(inputString));
    }

    public static String ToBase64Hash(byte[] inputBytes)
    {
        return Convert.ToBase64String(ToBytesHash(inputBytes));
    }

    public static String ToBase64Hash(String inputString)
    {
        return Convert.ToBase64String(ToBytesHash(inputString));
    }
    
    public static byte[] FromBase64Hash(String inputHash)
    {
        return Convert.FromBase64String(inputHash);
    }
}