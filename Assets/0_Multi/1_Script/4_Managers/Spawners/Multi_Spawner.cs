using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
 
public enum SpawnerType
{
    NormalEnemy,
    BossEnemy,
    TowerEnemy,
    NormalUnit,
}

public abstract class Multi_Spawner
{
    protected string rootPath;
    protected string[] GetFilePathsFromFolder(string _folderPath) => Directory.GetFiles(_folderPath);

    protected abstract void CreatePool(string _path, int _count);
    public abstract void Init();
}
