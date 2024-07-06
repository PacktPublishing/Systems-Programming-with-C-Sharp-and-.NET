using System.Collections.Concurrent;
using System.Net.Sockets;

namespace ConnectionPooling
{
    internal class TcpClientConnectionPool : IAsyncDisposable
    {
        private readonly ConcurrentBag<TcpClient?> _availableConnections = new();
        private readonly int _maxPoolSize = 10; // Example pool size
        public TcpClient? GetConnection()
        {
            if (_availableConnections.TryTake(out TcpClient? client))
                return client;
            if (_availableConnections.Count < _maxPoolSize)
            {
                // Create a new connection if the pool is not full
                client = new TcpClient("my.server.com", 443);
            }
            else
            {
                // Pool is full, wait for an available connection or throw an exception
                // This strategy depends on your specific requirements
                throw new Exception("Connection pool limit reached.");
            }

            return client;
        }

        public void ReturnConnection(TcpClient? client)
        {
            // Check the state of the connection to ensure it's still valid
            if (client is { Connected: true })
            {
                _availableConnections.Add(client);
            }
            else
            {
                // Optionally, handle the case where the connection is no longer valid
                // e.g., reconnect or simply discard this connection
            }
        }

        public async ValueTask DisposeAsync()
        {
            foreach (var client in _availableConnections)
            {
                if (client is { Connected: true })
                {
                    await client.GetStream().DisposeAsync();
                }
                client?.Close();
                client?.Dispose();
            }
        }
    }
}
// Usage example:
// TcpConnectionPool pool = new TcpConnectionPool();
// using (TcpClient client = pool.GetConnection())
// {
//     // Use the client for network operations
// }
// pool.ReturnConnection(client);
