using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02Streams
{
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

            using (FileStream fs = File.OpenRead(fileName))
            {
                fileContent = new byte[fs.Length];
                int i = 0;
                int bytesRead=0;
                do
                {
                    var myBuffer = new byte[1];
                    bytesRead = fs.Read(myBuffer, 0, 1);
                    if(bytesRead > 0)
                        fileContent[i++] = myBuffer[0];
                }while(bytesRead > 0);

                fs.Close();

            }

            return Encoding.ASCII.GetString(fileContent);
        }
    }
}
