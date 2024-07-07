namespace SerialMonitor;

public interface IComPortWatcher : IDisposable
{
    event EventHandler<ComPortChangedEventArgs>? ComportAddedEvent;
    event EventHandler<ComPortChangedEventArgs>? ComportDeletedEvent;
    void Start();
    void Stop();
    string FindMatchingComPort(string partialMatch);
}