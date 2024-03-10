using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using ProtoBuf;

namespace _02Streams
{
    public class SerializerSample
    {

        [ProtoContract]
        public class MyData
        {
            [ProtoMember(1)]
            public int Id { get; set; }
            [ProtoMember(2)] 
            public double SomeMagicNumber { get; set; }
            [ProtoMember(3)] 
            public bool IsThisAGoodDataSet { get; set; }
            [ProtoMember(4)] 
            public MyFlags SomeFlags { get; set; }
            [ProtoMember(5)] 
            public string? SomeText { get; set; }
        }

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

            var result = System.Text.Json.JsonSerializer.Serialize(myData, options);

            

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

            var result = System.Text.Json.JsonSerializer.Deserialize<MyData>(json, options);

            return result!;
        }

        public async Task<byte[]> SerializeToBinary(MyData myData)
        {
            await using var stream = new MemoryStream();
            ProtoBuf.Serializer.Serialize(stream, myData);
            return stream.ToArray();
        }

        public async Task<MyData> DeserializeFromBinary(byte[] payLoad)
        {
            await using var stream = new MemoryStream(payLoad);
            var myData = ProtoBuf.Serializer.Deserialize<MyData>(stream);
            return myData;
        }
    }




}
