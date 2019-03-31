using System;
using System.Collections.Generic;

class BlockChain
{
    public List<Block> BlockList { get; set; }
    public int Difficulty { get; set; }
    public String DiffString { get; set; }

    BlockChain()
    {
        Init();
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
        Block genesis = new Block(null, DateTime.Now, null, null);
        return genesis;
    }

    //Get This BlockChain Latest Block
    private Block GetLatestBlock()
    {
        return BlockList[BlockList.Count - 1];
    }


    //Create String According to Difficulty
    private void CreateDiffString()
    {
        String DiffString = new String('0', Difficulty);
    }

    //Check BlockChain
    public bool IsChainValid()
    {
        for (int i = 1; i < BlockList.Count; i++)
        {
            Block prevBlock = BlockList[i - 1];
            Block currBlock = BlockList[i];

            if (currBlock.Hash != currBlock.CalculateHash()) return false;
            if (prevBlock.Hash != currBlock.PreviousHash) return false;
        }

        return true;
    }


    //Print Blocks in List
    public void PrintBlocks()
    {
        foreach (var block in BlockList) block.PrintConsole();
    }

    //Add Block to List
    public void AddBlock(Block block)
    {
        block.PreviousHash = GetLatestBlock().Hash;
        block.Mining(DiffString);
        BlockList.Add(block);
    }
}