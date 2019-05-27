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

        public Filesactions(String root, String caseID, String sourceDirectory)
        {
            FilesactionList = new List<Filesaction>();
            CaseID = caseID;
            Directory.CreateDirectory($"{ root }/{ CaseID }");

            DirectoryInfo directoryInfo = new DirectoryInfo(sourceDirectory);
            FileInfo[] files = directoryInfo.GetFiles();

            foreach (var file in files)
            {
                Filesaction filesaction = new Filesaction(file);
                FilesactionList.Add(filesaction);
                file.CopyTo($"{ root }/{ CaseID }/{ file.Name }", true);

                var destFile = new FileInfo($"{ root }/{ CaseID }/{ file.Name }");
                destFile.CreationTime = file.CreationTime;
                destFile.LastAccessTime = file.LastAccessTime;
                destFile.LastWriteTime = file.LastWriteTime;
            }
        }
    }
}
