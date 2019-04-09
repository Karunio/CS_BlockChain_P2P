using System.Text;
using System.IO;
using System;

class Filesaction
{
    public String FileName { get; set; }
    public String Extension { get; set; }
    public String DataHash { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime LastWriteTime { get; set; }

    public Filesaction(String filePath)
    {
        FileInfo fileInfo = new FileInfo(filePath);

        FileName = fileInfo.Name;
        Extension = fileInfo.Extension;
        CreationTime = fileInfo.CreationTime;
        LastWriteTime = fileInfo.LastWriteTime;
        DataHash = CalculateDataHash(filePath);
    }

    public Filesaction(FileInfo fileInfo)
    {
        FileName = fileInfo.Name;
        Extension = fileInfo.Extension;
        CreationTime = fileInfo.CreationTime;
        LastWriteTime = fileInfo.LastWriteTime;
        DataHash = CalculateDataHash(fileInfo.FullName);
    }

    public String CalculateHash()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append($"{ FileName }{ Extension }{ CreationTime }{ LastWriteTime }{ DataHash }");

        return HashTools.ToBase64Hash(stringBuilder.ToString());
    }

    private String CalculateDataHash(String filePath)
    {
        return HashTools.ToBase64Hash(File.ReadAllBytes(filePath));
    }
}