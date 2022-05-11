using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

public enum SpawnerType
{
    NormalEnemy,
    BossEnemy,
    TowerEnemy,
    NormalUnit,
}

public abstract class Multi_SpawnerBase : MonoBehaviour
{
    [SerializeField] protected string _rootName;
    [SerializeField] protected string _rootPath;

    public abstract void Init();

    protected T[] CreatePool<T>(GameObject go, string path, int count) where T : Component
        => Multi_Managers.Pool.CreatePool(go, path, count).GetComponentsInChildren<Poolable>().Select(x => x.GetComponent<T>()).ToArray();

    protected T[] CreatePool_InGroup<T>(GameObject go, string path, int count) where T : Component
        => Multi_Managers.Pool.CreatePool_InGroup(go, path, count, _rootName).GetComponentsInChildren<Poolable>().Select(x => x.GetComponent<T>()).ToArray();

    public string BuildPath(string rooPath, GameObject go) => $"{rooPath}/{go.name}";
    public string BuildPath(string rooPath, string folderName, GameObject go) => $"{rooPath}/{folderName}/{go.name}";
}