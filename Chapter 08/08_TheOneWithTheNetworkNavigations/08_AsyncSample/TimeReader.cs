using System.Net.Sockets;
using System.Net;

namespace _08_AsyncSample
{
    internal class TimeReader
    {
        public DateTime SyncGetNetworkTime(string ntpServer = "pool.ntp.org")
        {
            // NTP message size - 16 bytes (RFC 2030)
            var ntpData = new byte[48];

            // Setting the Leap Indicator, Version Number and Mode values
            ntpData[0] = 0x23; // LI, Version, Mode

            var addresses = Dns.GetHostEntry(ntpServer);
            var ipEndPoint = new IPEndPoint(addresses.AddressList[0], 123); // NTP uses port 123

            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            {
                socket.Connect(ipEndPoint);
                socket.Send(ntpData);
                socket.Receive(ntpData);
                socket.Close();
            }

            return ConvertNtpTimeToDateTime(ntpData);
        }


        public async Task<DateTime> ProperGetNetworkTimeAsync(string ntpServer = "pool.ntp.org")
        {
            // NTP message size - 16 bytes (RFC 2030)
            var ntpData = new byte[48];

            // Setting the Leap Indicator, Version Number and Mode values
            ntpData[0] = 0x23; // LI, Version, Mode

            var addresses = await Dns.GetHostEntryAsync(ntpServer);
            var ipEndPoint = new IPEndPoint(addresses.AddressList[0], 123); // NTP uses port 123

            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            {
                await socket.ConnectAsync(ipEndPoint); // Now using ConnectAsync

                // Using SendAsync and ReceiveAsync requires a bit more setup, specifically the use of SocketAsyncEventArgs
                var sendEventArgs = new SocketAsyncEventArgs();
                sendEventArgs.SetBuffer(ntpData, 0, ntpData.Length);
                var receiveEventArgs = new SocketAsyncEventArgs();
                var receiveBuffer = new byte[48];
                receiveEventArgs.SetBuffer(receiveBuffer, 0, receiveBuffer.Length);

                // Wait for the send operation to complete
                await Task.Factory.FromAsync(socket.BeginSend(ntpData, 0, ntpData.Length, SocketFlags.None, null, socket), socket.EndSend);

                // Wait for the receive operation to complete
                await Task.Factory.FromAsync(socket.BeginReceive(receiveBuffer, 0, receiveBuffer.Length, SocketFlags.None, null, socket), socket.EndReceive);

                ntpData = receiveEventArgs.Buffer; // Update ntpData with the received data
            }

            return ConvertNtpTimeToDateTime(ntpData);
        }

        DateTime ConvertNtpTimeToDateTime(byte[] ntpData)
        {
            //Offset to get to the "Transmit Timestamp" field (time at the server)
            const byte serverReplyTime = 40;

            //Get the seconds part
            ulong intPart = BitConverter.ToUInt32(ntpData, serverReplyTime);

            //Get the seconds fraction
            ulong fractPart = BitConverter.ToUInt32(ntpData, serverReplyTime + 4);

            //Convert From big-endian to little-endian
            intPart = SwapEndian(intPart);
            fractPart = SwapEndian(fractPart);

            ulong milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);

            //**Time is January 1, 1900
            DateTime networkDateTime = (new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddMilliseconds((long)milliseconds);

            return networkDateTime.ToLocalTime();
        }


        uint SwapEndian(ulong x)
        {
            return (uint)(((x & 0x000000ff) << 24) +
                          ((x & 0x0000ff00) << 8) |
                          ((x & 0x00ff0000) >> 8) |
                          ((x & 0xff000000) >> 24));
        }
        
        
    }
}
