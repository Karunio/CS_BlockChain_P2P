using System.IO;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

class Filesactions
{
    public String CaseID { get; set; }
    public List<Filesaction> FilesactionList { get; set; }

    public Filesactions(String caseID)
    {
        FilesactionList = new List<Filesaction>();
        CaseID = caseID;
        if (!Directory.Exists($"./{ CaseID }")) Directory.CreateDirectory($"./{ CaseID }");

        DirectoryInfo directoryInfo = new DirectoryInfo(CaseID);
        FileInfo[] files = directoryInfo.GetFiles();

        foreach (var file in files)
        {
            Filesaction filesaction = new Filesaction(file);
            FilesactionList.Add(filesaction);
        }
    }
}