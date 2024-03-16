using System.IO.Ports;

namespace _09_SerialMonitor;

internal class AsyncSerial : IAsyncSerial
{
    private bool _isOpen;
    private SerialPort? _serialPort;

    public void Open(
        string portName,
        int baudRate = 9600,
        Parity parity = Parity.None,
        int dataBits = 8,
        StopBits stopBits = StopBits.One)
    {
        if (_isOpen) throw new InvalidOperationException("Serial port is already open");

        _serialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
        _serialPort.Open();

        _isOpen = true;
    }

    public void Close()
    {
        if (!_isOpen) throw new InvalidOperationException("Serial port is not open");

        _serialPort?.Close();
        _serialPort?.Dispose();
        _serialPort = null;
    }

    public Task<byte> ReadByteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(() =>
        {
            if (!_isOpen) throw new InvalidOperationException("Serial port is not open");
            var buffer = new byte[1];
            try
            {
                _serialPort?.Read(buffer, 0, 1);
            }
            catch (OperationCanceledException)
            {
                // This happens when the device has been unplugged
                // We pass it a 0xFF to indicate that the device is no longer available
                buffer[0] = 255;
            }

            return buffer[0];
        }, stoppingToken);
    }
}