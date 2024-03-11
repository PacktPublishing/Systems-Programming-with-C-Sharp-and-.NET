using System.Text;
using _02Streams;
using HelperClasses;

SerializerSample sample = new();
var myData = new SerializerSample.MyData()
{
    Id = 42,
    IsThisAGoodDataSet = true,
    SomeFlags = SerializerSample.MyFlags.FlagOne | SerializerSample.MyFlags.FlagThree,
    SomeMagicNumber = 3.1415,
    SomeText = "This is some text that we want to serialize"
};

var serialized = sample.SerializeToJSon(myData);
serialized.Dump(ConsoleColor.DarkYellow);
// Display the size in bytes of the string serialized
Encoding.UTF8.GetByteCount(serialized).ToString().Dump(ConsoleColor.DarkYellow);

var myNewData = sample.DeserializeFromJSon(serialized);
"Conversion done.".Dump(ConsoleColor.Cyan);


var myBinaryData = await sample.SerializeToBinary(myData);
myBinaryData.Length.ToString().Dump(ConsoleColor.DarkYellow);
var myNewData2 = await sample.DeserializeFromBinary(myBinaryData);

myNewData2.SomeText.Dump(ConsoleColor.Cyan);

//var cts = new CancellationTokenSource();
//var myText = "This is some text that I want to compress.";

//var compression = new Compression();
//var compressed = await compression.CompressString(myText, cts.Token);

//var decompressed = await compression.DecompressString(compressed, cts.Token);
//decompressed.Dump(ConsoleColor.DarkYellow);


//(string, string) keyPair = Encryption.GenerateKeyPair();
//keyPair.Item1.Dump();
//keyPair.Item2.Dump();

//var publicKey = Convert.FromBase64String(keyPair.Item1);
//var privateKey = Convert.FromBase64String(keyPair.Item2);

//string message = "This is the text that we, as System Programmers, want to secure.";
//byte[] messageBytes = Encoding.UTF8.GetBytes(message);

//byte[] encryptedBytes = Encryption.EncryptWithPublicKey(messageBytes, publicKey);
//string encrypted = Encoding.UTF8.GetString(encryptedBytes);
//encrypted.Dump(ConsoleColor.DarkYellow);

//byte[] decryptedBytes = Encryption.DecryptWithPrivateKey(encryptedBytes, privateKey);
//string decrypted = Encoding.UTF8.GetString(decryptedBytes);
//decrypted.Dump(ConsoleColor.DarkYellow);




//Encryption.EncryptFileSymmetric(@"c:\temp\watch\hello2.txt", @"c:\temp\watch\encrypted.txt", "SystemSoftware42");
//Encryption.DecryptFileSymmetric(@"c:\temp\watch\encrypted.txt", @"c:\temp\watch\cleartext.txt", "SystemSoftware42");


//var encryption = new Encryption();
//var encrypted = encryption.EncryptString("This is a good sample of an encrypted text");
//encrypted.Dump();

//var decrypted = encryption.DecryptString(encrypted);
//decrypted.Dump();

//var cancellationTokenSource = new CancellationTokenSource();

//ThreadPool.QueueUserWorkItem((_) =>
//{
//    Thread.Sleep(10000);
//    Console.WriteLine("About to cancel the operation");
//    cancellationTokenSource.Cancel();
//});

//var asyncSample = new AsyncSample();
//await asyncSample.CreateBigFile(
//    @"c:\temp\bigFile.txt", 
//    cancellationTokenSource.Token);


//MyFolderWatcher watcher = new();
//"Setting up the watcher".Dump(ConsoleColor.Yellow);
//watcher.SetupWatcher(@"c:\temp\watch");
//watcher.FileAdded += (sender, eventArgs) => { $"Event is raised: {eventArgs.FilePath}".Dump(ConsoleColor.DarkYellow); };
//watcher.FileChanged += (sender, eventArgs) => { $"File has changed: {eventArgs.FilePath}".Dump(ConsoleColor.Cyan); };

Console.ReadKey();

//new DirectoryWatcher().DoWork();

//new PathReader().DumpImages();
//new PathReader().GetDirectoryInformation();

//var jsonFile = @"c:\temp\test.txt";
//var binaryFile = @"c:\temp\test.bin";

//new ClassWriter().WriteAsJson(jsonFile);
//new ClassWriter().WriteAsBinary(binaryFile);
//var myData = new ClassWriter().Read(binaryFile);
//Console.WriteLine(myData.SomeText);

//var allLines = new Reader().ReadWithStream(jsonFile);
//Console.WriteLine(allLines);

//var fileName = Path.GetTempFileName();
//fileName.ToClipboard();

//var info = new UTF8Encoding(true).GetBytes("Hello fellow System Developers!");
//using (var fs = new FileStream(
//    path: fileName,
//    mode: FileMode.Create,
//    access: FileAccess.Write,
//    share: FileShare.Delete, // We allow other processes to delete the file
//    bufferSize: 0x1000,
//    options: FileOptions.Asynchronous))
//{
//    try
//    {
//        fs.Write(info, 0, info.Length);
//        Console.WriteLine($"Wrote to the file. Now try to delete it. You can find it here:\n{fileName}");
//        Console.ReadKey();
//        fs.Write(info);
//        Console.WriteLine("Done with all the writing");
//        Console.ReadKey();
//    }
//    finally
//    {
//        fs.Close();
//    }
//}

Console.WriteLine("Done.");
Console.ReadKey();

