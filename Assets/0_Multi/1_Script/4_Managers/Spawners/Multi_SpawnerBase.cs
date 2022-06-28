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
    public virtual void MasterInit() { }

    protected T[] CreatePool<T>(GameObject go, string path, int count) where T : Component
        => Multi_Managers.Pool.CreatePool(go, path, count).GetComponentsInChildren<T>(true).ToArray();

    protected T[] CreatePool_InGroup<T>(GameObject go, string path, int count) where T : Component
        => Multi_Managers.Pool.CreatePool_InGroup(go, path, count, _rootName).GetComponentsInChildren<T>(true).ToArray();

    protected Transform CreatePool_InGroup(GameObject go, string path, int count)
        => Multi_Managers.Pool.CreatePool_InGroup(go, path, count, _rootName);

    public string BuildPath(string rooPath, GameObject go) => $"{rooPath}/{go.name}";
    public string BuildPath(string rooPath, string folderName, GameObject go) => $"{rooPath}/{folderName}/{go.name}";

    public virtual void SettingPoolObject(object obj) { }
}