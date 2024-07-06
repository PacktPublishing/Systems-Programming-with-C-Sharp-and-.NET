namespace _02Streams;

public class FileCreatedEventArgs : EventArgs
{
    public FileCreatedEventArgs(string filePath)
    {
        FilePath = filePath;
    }

    public string FilePath { get; }
}