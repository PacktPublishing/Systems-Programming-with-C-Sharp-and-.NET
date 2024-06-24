// See https://aka.ms/new-console-template for more information

using _07_WMISamples.Samples;
using ExtensionLibrary;

//TemperatureReader temperatureReader = new TemperatureReader();
//temperatureReader.ReadTemperaturesUsingMsAcpi();

//BIOSReader biosReader = new BIOSReader();
//biosReader.ReadBIOSDetails();

//ServiceController serviceController = new ServiceController();
//serviceController.ControlService();

USBWarning usbWarning = new USBWarning();
usbWarning.StartListening();
