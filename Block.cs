using System.Text;
using System;
using Newtonsoft.Json;

class Block
{
    public int Index { get; set; }
    public DateTime TimeStamp { get; set; }
    public String PreviousHash { get; set; }
    public String Hash { get; set; }
    public Filesactions Filesactions { get; set; }
    public int Nonce { get; set; }

    public Block(DateTime timeStamp, String previousHash, Filesactions filesactions)
    {
        Index = 0;
        TimeStamp = timeStamp;
        PreviousHash = previousHash;
        this.Filesactions = filesactions;
        Nonce = 0;
    }

    //Concatenate Filed Members And Calc Hash
    public String CalculateBlockHash()
    {
        //Use StringBuilder for Concatenate Members
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append($"{ TimeStamp }");
        stringBuilder.Append($"{ PreviousHash }{ JsonConvert.SerializeObject(Filesactions) }{ Nonce }");

        return HashTools.ToBase64Hash(stringBuilder.ToString());
    }

    //Calculate Hash According to DiffString
    public void Mining(String diffString)
    {
        while (Hash == null || Hash.Substring(0, diffString.Length) != diffString)
        {
            Nonce++;
            Hash = CalculateBlockHash();
        }
    }

    //Print Block to Console
    public void PrintConsole()
    {
        Console.WriteLine($"Index: { Index }");
        Console.WriteLine($"TimeStamp: { TimeStamp }");
        Console.WriteLine($"PreviousHash: { PreviousHash }");
        Console.WriteLine($"Hash: { Hash }");
        Console.WriteLine($"Nonce: { Nonce }\n");
    }

    //ToString Override According to Block
    public override String ToString()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append($"{ Index }-{ TimeStamp }-");
        stringBuilder.Append($"{ PreviousHash }-{ Hash }-{ Nonce }");

        return stringBuilder.ToString();
    }
}