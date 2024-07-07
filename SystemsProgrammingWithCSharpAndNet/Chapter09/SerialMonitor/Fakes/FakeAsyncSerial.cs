using System.IO.Ports;

namespace SerialMonitor.Fakes;

internal class FakeAsyncSerial : IAsyncSerial
{
    public bool IsOpen { get; private set; }

    public void Open(string portName, int baudRate = 9600, Parity parity = Parity.None, int dataBits = 8,
        StopBits stopBits = StopBits.One)
    {
        // Nothing
        IsOpen = true;
    }

    public void Close()
    {
        // Nothing
        IsOpen = false;
    }

    public Task<byte> ReadByteAsync(CancellationToken stoppingToken)
    {
        return IsOpen ? Task.FromResult((byte)1) : Task.FromResult((byte)0);
    }
}