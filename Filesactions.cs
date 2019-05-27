using System.IO;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace VSCODE_PR
{
    class Filesactions
    {
        public String CaseID { get; set; }
        public List<Filesaction> FilesactionList { get; set; }

        public Filesactions()
        {

        }

        public Filesactions(String caseID, String sourceDirectory)
        {
            FilesactionList = new List<Filesaction>();
            CaseID = caseID;
            Directory.CreateDirectory($"./{ CaseID }");

            DirectoryInfo directoryInfo = new DirectoryInfo(sourceDirectory);
            FileInfo[] files = directoryInfo.GetFiles();

            foreach (var file in files)
            {
                Filesaction filesaction = new Filesaction(file);
                FilesactionList.Add(filesaction);
                file.CopyTo($"./{ CaseID }/{ file.Name }", true);

                var destFile = new FileInfo($"./{ CaseID }/{ file.Name }");
                destFile.CreationTime = file.CreationTime;
                destFile.LastAccessTime = file.LastAccessTime;
                destFile.LastWriteTime = file.LastWriteTime;
            }
        }
    }
}
