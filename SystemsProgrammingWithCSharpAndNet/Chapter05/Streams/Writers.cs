using System.Runtime.InteropServices;
using System.Text;
using BenchmarkDotNet.Attributes;
using Microsoft.Win32.SafeHandles;

namespace Streams;

public class Writers
{
    private const uint GENERIC_WRITE = 0x40000000;
    private const uint OPEN_EXISTING = 3;
    private const uint FILE_APPEND_DATA = 0x00000004;

    private byte[] _dataToWrite;

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern SafeFileHandle CreateFile(
        string lpFileName,
        uint dwDesiredAccess,
        uint dwShareMode,
        nint lpSecurityAttributes,
        uint dwCreationDisposition,
        uint dwFlagsAndAttributes,
        nint hTemplateFile);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool WriteFile(
        SafeFileHandle hFile,
        byte[] lpBuffer,
        uint nNumberOfBytesToWrite,
        out uint lpNumberOfBytesWritten,
        nint lpOverlapped);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool CloseHandle(SafeFileHandle hObject);

    [Benchmark]
    public void WriteWithWin32()
    {
        var fileName = Path.GetTempFileName();
        try
        {
            var fileHandle = CreateFile(
                fileName,
                GENERIC_WRITE,
                0,
                nint.Zero,
                OPEN_EXISTING,
                FILE_APPEND_DATA,
                nint.Zero);

            if (!fileHandle.IsInvalid)
                try
                {
                    var bytes = Encoding.ASCII.GetBytes("Hello, fellow System Programmers! ");

                    var writeResult = WriteFile(
                        fileHandle,
                        bytes,
                        (uint)bytes.Length,
                        out var bytesWritten,
                        nint.Zero);
                }
                finally
                {
                    // Always close the handle once you are done
                    CloseHandle(fileHandle);
                }
            else
                Console.WriteLine("Failed to open file.");
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
                    var bytes = Encoding.ASCII.GetBytes("Hello, fellow System Programmers!");

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