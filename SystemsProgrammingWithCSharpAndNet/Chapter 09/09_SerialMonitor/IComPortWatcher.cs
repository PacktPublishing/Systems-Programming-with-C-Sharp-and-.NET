namespace _09_SerialMonitor;

internal interface IComPortWatcher : IDisposable
{
    event EventHandler<ComPortChangedEventArgs>? ComportAddedEvent;
    event EventHandler<ComPortChangedEventArgs>? ComportDeletedEvent;
    void Start();
    void Stop();

    string GetAvailableComPorts();
}