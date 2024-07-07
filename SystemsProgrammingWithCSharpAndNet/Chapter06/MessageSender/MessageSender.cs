using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ExtensionLibrary;

namespace MessageSender
{
    internal class MessageSender
    {
        #region Custom Messages

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern uint RegisterWindowMessage(string lpString);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern nint SendMessage(nint hWnd, uint Msg, nint wParam, nint lParam);

        #endregion

        public void Send()
        {
            var myMessage = RegisterWindowMessage("SP_DoSomething");

            "Enter the window handle".Dump(ConsoleColor.DarkYellow);
            var windowHandle = Convert.ToInt32(Console.ReadLine());

            SendMessage(windowHandle, myMessage, nint.Zero, nint.Zero);

            "Message has been sent".Dump(ConsoleColor.DarkYellow);
        }
    }
}
