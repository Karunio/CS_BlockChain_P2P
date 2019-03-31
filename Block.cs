using System.Text;
using System.Security.Cryptography;
using System;
using System.Security;

class Block
{
    public int Index { get; set; }
    public DateTime TimeStamp { get; set; }
    public String FileName { get; set; }
    public String Extension { get; set; }
    public String DataHash { get; set; }
    public String PreviousHash { get; set; }
    public String Hash { get; set; }
    public int Nonce { get; set; }

    public Block(String filePath, DateTime timeStamp, String previousHash, String dataHash)
    {
        Index = 0;
        TimeStamp = timeStamp;
        PreviousHash = previousHash;
        DataHash = dataHash;
        Hash = CalculateHash();
        Nonce = 0;
    }

    //Set a FileName, Extention, DataHash
    private void SetFileAttributes()
    {
        
    }

    //If File exist, Read FileData to Binary and Calc Hash
    private void CalculateDataHash()
    {

    }

    //Concatenate Filed Members And Calc Hash
    public String CalculateHash()
    {
        //Use SHA256 Hash Function
        SHA256 sha256 = SHA256.Create();

        //Use StringBuilder for Concatenate Members
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append($"{ TimeStamp }{ DataHash }{ PreviousHash ?? "" }{ Nonce }");

        //Calc SHA256
        byte[] inputDatas = Encoding.UTF8.GetBytes(stringBuilder.ToString());
        byte[] outputDatas = sha256.ComputeHash(inputDatas);

        //Created Byte[] Change to String
        return BitConverter.ToString(outputDatas).Replace("-", String.Empty);
    }

    //Calculate Hash According to DiffString
    public void Mining(String diffString)
    {
        while (Hash == null || Hash.Substring(0, diffString.Length) != diffString)
        {
            Nonce++;
            Hash = CalculateHash();
        }
    }

    //Print Block to Console
    public void PrintConsole()
    {
        Console.WriteLine($"Index: { Index }");
        Console.WriteLine($"TimeStamp: { TimeStamp }");
        Console.WriteLine($"FileName: { FileName }");
        Console.WriteLine($"Extension: { Extension }");
        Console.WriteLine($"DataHash: { DataHash }");
        Console.WriteLine($"PreviousHash: { PreviousHash ?? "" }");
        Console.WriteLine($"Hash: { Hash }");
        Console.WriteLine($"Nonce: { Nonce }");
    }

    //ToString Override According to Block
    public override String ToString()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append($"{ Index }{ TimeStamp }{ DataHash }{ PreviousHash ?? "" }{ Hash }{ Nonce }");

        return stringBuilder.ToString();
    }
}