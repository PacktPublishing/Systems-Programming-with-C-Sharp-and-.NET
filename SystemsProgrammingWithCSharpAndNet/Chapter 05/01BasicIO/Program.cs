var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
var fileName = "WriteLines.txt";
var fullPath = Path.Combine(path, fileName);
File.WriteAllText(fullPath, "Hello, System Programmers");
