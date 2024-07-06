using System.Text;
using ExtensionLibrary;
using Streams;

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

"Done".Dump();
Console.ReadKey();

