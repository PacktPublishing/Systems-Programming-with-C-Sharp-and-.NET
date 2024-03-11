using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ExtensionLibrary;

// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

namespace _01Windows_Messages
{
    internal class WindowManager
    {
        private static uint _myMessage;

        #region Windows Creation
        [StructLayout(LayoutKind.Sequential)]
        public struct WNDCLASSEX
        {
            public uint cbSize;
            public uint style;
            public IntPtr lpfnWndProc;
            public int cbClsExtra;
            public int cbWndExtra;
            public IntPtr hInstance;
            public IntPtr hIcon;
            public IntPtr hCursor;
            public IntPtr hbrBackground;
            public string lpszMenuName;
            public string lpszClassName;
            public IntPtr hIconSm;
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern ushort RegisterClassEx([In] ref WNDCLASSEX lpWndClass);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr CreateWindowEx(
            uint dwExStyle,
            string lpClassName,
            string lpWindowName,
            uint dwStyle,
            int x,
            int y,
            int nWidth,
            int nHeight,
            IntPtr hWndParent,
            IntPtr hMenu,
            IntPtr hInstance,
            IntPtr lpParam
        );

        private const uint CS_HREDRAW = 0x0002;
        private const uint CS_VREDRAW = 0x0001;
        #endregion


        #region Windows Messages
        [DllImport("user32.dll")]
        private static extern IntPtr DefWindowProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool DestroyWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern void PostQuitMessage(int nExitCode);

        private const uint WM_CLOSE = 0x0010;
        private const uint WM_DESTROY = 0x0002;

        private const uint WS_VISIBLE = 0x10000000;

        #endregion


        #region Message Handling
        [DllImport("user32.dll")]
        private static extern bool GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

        [DllImport("user32.dll")]
        private static extern bool TranslateMessage([In] ref MSG lpMsg);

        [DllImport("user32.dll")]
        private static extern IntPtr DispatchMessage([In] ref MSG lpMsg);

        [StructLayout(LayoutKind.Sequential)]
        public struct MSG
        {
            public IntPtr hwnd;
            public uint message;
            public IntPtr wParam;
            public IntPtr lParam;
            public uint time;
            public POINT pt;
        }

        public struct POINT
        {
            public int X;
            public int Y;
        }

        #endregion


        #region Custom Messages

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern uint RegisterWindowMessage(string lpString);
        #endregion



        public static void CreateWindow()
        {

            // Define and register the window class
            WNDCLASSEX wc = new WNDCLASSEX();
            wc.cbSize= (uint)Marshal.SizeOf(typeof(WNDCLASSEX));
            wc.style = CS_HREDRAW | CS_VREDRAW;
            wc.hInstance = Marshal.GetHINSTANCE(typeof(WindowManager).Module);
            wc.lpszClassName = "MyHiddenWindowClass";
            
            WndProcDelegate wndProcDel = WndProc; // Create delegate
            
            // Fill wc fields here...
            wc.lpfnWndProc = Marshal.GetFunctionPointerForDelegate(wndProcDel);

            
            ushort classAtom = RegisterClassEx(ref wc);

            // Create the window
            IntPtr hwnd = CreateWindowEx(
                0, 
                wc.lpszClassName, 
                "My Hidden Window", 
                WS_VISIBLE,
                0, 0, 100, 100, 
                IntPtr.Zero,
                IntPtr.Zero, 
                IntPtr.Zero,
                IntPtr.Zero);

            $"Window has been created. Window Handle is {hwnd}".Dump(ConsoleColor.Yellow);

            // Register a custom message 
            _myMessage = RegisterWindowMessage("SP_DoSomething");

            // Implement the message loop and WndProc...
            RunMessageLoop();

        }

        private delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);


        private static IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            if (msg == _myMessage)
            {
                "We got our own message!".Dump(ConsoleColor.Yellow);
                return IntPtr.Zero;
            }else{}
            switch (msg)
            {
                // Handle different messages here
                case WM_CLOSE:
                    DestroyWindow(hWnd);
                    break;
                case WM_DESTROY:
                    PostQuitMessage(0);
                    break;
                // Default case for unhandled messages
                default:
                    Console.WriteLine($"Windows Handle: {hWnd:x8}, {msg:x8}, {wParam:x8}, {lParam:x8}");
                    return DefWindowProc(hWnd, msg, wParam, lParam);
            }
            return IntPtr.Zero;
        }


        // Example message loop
        public static void RunMessageLoop()
        {
            MSG msg;
            while (GetMessage(out msg, IntPtr.Zero, 0, 0))
            {
                TranslateMessage(ref msg);
                DispatchMessage(ref msg);
            }
        }

    }
}
