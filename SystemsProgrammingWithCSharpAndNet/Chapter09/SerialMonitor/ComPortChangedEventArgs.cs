namespace _09_SerialMonitor;

public class ComPortChangedEventArgs(string comPortName) : EventArgs
{
    public string? ComPortName { get; set; } = comPortName;
}