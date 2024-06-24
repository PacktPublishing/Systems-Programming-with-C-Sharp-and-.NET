

using ExtensionLibrary;

ConsoleColor consoleColor = ConsoleColor.Red;
if (OperatingSystem.IsWindows())
{
    consoleColor = ConsoleColor.Cyan;
    "Worker running on Windows".Dump(consoleColor);
}
else if (OperatingSystem.IsLinux())
{
    consoleColor = ConsoleColor.Blue;
    "Worker running on Linux".Dump(consoleColor);
}

var directorySeparatorChar = Path.DirectorySeparatorChar;
var pathSeparator = Path.PathSeparator;

var currentPath = Directory.GetCurrentDirectory();
var newPath = currentPath + directorySeparatorChar + "newFolder";
var betterWay = Path.Combine(currentPath, "newFolder");

var twoPaths = currentPath + pathSeparator + newPath;

$"DirectorySeparatorChar: {directorySeparatorChar}".Dump(consoleColor);
$"PathSeparator: {pathSeparator}".Dump(consoleColor);
$"Current Path: {currentPath}".Dump(consoleColor);
$"newPath: {newPath}".Dump(consoleColor);
$"betterWay: {betterWay}".Dump(consoleColor);
$"twoPaths: {twoPaths}".Dump(consoleColor);
$"End of the output: {Environment.NewLine}".Dump(consoleColor);

