using System.Globalization;
using System.IO.Ports;
using System.Management;
using System.Text.RegularExpressions;

#pragma warning disable CA1416

namespace _09_SerialMonitor
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private ManagementEventWatcher _comPortInsertedWatcher;
        private readonly ManagementEventWatcher _comPortDeletedWatcher;

        private bool _deviceIsAvailable = false;
        private string _comPortName = string.Empty;

        private bool _serialConnected = false;
        private SerialPort? _serial;
        
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            
            GetAvailableUsbDevices();
            
            string queryInsert = "SELECT * FROM __InstanceCreationEvent WITHIN 1 " +
                           "WHERE TargetInstance ISA 'Win32_PnPEntity' " +
                           "AND TargetInstance.Caption  LIKE '%Arduino%'";
            string queryDelete = "SELECT * FROM __InstanceDeletionEvent WITHIN 1 " +
                            "WHERE TargetInstance ISA 'Win32_PnPEntity' " +
                            "AND TargetInstance.Caption  LIKE '%Arduino%'";
            
            _comPortInsertedWatcher = new ManagementEventWatcher(queryInsert);
            _comPortInsertedWatcher.EventArrived += HandleInsertEvent;
            _comPortInsertedWatcher.Start();
            
            _comPortDeletedWatcher = new ManagementEventWatcher(queryDelete);
            _comPortDeletedWatcher.EventArrived += HandleDeleteEvent;
            _comPortDeletedWatcher.Start();
        }

        private void HandleDeleteEvent(object sender, EventArrivedEventArgs e)
        {
            var newInstance = e.NewEvent["TargetInstance"] as ManagementBaseObject;
            _logger.LogInformation("COM port removed: {0}", newInstance["Caption"]);
            _deviceIsAvailable = false;
            _comPortName = string.Empty;
        }

        private void HandleInsertEvent(object sender, EventArrivedEventArgs e)
        {
            var newInstance = e.NewEvent["TargetInstance"] as ManagementBaseObject;
            _logger.LogInformation("New COM port detected: {0}", newInstance["Caption"]);
            // Get the matching COM port

            _comPortName= GetComPortName(newInstance["Caption"].ToString());

            //SerialPort port = new SerialPort("COM4", 9600);
            _deviceIsAvailable = true;
            
        }

        private string GetComPortName(string foundCaption)
        {
            string foundComPort = string.Empty;
            var regExPattern = @"(COM\d+)";
            var match = Regex.Match(foundCaption, regExPattern);
            if (match.Success)
            {
                var comPort = match.Groups[1].Value;
                _logger.LogInformation("COM port: {0}", comPort);
                foundComPort= match.Value;
            }

            return foundComPort;
        }

        private void GetAvailableUsbDevices()
        {
            //var searcher = new ManagementObjectSearcher(@"Select * From Win32_PnPEntity Where DeviceID Like 'USB%'");
            var searcher = new ManagementObjectSearcher(@"Select * From Win32_PnPEntity Where Caption Like '%Arduino%'");
            var devices = searcher.Get();
            
            _deviceIsAvailable = (devices.Count > 0);
            // Get the matching COM port
            var firstDevice = devices.Cast<ManagementObject>().First();
            _comPortName = GetComPortName(firstDevice["Caption"].ToString());

        }

        private void StartSerialConnection()
        {
            _serial = new SerialPort(_comPortName, 9600);
            _serialConnected = true;
            _serial.Open();
            _serial.DataReceived += SerialOnDataReceived;
        }

        private void StopSerialConnection()
        {
            if (_serial == null) return;

            if (_serial is { IsOpen: true })
                _serial.Close();

            _serial.DataReceived -= SerialOnDataReceived;
            _serial.Dispose();
            _serial = null;
        }
        private void SerialOnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //if (_logger.IsEnabled(LogLevel.Information))
                //{
                //    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                //}

                if (_deviceIsAvailable)
                {
                    
                }
                await Task.Delay(10, stoppingToken);
                
            }
        }

        public override void Dispose()
        {
            _comPortInsertedWatcher.Stop();
            _comPortInsertedWatcher.Dispose();
            
            _comPortDeletedWatcher.Stop();
            _comPortDeletedWatcher.Dispose();

            base.Dispose();
            
        }
    }
}
