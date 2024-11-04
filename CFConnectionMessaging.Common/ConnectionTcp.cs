using CFConnectionMessaging.Models;
using CFConnectionMessaging.Utilities;
using System.Net;
using System.Net.Sockets;

namespace CFConnectionMessaging
{
    /// <summary>
    ///  Connection for ConnectionMessage instances with transport via TCP.
    /// </summary>
    public class ConnectionTcp : ConnectionSocketBase
    {
        private Mutex _mutex = new Mutex();        
        private Thread? _listenerThread;
        private Thread? _receiveThread;

        private int _listenPort;

        private CancellationTokenSource? _cancellationTokenSource;        

        private class ClientInfo
        {
            public TcpClient? TcpClient { get; set; }

            public NetworkStream? Stream { get; set; }
            
        }

        private List<ClientInfo> _clientInfos = new List<ClientInfo>();    

        // Event handler for connection messages
        public delegate void ConnectionMessageReceived(ConnectionMessage connectionMessage, MessageReceivedInfo messageReceivedInfo);
        public event ConnectionMessageReceived? OnConnectionMessageReceived;

        //private int _receivePort = 11000;       // Default

        //public int ReceivePort
        //{
        //    get { return _receivePort; }
        //    set { _receivePort = value; }
        //}

        public void StartListening()
        {
            _cancellationTokenSource = new CancellationTokenSource();

            // Start listener thread
            _listenPort = ReceivePort;
            _listenerThread = new Thread(ListenerWorker);
            _listenerThread.Start();

            // Start receive thread
            _receiveThread = new Thread(ReceiveWorker);
            _receiveThread.Start();
        }

        public void StopListening()
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
            }

            // Wait for listening thread to exit
            if (_listenerThread != null)
            {
                _listenerThread.Join();
                _listenerThread = null;
            }

            // Wait for receive thread to exist
            if (_receiveThread != null)
            {
                _receiveThread.Join();
                _receiveThread = null;
            }
        }

        /// <summary>
        /// Worker thread that listens for connection requests
        /// </summary>
        public void ListenerWorker()
        {
            // Start listening
            var tcpLisener = new TcpListener(System.Net.IPAddress.Any, _listenPort);
            tcpLisener.Start();

            // Run until cancelled
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                try
                {
                    // Accept TCP client
                    var tcpClient = AcceptTcpClientAsync(tcpLisener).Result;
                    if (tcpClient != null)
                    {
                        var clientInfo = new ClientInfo()
                        {
                            TcpClient = tcpClient,
                            Stream = tcpClient.GetStream()
                        };
                        _clientInfos.Add(clientInfo);

                        IPEndPoint remoteEndpoint = clientInfo.TcpClient.Client.RemoteEndPoint as IPEndPoint;
                        Console.WriteLine($"Client {remoteEndpoint.Address.ToString()}:{remoteEndpoint.Port} connected");
                    }
                }
                catch(SocketException socketException)
                {
                    System.Diagnostics.Debug.WriteLine($"Error accepting TCP client: {socketException.Message}");
                    Thread.Sleep(500);
                }

                System.Threading.Thread.Yield();
            }
        }

        private async Task<TcpClient?> AcceptTcpClientAsync(TcpListener tcpListener)
        {
            return await tcpListener.AcceptTcpClientAsync(_cancellationTokenSource.Token);
        }

        /// <summary>
        /// Worker thread that receives packets of data        
        /// </summary>
        public void ReceiveWorker()
        {
            var receiveTasks = new List<Task>();

            // Run until cancelled
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                // Receive data from clients
                foreach(var clientInfo in _clientInfos.Where(ci => ci.Stream.DataAvailable))
                {
                    var receiveTask = ReceiveAsync(clientInfo);
                    receiveTasks.Add(receiveTask);
                }
                System.Threading.Thread.Sleep(5);

                // Wait for receive complete
                if (receiveTasks.Any())
                {
                    Task.WaitAll(receiveTasks.ToArray());
                    receiveTasks.Clear();                    
                }
                System.Threading.Thread.Sleep(5);

                // Process packets
                if (_packets.Any())
                {
                    ProcessPackets();
                }

                System.Threading.Thread.Sleep(5);
            }
        }       

        /// <summary>
        /// Receives from client asynchronously
        /// </summary>
        /// <param name="clientInfo"></param>
        /// <returns></returns>
        private Task ReceiveAsync(ClientInfo clientInfo)
        {
            return Task.Factory.StartNew(() =>
            {
                var data = new byte[1024 * 50];     // Use same array for all packets. No need to reset between packets
                while (clientInfo.Stream.DataAvailable)
                {
                    var byteCount = clientInfo.Stream.Read(data, 0, data.Length);
                    if (byteCount > 0)
                    {
                        // Add packet to queue
                        IPEndPoint remoteEndpoint = clientInfo.TcpClient.Client.RemoteEndPoint as IPEndPoint;
                        var packet = new Packet()
                        {
                            Endpoint = new EndpointInfo()
                            {
                                Ip = remoteEndpoint.Address.ToString(),
                                Port = remoteEndpoint.Port = remoteEndpoint.Port
                            },
                            Data = new byte[byteCount]
                        };
                        Buffer.BlockCopy(data, 0, packet.Data, 0, byteCount);

                        _mutex.WaitOne();
                        _packets.Add(packet);
                        _mutex.ReleaseMutex();

                        System.Diagnostics.Debug.WriteLine($"Packet received from {packet.Endpoint.Ip}:{packet.Endpoint.Port} ({packet.Data.Length} bytes)");
                    }
                    Thread.Sleep(5);
                }
            });
        }

        /// <summary>
        /// Sends message
        /// </summary>
        /// <param name="connectionMessage"></param>
        /// <param name="remoteEndpointInfo"></param>
        public void SendMessage(ConnectionMessage connectionMessage, EndpointInfo remoteEndpointInfo)
        {
            // Get ClientInfo for remote endpoint
            var clientInfo = GetClientInfoByRemoteEndpoint(remoteEndpointInfo);

            // If no connection then connect
            if (clientInfo == null)
            {                
                System.Diagnostics.Debug.WriteLine($"Connecting to {remoteEndpointInfo.Ip}:{remoteEndpointInfo.Port}");
                var tcpClient = new TcpClient();
                tcpClient.Connect(IPAddress.Parse(remoteEndpointInfo.Ip), remoteEndpointInfo.Port);
                System.Diagnostics.Debug.WriteLine($"Connected to {remoteEndpointInfo.Ip}:{remoteEndpointInfo.Port}");

                clientInfo = new ClientInfo()
                {
                    TcpClient = tcpClient,
                    Stream = tcpClient.GetStream()
                };
                _clientInfos.Add(clientInfo);
            }

            // Serialize message
            var data = InternalUtilities.Serialise(connectionMessage);

            // Send data
            clientInfo.TcpClient.Client.Send(data);                       
        }

        /// <summary>
        /// Gets ClientInfo by remote endpoint
        /// </summary>
        /// <param name="endpointInfo"></param>
        /// <returns></returns>
        private ClientInfo? GetClientInfoByRemoteEndpoint(EndpointInfo endpointInfo)
        {                    
            foreach (var clientInfo in _clientInfos)
            {
                IPEndPoint clientRemoteEndpoint = clientInfo.TcpClient.Client.RemoteEndPoint as IPEndPoint;               
                if (clientRemoteEndpoint.Address.ToString() == endpointInfo.Ip &&
                    clientRemoteEndpoint.Port == endpointInfo.Port)
                {
                    return clientInfo;                    
                }
            }

            return null;
        }

        protected override void ConnectMessageReceived(ConnectionMessage connectionMessage, MessageReceivedInfo messageReceivedInfo)
        {
            if (OnConnectionMessageReceived != null)
            {
                OnConnectionMessageReceived(connectionMessage, messageReceivedInfo);
            }
        }
    }
}
