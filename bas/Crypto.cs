using System;
using System.Text;
using System.Security.Cryptography;

public class Crypto
{
    private string _key { get; set; } = "Load1None2Hell3";

    private static byte[] MD5Hash(string value)
    {
        var MD5 = new MD5CryptoServiceProvider();
        return MD5.ComputeHash(UTF8Encoding.UTF8.GetBytes(value));
    }

    public string Encrypt(string strExpression, string key = null)
    {
        if (key == null) key = _key;

        TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
        DES.Key = MD5Hash(key);
        DES.Mode = CipherMode.ECB;

        byte[] Buffer = UTF8Encoding.UTF8.GetBytes(strExpression);

        return Convert.ToBase64String(DES.CreateEncryptor().TransformFinalBlock(Buffer, 0, Buffer.Length));
    }

    public string Decrypt(string strExpression, string key = null)
    {
        if (key == null) key = _key;
        TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
        DES.Key = MD5Hash(key);
        DES.Mode = CipherMode.ECB;

        byte[] Buffer = Convert.FromBase64String(strExpression);

        return UTF8Encoding.UTF8.GetString(DES.CreateDecryptor().TransformFinalBlock(Buffer, 0, Buffer.Length));
    }
}
