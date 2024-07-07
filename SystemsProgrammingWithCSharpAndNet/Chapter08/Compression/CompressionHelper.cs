using System.IO.Compression;
using System.Text;

namespace Compression;

internal class CompressionHelper
{
    public byte[] Compress(string message)
    {
        var originalMessage = Encoding.UTF8.GetBytes(message);
        using var memoryStream = new MemoryStream();
        using var gzipStream = new GZipStream(memoryStream, CompressionMode.Compress);
        gzipStream.Write(originalMessage, 0, originalMessage.Length);
        gzipStream.Flush();
        memoryStream.Flush();
        return memoryStream.ToArray();
    }

    public string Decompress(byte[] compressedMessage)
    {
        using var decompressedMemoryStream = new MemoryStream(compressedMessage);
        using var decompressionStream = new GZipStream(decompressedMemoryStream, CompressionMode.Decompress);
        using var decompressedMemoryStreamCopy = new MemoryStream();
        decompressionStream.CopyTo(decompressedMemoryStreamCopy);
        var decompressedMessage = decompressedMemoryStreamCopy.ToArray();
        return Encoding.UTF8.GetString(decompressedMessage);
    }
}