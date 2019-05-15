using System.IO;
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
        public bool Sync { get; set; } = false;

        public Node()
        {

        }

        public Node(BlockChain Chain)
        {
            this.Chain = Chain;
        }

        public void StartListening()
        {
            Thread join = new Thread(JoinListening);
            join.IsBackground = true;
            join.Start();
        }

        #region Node Try to join Chain Network
        public void StartJoin()
        {
            using (TcpClient syncServer = TcpClientParseFromText("./serverList.txt"))
            {
                if (syncServer == null)
                {
                    Console.WriteLine("Network Connection is Unstable");
                    return;
                }

                byte[] buffer = new byte[BufferSize];
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
                        Block addBlock = JsonConvert.DeserializeObject<Block>(deSerialJson);
                        this.Chain.AddBlock(addBlock, true);
                        if (this.Chain.IsChainValid())
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
            Sync = true;
        }

        private TcpClient TcpClientParseFromText(String fileName)
        {
            StreamReader reader = new StreamReader(fileName);
            String serverIp;
            while ((serverIp = reader.ReadLine()) != null)
            {
                TcpClient target = new TcpClient(serverIp, JoinListenPort);

                if (target.Connected)
                    return target;
            }

            return null;
        }

        private void StartSync(TcpClient target)
        {
            byte[] buffer = new byte[BufferSize];

            using (NetworkStream networkStream = target.GetStream())
            {
                int nRecieve = networkStream.Read(buffer);
                String recievedHash = Convert.ToBase64String(buffer, 0, nRecieve);

                int blockIndex = Chain.FindBlockIndex(recievedHash);
                blockIndex++;
                networkStream.Write(BitConverter.GetBytes(Chain.BlockList.Count - blockIndex));
                networkStream.Flush();

                for (int i = blockIndex; i < Chain.BlockList.Count;)
                {
                    byte[] sendBlock = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(Chain.BlockList[i], Formatting.Indented));
                    networkStream.Write(sendBlock);
                    networkStream.Flush();

                    nRecieve = networkStream.Read(buffer);
                    String recvString = Encoding.UTF8.GetString(buffer, 0, nRecieve);
                    if (recvString == "OK")
                    {
                        i++;
                    }
                }
            }
        }
        #endregion

        private void JoinListening()
        {
            TcpListener joinLisetner = new TcpListener(IPAddress.Any, JoinListenPort);
            joinLisetner.Start();
            while (true)
            {
                if (!Sync)
                {
                    Task.Delay(1000);
                    continue;
                }

                using (TcpClient target = joinLisetner.AcceptTcpClient())
                {
                    StartSync(target);
                }
            }
        }
    }
}
