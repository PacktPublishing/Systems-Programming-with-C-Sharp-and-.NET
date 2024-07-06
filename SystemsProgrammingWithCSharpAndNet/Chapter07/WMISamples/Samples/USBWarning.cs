using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using ExtensionLibrary;

// This disables the warning about this code only being available on Windows.
#pragma warning disable CA1416

namespace WMISamples.Samples
{
    internal class USBWarning
    {
        public void StartListening()
        {
            string wmiQuery = "SELECT * FROM __InstanceDeletionEvent WITHIN 2 " +
                              "WHERE TargetInstance ISA 'Win32_USBHub'";

            ManagementEventWatcher watcher = new ManagementEventWatcher(wmiQuery);
            watcher.EventArrived += new EventArrivedEventHandler(USBRemoved);

            // Start listening for events
            watcher.Start();

            "Unplug a USB device to see the event.\nPress ENTER to exit.".Dump(ConsoleColor.Cyan);
            Console.ReadLine();

            // Stop listening for events
            watcher.Stop();
        }

        private void USBRemoved(object sender, EventArrivedEventArgs e)
        {
            // Get the instance of the removed device
            ManagementBaseObject instance = (ManagementBaseObject)e.NewEvent["TargetInstance"];

            // Extract some properties
            string deviceID = (string)instance["DeviceID"];
            string pnpDeviceID = (string)instance["PNPDeviceID"];
            string description = (string)instance["Description"];

            var message =
                $"USB device removed:" +
                $"\n\t\tDeviceID={deviceID}" +
                $"\n\t\tPNPDeviceID={pnpDeviceID}" +
                $"\n\t\tDescription={description}";

            message.Dump(ConsoleColor.Yellow);
        }
    }
}
