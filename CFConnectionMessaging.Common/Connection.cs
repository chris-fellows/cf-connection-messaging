using CFConnectionMessaging.Models;
using CFConnectionMessaging.Utilities;
using System.Net.Sockets;
using System.Net;

namespace CFConnectionMessaging
{
    /// <summary>
    /// Connection for UDP data.
    /// 
    /// Notes:
    /// - Thread listens for packets and adds them to queue.
    /// - Thread processes queue packets and deserializes them in to messages.
    /// - This class is thread safe so that multiple clients can use it simultaneously.
    /// </summary>
    public class Connection
    {        
        private List<Packet> _packets = new List<Packet>();
        private Thread? _receiveThread;
        private Thread? _packetThread;
        private bool _listening;
        private Mutex _mutex = new Mutex();

        private CancellationTokenSource? _cancellationTokenSource;

        // Event handler for connection messages
        public delegate void ConnectionMessageReceived(ConnectionMessage connectionMessage, MessageReceivedInfo messageReceivedInfo);
        public event ConnectionMessageReceived? OnConnectionMessageReceived;

        private int _receivePort = 11000;       // Default

        public int ReceivePort
        {
            get { return _receivePort; }
            set { _receivePort = value; }
        }
            
        /// <summary>
        /// Starts listening for packets
        /// </summary>
        public void StartListening()
        {
            if (_listening)
            {
                throw new ArgumentException("Already listening");
            }
            if (ReceivePort <= 0)
            {
                throw new ArgumentException("ReceivePort must be set to a valid port");
            }

            _cancellationTokenSource = new CancellationTokenSource();
            _listening = true;

            _receiveThread = new Thread(ReceiveWorker);
            _receiveThread.Start();

            _packetThread = new Thread(PacketWorker);
            _packetThread.Start();            
        }

        public void StopListening()
        {
            if (_listening && _cancellationTokenSource != null)
            {
                _listening = false;
                _cancellationTokenSource.Cancel();

                // Wait for receive thread to exit
                _receiveThread!.Join();

                // Wait for packet thread to exit
                _packetThread!.Join();
            }
        }

        //public void StartTest()
        //{
        //    _thread = new Thread(Worker);
        //    _thread.Start();
        //}

        public void SendMessage(ConnectionMessage connectionMessage, EndpointInfo remoteEndpointInfo)
        {
            _mutex.WaitOne();

            // Serialize message
            var data = InternalUtilities.Serialise(connectionMessage);

            var client = new UdpClient();
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(remoteEndpointInfo.Ip), remoteEndpointInfo.Port); // endpoint where server is listening
            client.Connect(endpoint);
            client.Send(data);            

            _mutex.ReleaseMutex();
        }    

        /// <summary>
        /// Worker thread to process packets received
        /// </summary>
        public void PacketWorker()
        {
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                if (_packets.Any())
                {
                    ProcessPackets();
                }

                Thread.Sleep(50);
            }
        }

        /// <summary>
        /// Worker thread to receive packets
        /// </summary>
        public void ReceiveWorker()
        {         
            //int port = 11000;
            UdpClient udpClient = new UdpClient(ReceivePort);            

            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                /*
                var remoteEP = new IPEndPoint(IPAddress.Any, ReceivePort);
                //var data = udpServer.Receive(ref remoteEP); // listen on port 11000
                //var message = Encoding.UTF8.GetString(data);
                var packet = new Packet() {
                    Data = udpClient.Receive(ref remoteEP),
                    EndpointIP = remoteEP.ToString(),
                    EndpointPort = remoteEP.Port
                };
                */

                // Receive packet
                var result = udpClient.ReceiveAsync(_cancellationTokenSource.Token).Result;                

                if (!_cancellationTokenSource.IsCancellationRequested)
                {
                    var packet = new Packet()
                    {
                        Data = result.Buffer,
                        EndpointIP = result.RemoteEndPoint.Address.ToString(),
                        EndpointPort = result.RemoteEndPoint.Port
                    };
                    _packets.Add(packet);

                    Console.Write($"Received packet from {packet.EndpointIP}:{packet.EndpointPort}");
                }
               
                //Console.Write("receive data from " + remoteEP.ToString());
                //udpServer.Send(new byte[] { 1 }, 1, remoteEP); // reply back
            }
        }

        /// <summary>
        /// Process packets. Converts to ConnectionMessage, removes used packets, notifies client.       
        /// </summary>
        private void ProcessPackets()
        {
            if (_packets.Any())
            {
                // Get all distinct endpoints
                var endpoints = _packets.Select(p => $"{p.EndpointIP}\t{p.EndpointPort}").ToList();

                // Process packets for each endpoint
                foreach (var endpoint in endpoints)
                {
                    ProcessPackets(endpoint.Split('\t')[0], Convert.ToInt32(endpoint.Split('\t')[1]));
                }
            }
        }

        /// <summary>
        /// Process packets for endpoint
        /// </summary>
        private void ProcessPackets(string endpointIP, int endpointPort)
        {                            
            // Get all packets for client related to first packet
            var packetsForEndpoint = _packets.Where(p => p.EndpointIP == endpointIP && p.EndpointPort == endpointPort).ToList();

            // Try and get message header
            var messageHeader = InternalUtilities.GetMessageHeader(packetsForEndpoint.First());
            if (messageHeader != null)   // Header read from first packet
            {
                // Try and get connection message
                var connectionMessage = GetConnectionMessage(messageHeader, packetsForEndpoint);
                if (connectionMessage != null)
                {
                    // Remove all fully used packets
                    _packets.RemoveAll(packet => packetsForEndpoint.Where(p => p.Data.Length == 0).Contains(packet));

                    if (OnConnectionMessageReceived != null)
                    {
                        OnConnectionMessageReceived(connectionMessage, new MessageReceivedInfo()
                        {
                            ReceivedTime = DateTimeOffset.UtcNow,
                            RemoteEndpointInfo = new EndpointInfo()
                            {
                                Ip = endpointIP,
                                Port = endpointPort
                            }
                        });
                    }
                }
            }                            
        }

        //public void AddPacketForTesting(Packet packet)
        //{
        //    _packets.Add(packet);
        //}

        /// <summary>
        /// Gets first ConnectionMessage if sufficient packets received. Packet.Data will be updated to remove any data
        /// used. We need to consider that we may have a packet with the end of message #1 and the start of message #2
        /// if the client is sending messages in quick succession.
        /// </summary>
        /// <param name="messageHeader"></param>
        /// <param name="packetsForEndpoint"></param>
        /// <returns></returns>
        private ConnectionMessage GetConnectionMessage(MessageHeader messageHeader, List<Packet> packetsForEndpoint)
        {
            // Get total of all packets
            int totalPacketBytes = packetsForEndpoint.Sum(p => p.Data.Length);

            // Check that we have sufficient data
            if (totalPacketBytes >= messageHeader.PayloadLength)    // Sufficient data packets for message
            {                             
                // Set array of payload to create from each packet
                byte[] payloadData = new byte[messageHeader.PayloadLength];
                int payloadOffset = 0;                
                
                // Concatenate packet payloads
                var packet = packetsForEndpoint.First();
                int packetIndex = -1;
                do
                {
                    packetIndex++;
                   
                    System.Diagnostics.Debug.WriteLine($"{packetIndex} Packet.Data.Length={packet.Data.Length}");

                    // Calculate bytes remaining to copy. We may get a situation where a packet contains multiple ConnectionMessage
                    // instances and so we only need the first part of Packet.Data
                    var bytesRemainingToCopy = messageHeader.PayloadLength - payloadOffset;

                    // Determine position in Packet.Data to copy from to payloadData
                    var sourceOffset = packet == packetsForEndpoint.First() ?
                                            messageHeader.HeaderLength :   // For 1st packet then copy from after header
                                            0;                  // For 2nd+ packet then no header, so copy from start

                    // Determine how many packet bytes to copy to payloadData
                    var packetBytesToCopy = packet.Data.Length - sourceOffset;  // Default to all
                    var newData = new byte[0];      // Default to using all data
                    if (packetBytesToCopy > bytesRemainingToCopy)   // Only need some of the packet
                    {
                        packetBytesToCopy = bytesRemainingToCopy;

                        // Remove used data
                        newData = new byte[packet.Data.Length - packetBytesToCopy];
                        Buffer.BlockCopy(packet.Data, packetBytesToCopy, newData, 0, packet.Data.Length - packetBytesToCopy);
                        int zzz = 1000;
                    }                                
                   
                    // Copy from Packet.Data to payloadData
                    Buffer.BlockCopy(packet.Data, sourceOffset, payloadData, payloadOffset, packetBytesToCopy);

                    // Set remaining packet data (if any). We may have a packet with the end of message #1 and the start of
                    // message #2
                    packet.Data = newData;

                    // Move payload offset for next packet to copy in
                    payloadOffset += packetBytesToCopy;
                   
                    System.Diagnostics.Debug.WriteLine($"{packetIndex} bytesRemainingToCopy={bytesRemainingToCopy}, sourceOffset={sourceOffset}, packetBytesToCopy={packetBytesToCopy}, payloadOffset={payloadOffset}");

                    // Set next packet
                    packet = packet != packetsForEndpoint.Last() ? packetsForEndpoint[packetsForEndpoint.IndexOf(packet) + 1] : null;
                    
                } while (payloadOffset < messageHeader.PayloadLength);

                // Deserialize payload
                var connectionMessage = InternalUtilities.DeserialiseToConnectionMessage(payloadData);
                return connectionMessage;                                
            }            

            return null;
        }        
    }
}
