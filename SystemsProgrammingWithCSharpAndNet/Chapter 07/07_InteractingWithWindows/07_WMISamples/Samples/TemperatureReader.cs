using System.Management;
using ExtensionLibrary;

// This disables the warning about this code only being available on Windows.
#pragma warning disable CA1416


namespace _07_WMISamples.Samples;

internal class TemperatureReader
{
    public void ReadTemperaturesUsingMsAcpi()
    {
        var scope = "root\\WMI";
        var query = "SELECT * FROM MSAcpi_ThermalZoneTemperature";
        var searcher = new ManagementObjectSearcher(scope, query);
        try
        {
            foreach (var o in searcher.Get())
            {
                var obj = (ManagementObject)o;
                var temperature = Convert.ToDouble(obj["CurrentTemperature"]) / 10 - 273.15;
                $"CPU Temperature: {temperature}°C".Dump();
            }
        }
        catch (ManagementException)
        {
            "Unfortunately, your BIOS does not support this API.".Dump(ConsoleColor.Red);
        }
    }
}