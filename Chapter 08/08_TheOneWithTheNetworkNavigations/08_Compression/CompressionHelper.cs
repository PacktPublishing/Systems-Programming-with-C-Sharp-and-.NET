using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _08_Compression
{
    internal class CompressionHelper
    {
        public byte[] Compress(string message)
        {
            var originalMessage = System.Text.Encoding.UTF8.GetBytes(message);
            using var memoryStream = new System.IO.MemoryStream();
            using var gzipStream = new System.IO.Compression.GZipStream(memoryStream, System.IO.Compression.CompressionMode.Compress);
            gzipStream.Write(originalMessage, 0, originalMessage.Length);
            gzipStream.Flush();
            memoryStream.Flush();
            return memoryStream.ToArray();
        }

        public string Decompress(byte[] compressedMessage)
        {
            using var decompressedMemoryStream = new System.IO.MemoryStream(compressedMessage);
            using var decompressionStream = new System.IO.Compression.GZipStream(decompressedMemoryStream, System.IO.Compression.CompressionMode.Decompress);
            using var decompressedMemoryStreamCopy = new System.IO.MemoryStream();
            decompressionStream.CopyTo(decompressedMemoryStreamCopy);
            var decompressedMessage = decompressedMemoryStreamCopy.ToArray();
            return System.Text.Encoding.UTF8.GetString(decompressedMessage);
        }
    }
}
