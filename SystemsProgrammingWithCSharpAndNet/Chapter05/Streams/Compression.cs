using System.IO.Compression;

namespace Streams
{
    internal class Compression
    {

        public async Task<byte[]> CompressString(string input,
            CancellationToken cancellationToken)
        {
            // Get the payload as bytes
            byte[] data = System.Text.Encoding.UTF8.GetBytes(input);

            // Compress to a MemoryStream
            await using var ms = new MemoryStream();
            await using var compressionStream = new GZipStream(ms, CompressionMode.Compress);
            await compressionStream.WriteAsync(data, 0, data.Length, cancellationToken);
            await compressionStream.FlushAsync(cancellationToken);
            // Get the compressed data.
            byte[] compressedData = ms.ToArray();
            return compressedData;
        }

        // Decompress a string 
        public async Task<string> DecompressString(byte[] input,
            CancellationToken cancellationToken)
        {
            // Write the data into a memory stream
            await using var ms = new MemoryStream();
            await ms.WriteAsync(input, cancellationToken);
            await ms.FlushAsync(cancellationToken);
            ms.Position = 0;

            // Decompress
            await using var decompressionStream = new GZipStream(ms, CompressionMode.Decompress);
            await using var resultStream = new MemoryStream();
            await decompressionStream.CopyToAsync(resultStream, cancellationToken);

            // Convert to readable text. 
            byte[] decompressedData = resultStream.ToArray();
            string decompressedString = System.Text.Encoding.UTF8.GetString(decompressedData);
            return decompressedString;
        }
    }
}
