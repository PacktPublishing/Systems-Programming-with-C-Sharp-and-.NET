using HelperClasses;

namespace _02Streams;

internal class MyFolderWatcherWithFilteredCallback : IDisposable
{
    private static readonly TimeSpan ReadDelay = TimeSpan.FromSeconds(1);
    private readonly Dictionary<string, DateTime> _lastRead = new();
    private readonly object _lockObject = new();
    private Timer? _cleanupTimer;

    private FileSystemWatcher? _watcher;

    #region

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    #endregion

    // Create an event that is raised when a file has been added
    public event EventHandler<FileCreatedEventArgs>? FileAdded;
    public event EventHandler<FileChangedEventArgs>? FileChanged;

    public void SetupWatcher(string folderToWatch)
    {
        // Set the folder to keep an eye on
        _watcher = new FileSystemWatcher(folderToWatch);
        // We only want notifications when a file is created or when it has changed .
        _watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite;
        // Set the callbacks
        _watcher.Created += WatcherCallback;
        _watcher.Changed += WatcherCallback;
        // Start watching
        _watcher.EnableRaisingEvents = true;

        // Now, we need to set up the cleaning thread.
        _cleanupTimer = new Timer(
            _ => CleanupOldCalls(), 
            null, 
            TimeSpan.Zero, 
            ReadDelay);
        
    }

    private void CleanupOldCalls()
    {
        if (_watcher == null) return;
        lock (_lockObject)
        {
            var keysToRemove = _lastRead
                .Where(x => x.Value.Add(ReadDelay) < DateTime.Now)
                .Select(x => x.Key)
                .ToList();
            $"Items in the list: {keysToRemove.Count}".Dump(ConsoleColor.Magenta, ConsoleColor.DarkMagenta);

            foreach (var key in keysToRemove)
                _lastRead.Remove(key);
        }
    }

    private void WatcherCallback(object sender, FileSystemEventArgs e)
    {
        var lastWriteTime = File.GetLastWriteTime(e.FullPath);

        switch (e.ChangeType)
        {
            case WatcherChangeTypes.Created:
                lock (_lockObject)
                {
                    FileAdded?.Invoke(this, new FileCreatedEventArgs(e.FullPath));
                    _lastRead[e.FullPath] = lastWriteTime;
                }

                break;

            case WatcherChangeTypes.Changed:
                lock (_lockObject)
                {
                    if (_lastRead.ContainsKey(e.FullPath)
                        && _lastRead[e.FullPath].Add(ReadDelay) > lastWriteTime)
                        break;

                    FileChanged?.Invoke(
                        this,
                        new FileChangedEventArgs(e.FullPath));
                    _lastRead[e.FullPath] = lastWriteTime;
                }
                break;
        }
    }

    // Cleaning up.
    private void Dispose(bool disposing)
    {
        if (!disposing) return;
        if (_watcher == null) return;

        _cleanupTimer?.Dispose();
        _cleanupTimer = null;

        _watcher.EnableRaisingEvents = false;

        FileAdded = null;
        FileChanged = null;

        _watcher.Dispose();
        _watcher = null;
    }

    ~MyFolderWatcherWithFilteredCallback()
    {
        Dispose(false);
    }
}