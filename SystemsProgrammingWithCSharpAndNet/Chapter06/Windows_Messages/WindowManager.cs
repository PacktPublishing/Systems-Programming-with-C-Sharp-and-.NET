using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ExtensionLibrary;

// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

namespace Windows_Messages
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
            public nint lpfnWndProc;
            public int cbClsExtra;
            public int cbWndExtra;
            public nint hInstance;
            public nint hIcon;
            public nint hCursor;
            public nint hbrBackground;
            public string lpszMenuName;
            public string lpszClassName;
            public nint hIconSm;
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern ushort RegisterClassEx([In] ref WNDCLASSEX lpWndClass);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern nint CreateWindowEx(
            uint dwExStyle,
            string lpClassName,
            string lpWindowName,
            uint dwStyle,
            int x,
            int y,
            int nWidth,
            int nHeight,
            nint hWndParent,
            nint hMenu,
            nint hInstance,
            nint lpParam
        );

        private const uint CS_HREDRAW = 0x0002;
        private const uint CS_VREDRAW = 0x0001;
        #endregion


        #region Windows Messages
        [DllImport("user32.dll")]
        private static extern nint DefWindowProc(nint hWnd, uint uMsg, nint wParam, nint lParam);

        [DllImport("user32.dll")]
        private static extern bool DestroyWindow(nint hWnd);

        [DllImport("user32.dll")]
        private static extern void PostQuitMessage(int nExitCode);

        private const uint WM_CLOSE = 0x0010;
        private const uint WM_DESTROY = 0x0002;

        private const uint WS_VISIBLE = 0x10000000;

        #endregion


        #region Message Handling
        [DllImport("user32.dll")]
        private static extern bool GetMessage(out MSG lpMsg, nint hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

        [DllImport("user32.dll")]
        private static extern bool TranslateMessage([In] ref MSG lpMsg);

        [DllImport("user32.dll")]
        private static extern nint DispatchMessage([In] ref MSG lpMsg);

        [StructLayout(LayoutKind.Sequential)]
        public struct MSG
        {
            public nint hwnd;
            public uint message;
            public nint wParam;
            public nint lParam;
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
            wc.cbSize = (uint)Marshal.SizeOf(typeof(WNDCLASSEX));
            wc.style = CS_HREDRAW | CS_VREDRAW;
            wc.hInstance = Marshal.GetHINSTANCE(typeof(WindowManager).Module);
            wc.lpszClassName = "MyHiddenWindowClass";

            WndProcDelegate wndProcDel = WndProc; // Create delegate

            // Fill wc fields here...
            wc.lpfnWndProc = Marshal.GetFunctionPointerForDelegate(wndProcDel);


            ushort classAtom = RegisterClassEx(ref wc);

            // Create the window
            nint hwnd = CreateWindowEx(
                0,
                wc.lpszClassName,
                "My Hidden Window",
                WS_VISIBLE,
                0, 0, 100, 100,
                nint.Zero,
                nint.Zero,
                nint.Zero,
                nint.Zero);

            $"Window has been created. Window Handle is {hwnd}".Dump(ConsoleColor.Yellow);

            // Register a custom message 
            _myMessage = RegisterWindowMessage("SP_DoSomething");

            // Implement the message loop and WndProc...
            RunMessageLoop();

        }

        private delegate nint WndProcDelegate(nint hWnd, uint msg, nint wParam, nint lParam);


        private static nint WndProc(nint hWnd, uint msg, nint wParam, nint lParam)
        {
            if (msg == _myMessage)
            {
                "We got our own message!".Dump(ConsoleColor.Yellow);
                return nint.Zero;
            }
            else { }
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
            return nint.Zero;
        }


        // Example message loop
        public static void RunMessageLoop()
        {
            MSG msg;
            while (GetMessage(out msg, nint.Zero, 0, 0))
            {
                TranslateMessage(ref msg);
                DispatchMessage(ref msg);
            }
        }

    }
}
