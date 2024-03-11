using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace _02Streams
{
    internal class ClassWriter
    {
        public void WriteAsJson(string fileName)
        {
            var myData = new MyData()
            {
                Id = 42,
                IsThisAGoodDataSet = true,
                SomeFlags = MyFlags.FlagOne | MyFlags.FlagThree,
                SomeMagicNumber = 3.1415,
                SomeText = "Hello, Systems Programmers"
            };

            var serializedData = JsonSerializer.Serialize(myData);

            File.WriteAllText(fileName, serializedData);
        }

        public void WriteAsBinary(string fileName)
        {
            var myData = new MyData()
            {
                Id = 42,
                IsThisAGoodDataSet = true,
                SomeFlags = MyFlags.FlagOne | MyFlags.FlagThree,
                SomeMagicNumber = 3.1415,
                SomeText = "Hello, Systems Programmers"
            };

            using var fs = File.OpenWrite(fileName);
            try
            {
                using BinaryWriter bw = new(fs);
                bw.Write(myData.Id);
                bw.Write(myData.IsThisAGoodDataSet);
                bw.Write(myData.SomeMagicNumber);
                bw.Write((int)myData.SomeFlags);
                bw.Write(myData.SomeText);
            }
            finally
            {
                fs.Close();
            }
        }

        public MyData Read(string fileName)
        {
            var myData = new MyData();

            using var fs = File.OpenRead(fileName);
            try
            {
                using BinaryReader br = new(fs);
                myData.Id = br.ReadInt32();
                myData.IsThisAGoodDataSet = br.ReadBoolean();
                myData.SomeMagicNumber = br.ReadDouble();
                myData.SomeFlags = (MyFlags)br.ReadInt32();
                myData.SomeText = br.ReadString();
            }
            finally
            {
                fs.Close();
            }
            return myData;
        }

    }
}


class MyData
{
    public int Id { get; set; }
    public double SomeMagicNumber { get; set; }
    public bool IsThisAGoodDataSet { get; set; }
    public MyFlags SomeFlags { get; set; }
    public string? SomeText { get; set; }
}

[Flags]
public enum MyFlags
{
    FlagOne,
    FlagTwo,
    FlagThree
}

