using System.Text;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VSCODE_PR
{
    class Node
    {
        private const int JoinListenPort = 45000;
        private const int SyncPort = 45001;
        private const int DownloadPort = 45002;
        private const int BufferSize = 2048;
        public BlockChain Chain { get; set; }
        public bool Sync = false;

        public Node()
        {

        }

        public void StartListening()
        {
            Thread join = new Thread(JoinListening);
            join.IsBackground = true;
            join.Start();
        }

        public void StartJoin()
        {
            byte[] buffer = new byte[BufferSize];
            TcpListener syncListener = new TcpListener(IPAddress.Any, SyncPort);
            syncListener.Start();
            using (Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            {
                IPEndPoint groupEP = new IPEndPoint(IPAddress.Broadcast, JoinListenPort);
                client.DontFragment = true;
                client.Send(HashTools.ToBytesHash("FCFJOIN"));
            }

            using (TcpClient syncServer = syncListener.AcceptTcpClientAsync().Result)
            {
                using (NetworkStream networkStream = syncServer.GetStream())
                {
                    byte[] sendLatestHash = Convert.FromBase64String(Chain.GetLatestBlock().Hash);
                    networkStream.Write(sendLatestHash);
                    networkStream.Flush();

                    int nRecieve = networkStream.Read(buffer);
                    int blockCount = BitConverter.ToInt32(buffer, 0);

                    for (int i = 0; i < blockCount;)
                    {
                        nRecieve = networkStream.Read(buffer);
                        String deSerialJson = Encoding.UTF8.GetString(buffer, 0, nRecieve);
                        Block addBlock = JsonConvert.DeserializeObject(deSerialJson) as Block;
                        this.Chain.AddBlock(addBlock, true);
                        if(this.Chain.IsChainValid())
                        {
                            i++;
                            networkStream.Write(Encoding.UTF8.GetBytes("OK"));
                            continue;
                        }
                        else
                        {                            
                            Chain.BlockList.Remove(addBlock);
                            networkStream.Write(Encoding.UTF8.GetBytes("FAIL"));
                        }
                        networkStream.Flush();
                        
                    }
                }
            }
            syncListener.Stop();
            Sync = true;
        }

        private void StartSync(EndPoint endPoint)
        {
            byte[] buffer = new byte[BufferSize];
            IPEndPoint targetEP = new IPEndPoint(((IPEndPoint)endPoint).Address, SyncPort);
            using (TcpClient target = new TcpClient(targetEP))
            {
                using (NetworkStream networkStream = target.GetStream())
                {
                    int nRecieve = networkStream.Read(buffer);
                    String recievedHash = Convert.ToBase64String(buffer, 0, nRecieve);

                    int blockIndex = Chain.FindBlockIndex(recievedHash);

                    networkStream.Write(BitConverter.GetBytes(blockIndex));

                    for (int i = blockIndex; i < Chain.BlockList.Count;)
                    {
                        byte[] sendBlock = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(Chain.BlockList[i]));
                        networkStream.Write(sendBlock);
                        networkStream.Flush();

                        nRecieve = networkStream.Read(buffer);
                        String recvString = Encoding.UTF8.GetString(buffer, 0, nRecieve);
                        if(recvString == "OK")
                        {
                            i++;                            
                        }
                    }
                }
            }
        }

        private void JoinListening()
        {
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, JoinListenPort);

            while (true)
            {
                if (!Sync)
                {
                    Task.Delay(1000);
                    continue;
                }

                using (UdpClient listener = new UdpClient(JoinListenPort))
                {
                    byte[] receivedData = listener.Receive(ref groupEP);
                    if (HashTools.ToBase64Hash(receivedData) == HashTools.ToBase64Hash("FCFJOIN"))
                    {
                        StartSync(listener.Client.RemoteEndPoint);
                    }
                }
            }
        }
    }
}
