using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace VSCODE_PR
{
    class BlockChain
    {
        public List<Block> BlockList { get; set; }
        public int Difficulty { get; set; }
        public String DiffString { get; set; }
        private String RootDirectory;

        public BlockChain(String rootDirectory)
        {
            Init();
            this.RootDirectory = rootDirectory;
        }

        //Init Function
        private void Init()
        {
            Difficulty = 1;
            CreateDiffString();
            BlockList = new List<Block>();
            BlockList.Add(CreateGenesisBlock());
        }

        //Add to List GenesisBlock
        private Block CreateGenesisBlock()
        {
            Block genesis = new Block(new DateTime(2019, 1, 1, 0, 0, 0, DateTimeKind.Utc), "null", null);
            genesis.Mining(DiffString);
            return genesis;
        }

        //Get This BlockChain Latest Block
        public Block GetLatestBlock()
        {
            return BlockList[BlockList.Count - 1];
        }


        //Create String According to Difficulty
        private void CreateDiffString()
        {
            DiffString = new String('0', Difficulty);
        }

        //Check BlockChain
        public bool IsChainValid()
        {
            for (int i = 1; i < BlockList.Count; i++)
            {
                Block prevBlock = BlockList[i - 1];
                Block currBlock = BlockList[i];

                if (currBlock.Hash != currBlock.CalculateBlockHash()) return false;
                if (prevBlock.Hash != currBlock.PreviousHash) return false;
            }

            return true;
        }

        public int FindBlockIndex(String Hash)
        {
            for (int i = 0; i < BlockList.Count; i++)
            {
                if (BlockList[i].Hash == Hash)
                {
                    return i;
                }
            }
            return -1;
        }


        //Print Blocks in List
        public void PrintBlocks()
        {
            Console.WriteLine(JsonConvert.SerializeObject(this, Formatting.Indented));
        }

        //Add Block to List
        public void AddBlock(Block block, bool sync = false)
        {
            if (sync)
            {
                BlockList.Add(block);
            }
            else
            {
                block.PreviousHash = GetLatestBlock().Hash;
                block.Mining(DiffString);
                block.Index = BlockList.Count;
                BlockList.Add(block);
            }
        }

        public void CreateBlock(String sourceDir)
        {
            Filesactions filesactions = new Filesactions(RootDirectory, $"부산-남구-부경대-{DateTime.Now.Ticks}", sourceDir);
            Block block = new Block(DateTime.Now, null, filesactions);
            AddBlock(block);
        }

        public override String ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}