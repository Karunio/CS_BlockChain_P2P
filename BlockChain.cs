using System;
using System.Collections.Generic;
using Newtonsoft.Json;

class BlockChain
{
    public List<Block> BlockList { get; set; }
    public int Difficulty { get; set; }
    public String DiffString { get; set; }

    public BlockChain()
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
        Block genesis = new Block(DateTime.Today, "null", new Filesactions("Genesis2", "Genesis"));
        genesis.Mining(DiffString);
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


    //Print Blocks in List
    public void PrintBlocks()
    {
        Console.WriteLine(JsonConvert.SerializeObject(this, Formatting.Indented));
    }

    //Add Block to List
    public void AddBlock(Block block)
    {
        block.PreviousHash = GetLatestBlock().Hash;
        block.Mining(DiffString);
        block.Index = BlockList.Count;
        BlockList.Add(block);
    }

    public override String ToString()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}