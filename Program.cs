using System.Reflection.Metadata;
using System.Text;
using System.Data;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace VSCODE_PR
{
    class Program
    {
        public static void Main(String[] args)
        {
            //Server Example
            BlockChain chain = new BlockChain(@"C:\Users\Karunio\Desktop\BlockChainRoot");
            chain.CreateBlock(@"C:\Users\Karunio\Desktop\BlockSample");
            chain.PrintBlocks();

            Node node = new Node(chain);
            node.Sync = true;
            node.StartListening();

            Console.ReadLine();
            Console.ReadLine();

            // // Client Example
            // BlockChain chain = new BlockChain(@"C:\Users\Karunio\Desktop\BlockChainRoot");
            // Node node = new Node(chain);
            // chain.PrintBlocks();
            // node.StartJoin();
            // chain.PrintBlocks();

            // Console.ReadLine();
            // Console.ReadLine();

        }
    }
}
