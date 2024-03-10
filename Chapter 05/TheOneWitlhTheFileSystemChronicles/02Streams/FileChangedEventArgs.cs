namespace _02Streams;

public class FileChangedEventArgs : EventArgs
{
    public FileChangedEventArgs(string filePath)
    {
        FilePath = filePath;
    }

    public string FilePath { get; }
}