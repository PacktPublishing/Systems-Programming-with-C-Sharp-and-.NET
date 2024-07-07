using System.Security.Cryptography;
using System.Text;

namespace Streams;

internal class Encryption
{
    // Encrypt a string using symmetric encryption and return that 
    public static void EncryptFileSymmetric(string inputFile, string outputFile, string key)
    {
        using (var inputFileStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
        using (var outputFileStream = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = keyBytes;
                aesAlg.GenerateIV();
                var ivBytes = aesAlg.IV;

                outputFileStream.Write(ivBytes, 0, ivBytes.Length);

                using (var csEncrypt = new CryptoStream(outputFileStream, aesAlg.CreateEncryptor(),
                           CryptoStreamMode.Write))
                {
                    var buffer = new byte[4096];
                    int bytesRead;
                    while ((bytesRead = inputFileStream.Read(buffer, 0, buffer.Length)) > 0)
                        csEncrypt.Write(buffer, 0, bytesRead);
                }
            }
        }
    }


    public static void DecryptFileSymmetric(string inputFile, string outputFile, string key)
    {
        using (var inputFileStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
        using (var outputFileStream = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            using (var aesAlg = Aes.Create())
            {
                var ivBytes = new byte[aesAlg.BlockSize / 8];
                inputFileStream.Read(ivBytes, 0, ivBytes.Length);
                aesAlg.Key = keyBytes;
                aesAlg.IV = ivBytes;

                using (var csDecrypt =
                       new CryptoStream(outputFileStream, aesAlg.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    var buffer = new byte[4096];
                    int bytesRead;
                    while ((bytesRead = inputFileStream.Read(buffer, 0, buffer.Length)) > 0)
                        csDecrypt.Write(buffer, 0, bytesRead);
                }
            }
        }
    }


    public static (string, string) GenerateKeyPair()
    {
        using var rsa = RSA.Create();

        var publicKeyBytes = rsa.ExportRSAPublicKey();
        var privateKeyBytes = rsa.ExportRSAPrivateKey();

        var publicKeyBase64 = Convert.ToBase64String(publicKeyBytes);
        var privateKeyBase64 = Convert.ToBase64String(privateKeyBytes);

        return (publicKeyBase64, privateKeyBase64);
    }

    public static byte[] EncryptWithPublicKey(
        byte[] data,
        byte[] publicKeyBytes)
    {
        using var rsa = RSA.Create();
        rsa.ImportRSAPublicKey(publicKeyBytes, out _);
        return rsa.Encrypt(data, RSAEncryptionPadding.OaepSHA256);
    }

    public static byte[] DecryptWithPrivateKey(
        byte[] encryptedData,
        byte[] privateKeyBytes)
    {
        using var rsa = RSA.Create();
        rsa.ImportRSAPrivateKey(privateKeyBytes, out _);
        return rsa.Decrypt(encryptedData, RSAEncryptionPadding.OaepSHA256);
    }
}