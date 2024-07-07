using System.Management;
using System.Text.RegularExpressions;

#pragma warning disable CA1416

namespace SerialMonitor;

internal class ComPortWatcher : IComPortWatcher
{
    private ManagementEventWatcher? _comPortDeletedWatcher;
    private ManagementEventWatcher? _comPortInsertedWatcher;

    private bool _isRunning;

    public event EventHandler<ComPortChangedEventArgs>? ComportAddedEvent;
    public event EventHandler<ComPortChangedEventArgs>? ComportDeletedEvent;

    public void Dispose()
    {
        DeleteWatchers();
    }

    public void Start()
    {
        if (_isRunning)
            return;

        var queryInsert = "SELECT * FROM __InstanceCreationEvent WITHIN 1 " +
                          "WHERE TargetInstance ISA 'Win32_PnPEntity' " +
                          "AND TargetInstance.Caption  LIKE '%Arduino%'";

        var queryDelete = "SELECT * FROM __InstanceDeletionEvent WITHIN 1 " +
                          "WHERE TargetInstance ISA 'Win32_PnPEntity' " +
                          "AND TargetInstance.Caption  LIKE '%Arduino%'";

        _comPortInsertedWatcher = new ManagementEventWatcher(queryInsert);
        _comPortInsertedWatcher.EventArrived += HandleInsertEvent;
        _comPortInsertedWatcher.Start();

        _comPortDeletedWatcher = new ManagementEventWatcher(queryDelete);
        _comPortDeletedWatcher.EventArrived += HandleDeleteEvent;
        _comPortDeletedWatcher.Start();

        _isRunning = true;
    }

    public void Stop()
    {
        if (!_isRunning)
            return;

        DeleteWatchers();

        _isRunning = false;
    }

    public string FindMatchingComPort(string partialMatch)
    {
        string comPortName;

        var searcher = new ManagementObjectSearcher(
            @$"Select * From Win32_PnPEntity Where Caption Like '%{partialMatch}%'");
        var devices = searcher.Get();

        var deviceIsAvailable = devices.Count > 0;
        if (deviceIsAvailable)
        {
            var firstDevice = devices.Cast<ManagementObject>().First();
            comPortName = GetComPortName(firstDevice["Caption"].ToString());
        }
        else
        {
            comPortName = string.Empty;
        }

        return comPortName;
    }

    private void DeleteWatchers()
    {
        if (_comPortInsertedWatcher != null)
        {
            _comPortInsertedWatcher.EventArrived -= HandleInsertEvent;
            _comPortInsertedWatcher.Stop();
            _comPortInsertedWatcher.Dispose();
            _comPortInsertedWatcher = null;
        }

        if (_comPortDeletedWatcher != null)
        {
            _comPortDeletedWatcher.EventArrived -= HandleDeleteEvent;
            _comPortDeletedWatcher.Stop();
            _comPortDeletedWatcher.Dispose();
            _comPortDeletedWatcher = null;
        }
    }

    private void HandleInsertEvent(object sender, EventArrivedEventArgs e)
    {
        var newInstance = e.NewEvent["TargetInstance"] as ManagementBaseObject;

        var comPortName = GetComPortName(newInstance["Caption"].ToString());
        Task.Run(() => ComportAddedEvent?.Invoke(this, new ComPortChangedEventArgs(comPortName)));
    }

    private void HandleDeleteEvent(object sender, EventArrivedEventArgs e)
    {
        var newInstance = e.NewEvent["TargetInstance"] as ManagementBaseObject;
        var caption = newInstance?["Caption"].ToString();

        var comPortName = GetComPortName(caption);
        Task.Run(() => ComportDeletedEvent?.Invoke(this, new ComPortChangedEventArgs(comPortName)));
    }

    private string GetComPortName(string foundCaption)
    {
        var foundComPort = string.Empty;
        var regExPattern = @"(COM\d+)";
        var match = Regex.Match(foundCaption, regExPattern);
        if (match.Success)
        {
            var comPort = match.Groups[1].Value;
            foundComPort = match.Value;
        }

        return foundComPort;
    }
}