using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace _02Streams;

internal class Win32Writer
{
    private const uint GENERIC_WRITE = 0x40000000;
    private const uint CREATE_ALWAYS = 0x00000002;
    private const uint FILE_APPEND_DATA = 0x00000004;

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern SafeFileHandle CreateFile(
        string lpFileName,
        uint dwDesiredAccess,
        uint dwShareMode,
        IntPtr lpSecurityAttributes,
        uint dwCreationDisposition,
        uint dwFlagsAndAttributes,
        IntPtr hTemplateFile);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool WriteFile(
        SafeFileHandle hFile,
        byte[] lpBuffer,
        uint nNumberOfBytesToWrite,
        out uint lpNumberOfBytesWritten,
        IntPtr lpOverlapped);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool CloseHandle(SafeFileHandle hObject);


    public void WriteToFile(string fileName, string textToWrite)
    {
        var fileHandle = CreateFile(
            fileName,
            GENERIC_WRITE,
            0,
            IntPtr.Zero,
            CREATE_ALWAYS,
            FILE_APPEND_DATA,
            IntPtr.Zero);

        if (!fileHandle.IsInvalid)
            try
            {
                var bytes = Encoding.ASCII.GetBytes(textToWrite);

                var writeResult = WriteFile(
                    fileHandle,
                    bytes,
                    (uint)bytes.Length,
                    out var bytesWritten,
                    IntPtr.Zero);
            }
            finally
            {
                // Always close the handle once you are done
                CloseHandle(fileHandle);
            }
        else
            Console.WriteLine("Failed to open file.");
    }
}