using BenchmarkDotNet.Attributes;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using static System.Net.Mime.MediaTypeNames;
using System.Text;

namespace _02Streams;

public class Writers
{
    const uint GENERIC_WRITE = 0x40000000;
    const uint OPEN_EXISTING = 3;
    const uint FILE_APPEND_DATA = 0x00000004;

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern SafeFileHandle CreateFile(
        string lpFileName,
        uint dwDesiredAccess,
        uint dwShareMode,
        IntPtr lpSecurityAttributes,
        uint dwCreationDisposition,
        uint dwFlagsAndAttributes,
        IntPtr hTemplateFile);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool WriteFile(
        SafeFileHandle hFile,
        byte[] lpBuffer,
        uint nNumberOfBytesToWrite,
        out uint lpNumberOfBytesWritten,
        IntPtr lpOverlapped);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool CloseHandle(SafeFileHandle hObject);

    private byte[] _dataToWrite;

    //[GlobalSetup]
    //public void Setup()
    //{
    //    _dataToWrite = new byte[0xFFFF];
    //    Random r = new();
    //    for (var i = 0; i < 0xFFFF; i++)
    //    {
    //        var nextByte = r.Next(0, 255);
    //        _dataToWrite[i] = (byte)nextByte;
    //    }
    //}

    [Benchmark]
    public void WriteWithWin32()
    {
        var fileName = Path.GetTempFileName();
        try
        {
            SafeFileHandle fileHandle = CreateFile(
                fileName,
                GENERIC_WRITE,
                0,
                IntPtr.Zero,
                OPEN_EXISTING,
                FILE_APPEND_DATA,
                IntPtr.Zero);

            if (!fileHandle.IsInvalid)
            {
                try
                {
                    byte[] bytes = Encoding.ASCII.GetBytes("Hello, fellow System Programmers! ");
                    
                    bool writeResult = WriteFile(
                            fileHandle,
                            bytes,
                            (uint)bytes.Length,
                            out uint bytesWritten,
                            IntPtr.Zero);

                }
                finally
                {

                    // Always close the handle once you are done
                    CloseHandle(fileHandle);
                }
            }
            else
            {
                Console.WriteLine("Failed to open file.");
            }
            //using var fs = File.Create(fileName);
            //try
            //{
            //    using var bs = new BufferedStream(fs);
            //    for (var i = 0; i < 0xFF; i++)
            //    {
            //        var buffer = bytesToWrite.Slice(i * 0xFF, 0xFF).ToArray();
            //        bs.Write(buffer, i*0xFF, 0xFF);
            //    }
            //}
            //finally
            //{
            //    fs.Close();
            //}
        }
        finally
        {
            File.Delete(fileName);
        }
    }

    [Benchmark]
    public void WriteWithStream()
    {
        var bytesToWrite = new Span<byte>(_dataToWrite);

        var fileName = Path.GetTempFileName();
        try
        {
            using var fs = File.Create(fileName);
            try
            {
                for (var i = 0; i < 0xFF; i++)
                {
                    byte[] bytes = Encoding.ASCII.GetBytes("Hello, fellow System Programmers!");

                    fs.Write(bytes, 0, bytes.Length);
                }
            }
            finally
            {
                fs.Close();
            }
        }
        finally
        {
            File.Delete(fileName);
        }
    }
}