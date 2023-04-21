using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System.Linq;

public class ResourcesFileData
{
    public ResourcesFileData(string path, string fileExtension)
    {
        _resourcesPath = FilePathToResourcesPath(path, fileExtension);
        _name = GetFileName(_resourcesPath);
    }

    string _resourcesPath;
    string _name;

    public string ResourcesPath => _resourcesPath;
    public string Name => _name;

    string FilePathToResourcesPath(string path, string fileExtension) => string.Join("", path.Replace("\\", "/").Replace(fileExtension, "").Skip(1));
    string GetFileName(string path) => path.Split('/')[path.Split('/').Length - 1];
}

public class CsvMacroUseCase
{
    public string DirvePath => Path.Combine(Application.dataPath, "0_Multi", "Resources").Replace("/", "\\");

    public void Save(string csv, string path)
    {
        Stream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
        StreamWriter outStream = new StreamWriter(fileStream, Encoding.UTF8);
        outStream.Write(csv);
        outStream.Close();
    }

    public IEnumerable<ResourcesFileData> GetFileDatas(string rootPath, string fileExtension)
    {
        return Directory.GetFiles(Path.Combine(DirvePath, rootPath), $"*{fileExtension}", SearchOption.AllDirectories)
            .Select(x => new ResourcesFileData(x.Replace(DirvePath, ""), fileExtension));
    }
}
