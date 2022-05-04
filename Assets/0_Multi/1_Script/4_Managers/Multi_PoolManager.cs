using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PoolGroup
{
    public Transform Root { get; set; }
    Dictionary<string, Pool> poolByPath = new Dictionary<string, Pool>();

    public void Init(string folderName, Transform root)
    {
        Root = new GameObject($"{folderName}_Root").transform;
        Root.SetParent(root);
    }

    public Transform CreatePool(string path, int count)
    {
        Debug.Log(path);
        Pool pool = new Pool();
        pool.Init(path, count);
        pool.Root.SetParent(Root);
        poolByPath.Add(path, pool);
        return pool.Root;
    }

    public bool TryGetPool(string path, out Pool pool)  => poolByPath.TryGetValue(path, out pool);
}

class Pool
{
    public Transform Root { get; set; }
    public GameObject Original { get; private set; }
    public string Path { get; private set;}

    Stack<Poolable> poolStack = new Stack<Poolable>();

    public void Init(string path, int count)
    {
        string rootName = path.Split('/')[path.Split('/').Length - 1];
        Root = new GameObject($"{rootName}_Root").transform;
        Path = path;
        for (int i = 0; i < count; i++)
            Push(CreateObject());
    }

    Poolable CreateObject()
    {
        GameObject go = Multi_Managers.Resources.PhotonInsantiate(Path, Root);
        if (go.GetComponent<Poolable>() == null) go.AddComponent<Poolable>();
        if (Original == null) Original = go;
        return go.GetComponent<Poolable>();
    }

    public void Push(Poolable poolable)
    {
        if (poolable == null) return;

        poolable.transform.SetParent(Root);
        poolable.gameObject.SetActive(false);
        poolable.IsUsing = false;
        poolStack.Push(poolable);
        Debug.Log($"is PUSH!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! {poolStack.Count}");
    }

    public Poolable Pop(Transform parent)
    {
        Poolable poolable;
        if (poolStack.Count > 0) poolable = poolStack.Pop();
        else poolable = CreateObject();

        poolable.transform.SetParent(parent);
        poolable.gameObject.SetActive(true);
        poolable.IsUsing = true;

        return poolable;
    }
}

public class Multi_PoolManager
{
    Transform _root;

    Dictionary<string, PoolGroup> _poolGroupByFolderName = new Dictionary<string, PoolGroup>();
    const string etcGroupName = "etc";
    public void Init()
    {
        if(_root == null)
        {
            _root = new GameObject("Mulit PoolManager").transform;
            //DontDestroyOnLoad(_root.gameObject);
        }
    }


    public Transform CreatePool(string path, int count) => CreatePool(path, count, etcGroupName);

    public Transform CreatePool(string path, int count, string groupName)
    {
        if (_poolGroupByFolderName.TryGetValue(groupName, out PoolGroup poolGroup))
            return poolGroup.CreatePool(path, count);
        else
        {
            PoolGroup _newPoolGroup = new PoolGroup();
            _newPoolGroup.Init(groupName, _root);
            _poolGroupByFolderName.Add(groupName, _newPoolGroup);
            return _newPoolGroup.CreatePool(path, count);
        }
    }

    public void Push(GameObject go, string path)
    {
        Pool pool = GetPool(path);
        if (pool == null)
        {
            Multi_Managers.Resources.PhotonDestroy(go);
            return;
        }

        pool.Push(go.GetComponent<Poolable>());
    }

    public Poolable Pop(string path, Transform parent = null) => GetPool(path).Pop(parent);

    private Pool GetPool(string path)
    {
        string folderName = path.Split('/')[0];
        Debug.Log(folderName);
        if (_poolGroupByFolderName.TryGetValue(folderName, out PoolGroup poolGroup))
        {
            if (poolGroup.TryGetPool(path, out Pool pool))
                return pool;
            else
                return null;
        }
        
        return null;
    }

    public GameObject GetOriginal(string path)
    {
        Pool pool = GetPool(path);
        if (pool != null) return pool.Original;
        else return null;
    }
}