using System.Management;
using ExtensionLibrary;

// This disables the warning about this code only being available on Windows.
#pragma warning disable CA1416

namespace WMISamples.Samples
{
    internal class BIOSReader
    // ReSharper restore InconsistentNaming
    {
        public void ReadBIOSDetails()
        {
            // Create a management scope object
            ManagementScope scope = new ManagementScope("\\\\.\\ROOT\\cimv2");
            scope.Connect();
            // Query object for BIOS information
            ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_BIOS");
            using ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
            foreach (var o in searcher.Get())
            {
                var queryObj = (ManagementObject)o;
                "-----------------------------------".Dump(ConsoleColor.Yellow);
                "BIOS Information".Dump(ConsoleColor.Yellow);
                "-----------------------------------".Dump(ConsoleColor.Yellow);
                $"Manufacturer: {queryObj["Manufacturer"]}".Dump(ConsoleColor.Yellow);
                $"Name: {queryObj["Name"]}".Dump(ConsoleColor.Yellow);
                $"Version: {queryObj["Version"]}".Dump(ConsoleColor.Yellow);
            }
        }
    }
}
