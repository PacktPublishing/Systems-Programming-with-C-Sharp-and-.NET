using ExtensionLibrary;

#pragma warning disable CA1416

namespace _09_SerialMonitor;

public class Worker : BackgroundService
{
    private readonly IComPortWatcher _comPortWatcher;
    private readonly ILogger<Worker> _logger;

    private readonly IAsyncSerial _serial;

    private string _comPortName;
    private bool _deviceIsAvailable;


    public Worker(ILogger<Worker> logger, IAsyncSerial serial, IComPortWatcher comPortWatcher)
    {
        _logger = logger;
        _serial = serial;
        _comPortWatcher = comPortWatcher;

        _comPortName = _comPortWatcher.GetAvailableComPorts();
        _deviceIsAvailable = !string.IsNullOrWhiteSpace(_comPortName);

        _comPortWatcher.ComportAddedEvent += HandleInsertEvent;
        _comPortWatcher.ComportDeletedEvent += HandleDeleteEvent;
        _comPortWatcher.Start();

        if (_deviceIsAvailable) StartSerialConnection();
    }


    private void HandleInsertEvent(object? sender, ComPortChangedEventArgs e)
    {
        _comPortName = e.ComPortName;

        _logger.LogInformation($"New COM port detected: {_comPortName}");
        if (!string.IsNullOrEmpty(_comPortName)) StartSerialConnection();
    }

    private void HandleDeleteEvent(object? sender, ComPortChangedEventArgs e)
    {
        StopSerialConnection();

        _logger.LogInformation($"COM port removed: {e.ComPortName}");

        _comPortName = string.Empty;
        _deviceIsAvailable = false;
    }

    private void StartSerialConnection()
    {
        if (_serial.IsOpen)
            return;

        //if (_serial != null) return;

        //_serial = new AsyncSerial();
        _serial.Open(_comPortName);

        _deviceIsAvailable = true;
    }

    private void StopSerialConnection()
    {
        _deviceIsAvailable = false;
        if (!_serial.IsOpen)
            return;

        //if (_serial == null) return;
        _serial.Close();
        //_serial = null;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        "Before the loop".Dump();

        while (!stoppingToken.IsCancellationRequested)
        {
            if (_deviceIsAvailable)
            {
                var receivedByte = await _serial?.ReadByteAsync(stoppingToken);
                if (receivedByte == 0xFF)
                {
                    StopSerialConnection();
                    _logger.LogWarning("Device is ejected.");
                }
                else
                {
                    _logger.LogInformation($"Data received: {receivedByte:X}");
                }
            }

            await Task.Delay(10, stoppingToken);
        }
    }

    public override void Dispose()
    {
        _comPortWatcher.Stop();
        _comPortWatcher.ComportAddedEvent -= HandleInsertEvent;
        _comPortWatcher.ComportDeletedEvent -= HandleDeleteEvent;
        _comPortWatcher.Dispose();
        base.Dispose();
    }
}