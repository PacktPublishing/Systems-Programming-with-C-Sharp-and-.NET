using System.Runtime.InteropServices;

namespace _02Streams;

internal class BufferedSample
{
    public async Task WriteBufferedData(string fileName)
    {
        var data = new DataRecord
        {
            Id = 42,
            LogDate = DateTime.UtcNow,
            Price = 12.34
        };

        await using FileStream stream = new(fileName, FileMode.CreateNew, FileAccess.Write);
        await using BufferedStream bufferedStream = new(stream, Marshal.SizeOf<DataRecord>());
        await using BinaryWriter writer = new(bufferedStream);
        writer.Write(data.Id);
        writer.Write(data.LogDate.ToBinary());
        writer.Write(data.Price);
    }
}

internal readonly record struct DataRecord
{
    public int Id { get; init; }
    public DateTime LogDate { get; init; }
    public double Price { get; init; }
}