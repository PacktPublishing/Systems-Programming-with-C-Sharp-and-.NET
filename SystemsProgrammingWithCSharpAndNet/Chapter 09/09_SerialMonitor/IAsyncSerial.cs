﻿using System.IO.Ports;

namespace _09_SerialMonitor;

internal interface IAsyncSerial
{
    void Open(string portName,
        int baudRate = 9600,
        Parity parity = Parity.None,
        int dataBits = 8,
        StopBits stopBits = StopBits.One);

    void Close();
    Task<byte> ReadByteAsync(CancellationToken stoppingToken);
}