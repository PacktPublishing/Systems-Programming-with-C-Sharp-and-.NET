using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace MyKiller
{
    internal class WindowFinder
    {
        private const int WM_GETTEXT = 0x000D;
        private const int WM_GETTEXTLENGTH = 0x000E;
        private const int WM_CLOSE = 0x0010;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string? lpClassName, string lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        public static void KillWindow()
        {
            // Specify the window title or partial caption of the target application
            string targetWindowTitle = "Kill Target";

            IntPtr targetWindowHandle = FindWindow(null, targetWindowTitle);
            if(targetWindowHandle == IntPtr.Zero)
            {
                Console.WriteLine("Target window not found.");
                return;
            }

            // Get the caption length
            int captionLength = SendMessage(targetWindowHandle, WM_GETTEXTLENGTH, 0, IntPtr.Zero) + 1;

            // Allocate memory for the caption
            IntPtr captionBuffer = Marshal.AllocCoTaskMem(captionLength * 2); // 2 bytes per character

            // Get the caption text
            SendMessage(targetWindowHandle, WM_GETTEXT, captionLength, captionBuffer);

            // Convert the caption from Unicode to a C# string
            
            string? caption = Marshal.PtrToStringUni(captionBuffer);
            // if caption is null, then the window handle is invalid, display an error
            if(caption == null)
            {
                Console.WriteLine("Target window handle is invalid.");
                return;
            }

            Console.WriteLine($"Caption: {caption}");

            // Free the allocated memory
            Marshal.FreeCoTaskMem(captionBuffer);

            // Kill the process associated with the target window
            int processId;
            GetWindowThreadProcessId(targetWindowHandle, out processId);
            if(processId != 0)
            {
                Process process = Process.GetProcessById(processId);
                process.Kill();
                Console.WriteLine("Process killed.");
            }

            Console.ReadLine();
        }

        // Helper method to retrieve the process ID associated with a window handle
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);
    }


    class WindowKiller
    {

    }
}
