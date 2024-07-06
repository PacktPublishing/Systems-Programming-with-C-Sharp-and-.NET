using System.Text.Json;
using ProtoBuf;

namespace Streams;

public class SerializerSample
{
    [Flags]
    public enum MyFlags
    {
        FlagOne,
        FlagTwo,
        FlagThree
    }

    public string SerializeToJSon(MyData myData)
    {
        ArgumentNullException.ThrowIfNull(myData, nameof(myData));

        var options = new JsonSerializerOptions
        {
            WriteIndented = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var result = JsonSerializer.Serialize(myData, options);


        return result;
    }

    public MyData DeserializeFromJSon(string json)
    {
        ArgumentNullException.ThrowIfNull(json, nameof(json));

        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var result = JsonSerializer.Deserialize<MyData>(json, options);

        return result!;
    }

    public async Task<byte[]> SerializeToBinary(MyData myData)
    {
        await using var stream = new MemoryStream();
        Serializer.Serialize(stream, myData);
        return stream.ToArray();
    }

    public async Task<MyData> DeserializeFromBinary(byte[] payLoad)
    {
        await using var stream = new MemoryStream(payLoad);
        var myData = Serializer.Deserialize<MyData>(stream);
        return myData;
    }

    [ProtoContract]
    public class MyData
    {
        [ProtoMember(1)] public int Id { get; set; }

        [ProtoMember(2)] public double SomeMagicNumber { get; set; }

        [ProtoMember(3)] public bool IsThisAGoodDataSet { get; set; }

        [ProtoMember(4)] public MyFlags SomeFlags { get; set; }

        [ProtoMember(5)] public string? SomeText { get; set; }
    }
}