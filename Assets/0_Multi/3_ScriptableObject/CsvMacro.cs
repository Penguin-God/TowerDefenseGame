using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System.Linq;

public class CsvMacro
{
    public void Save(string csv, string path)
    {
        Stream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
        StreamWriter outStream = new StreamWriter(fileStream, Encoding.UTF8);
        outStream.Write(csv);
        outStream.Close();
    }

    string GetFileName(string path) => path.Split('/')[path.Split('/').Length - 1];
    string FilePathToResourcesPath(string path, string replacePath, string fileExtension) => path.Replace("\\", "/").Replace(replacePath, "").Replace(fileExtension, "");
}
