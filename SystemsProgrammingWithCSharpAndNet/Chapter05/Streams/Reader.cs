using System.Text;

namespace Streams;

internal class Reader
{
    public string ReadFromFile(string fileName)
    {
        var text = File.ReadAllText(fileName);
        return text;
    }

    public string ReadWithStream(string fileName)
    {
        byte[] fileContent;

        using (var fs = File.OpenRead(fileName))
        {
            fileContent = new byte[fs.Length];
            var i = 0;
            var bytesRead = 0;
            do
            {
                var myBuffer = new byte[1];
                bytesRead = fs.Read(myBuffer, 0, 1);
                if (bytesRead > 0)
                    fileContent[i++] = myBuffer[0];
            } while (bytesRead > 0);

            fs.Close();
        }

        return Encoding.ASCII.GetString(fileContent);
    }
}