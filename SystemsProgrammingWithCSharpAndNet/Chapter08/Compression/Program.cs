using Compression;
using ExtensionLibrary;

var message = "This is my message. It is " +
              "not too long, so compressing it " +
              "will not do much. However, I " +
              "do think I can show you how " +
              "to compress it.The longer the " +
              "string is, the better the compression will be. " +
              "But even this small sample shows the power " +
              "of compression. Yes, it does take CPU time " +
              "but I think it is worth it!";
var messageLength = System.Text.Encoding.UTF8.GetBytes(message).Length;

var compressionHelper = new CompressionHelper();
var compressedMessage = compressionHelper.Compress(message);
var decompressedMessage = compressionHelper.Decompress(compressedMessage);

$"Original length: {messageLength}".Dump(ConsoleColor.Cyan);
$"Compressed length: {compressedMessage.Length}".Dump(ConsoleColor.Cyan);
$"Decompressed message: {decompressedMessage}".Dump(ConsoleColor.Cyan);
