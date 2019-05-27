using System.Text;
using System.IO;
using System;

namespace VSCODE_PR
{
    class Filesaction
    {
        public String FileName { get; set; }
        public String Extension { get; set; }
        public String DataHash { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastWriteTime { get; set; }
        public DateTime LastAccessTime { get; set; }

        public Filesaction()
        {
            
        }

        public Filesaction(String filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);

            FileName = fileInfo.Name;
            Extension = fileInfo.Extension;
            CreationTime = fileInfo.CreationTime;
            LastWriteTime = fileInfo.LastWriteTime;
            LastAccessTime = fileInfo.LastAccessTime;
            DataHash = CalculateDataHash(filePath);
        }

        public Filesaction(FileInfo fileInfo)
        {
            FileName = fileInfo.Name;
            Extension = fileInfo.Extension;
            CreationTime = fileInfo.CreationTime;
            LastWriteTime = fileInfo.LastWriteTime;
            LastAccessTime = fileInfo.LastAccessTime;
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
}
