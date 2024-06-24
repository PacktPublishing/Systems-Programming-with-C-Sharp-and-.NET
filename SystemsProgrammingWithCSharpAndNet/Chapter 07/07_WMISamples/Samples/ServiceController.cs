using System.Management;
using ExtensionLibrary;

// This disables the warning about this code only being available on Windows.
#pragma warning disable CA1416

namespace _07_WMISamples.Samples
{
    internal class ServiceController
    {
        public void ControlService()
        {
            // Define the service. In this case,
            // we're using the Windows Update service
            string serviceName = "wuauserv"; 
            
            // Define the query to get the service
            string queryString = $"SELECT * FROM Win32_Service WHERE Name = '{serviceName}'";
            
            // Create a query to get the specified service
            ManagementObjectSearcher searcher =
                new ManagementObjectSearcher(queryString);

            // Execute the query
            foreach (var o in searcher.Get())
            {
                var service = (ManagementObject)o;
                // Check the service state before trying to stop it
                if (service["State"].ToString().ToLower() == "running")
                {
                    // Stop the service
                    service.InvokeMethod("StopService", null);

                    // Wait a bit for the service to stop
                    System.Threading.Thread.Sleep(2000); 

                    // Start the service again
                    service.InvokeMethod("StartService", null);
                    $"{serviceName} service restarted successfully.".Dump(ConsoleColor.Cyan);
                }
            }
        }
    }
}
