using System.Timers;
using Timer = System.Timers.Timer;


namespace _09_SerialMonitor.Fakes;

internal class FakeComPortWatcher : IComPortWatcher
{
    private bool _deviceIsAvailable = false;
    private Timer? _timer;

    public void Dispose()
    {
        _timer?.Dispose();
    }

    public event EventHandler<ComPortChangedEventArgs>? ComportAddedEvent;
    public event EventHandler<ComPortChangedEventArgs>? ComportDeletedEvent;

    public void Start()
    {
        _timer = new Timer(2000);
        _timer.Elapsed += (sender, args) =>
        {
            // Trigger the event every second
            if (_deviceIsAvailable)
            {
                ComportDeletedEvent?.Invoke(this, new ComPortChangedEventArgs("COM4"));
            }
            else
            {
                ComportAddedEvent?.Invoke(this, new ComPortChangedEventArgs("COM4"));
            }

            _deviceIsAvailable = !_deviceIsAvailable;

        };
        
        
        _timer.Start();
    }


    public void Stop()
    {
        _timer?.Stop();
        _timer?.Dispose();
        _timer = null;
        
    }

    public string FindMatchingComPort(string partialMatch)
    {
        return _deviceIsAvailable ? "COM4" : string.Empty;
    }
}
