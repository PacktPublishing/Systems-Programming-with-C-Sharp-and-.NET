namespace _02Streams;

internal class PathReader
{
    public void DumpImages()
    {
        var imagesPath =
            Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        var allFiles =
            Directory.GetFiles(
                imagesPath,
                "*.jPg",
                SearchOption.AllDirectories);

        foreach (var file in allFiles) Console.WriteLine(file);
    }

    public void GetDirectoryInformation()
    {
        var imagesPath =
            Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

        var directoryInfo = new DirectoryInfo(imagesPath);
        Console.WriteLine(directoryInfo.FullName);
        Console.WriteLine(directoryInfo.CreationTime);
        Console.WriteLine(directoryInfo.Attributes);
    }
}