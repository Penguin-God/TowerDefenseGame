using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

class PoolGroup
{
    public Transform Root { get; set; }
    Dictionary<string, Pool> poolByPath = new Dictionary<string, Pool>();

    public void Init(string folderName, Transform root)
    {
        Root = new GameObject($"{folderName}_Root").transform;
        Root.SetParent(root);
    }

    public Transform CreatePool(GameObject original, string path, int count)
    {
        if(poolByPath.TryGetValue(path, out Pool dicPool))
        {
            dicPool.Init(original, path, count);
            return dicPool.Root;
        }
        Pool pool = new Pool();
        poolByPath.Add(path, pool);

        pool.Init(original, path, count);
        pool.Root.SetParent(Root);
        
        return pool.Root;
    }

    public bool TryGetPool(string path, out Pool pool)  => poolByPath.TryGetValue(path, out pool);
}

class Pool
{
    public Transform Root { get; set; } = null;
    public GameObject Original { get; private set; }
    public string Path { get; private set;}

    Stack<Poolable> poolStack = new Stack<Poolable>();
    public int Count => poolStack.Count;

    public void Init(GameObject original, string path, int count)
    {
        //string rootName = path.Split('/')[path.Split('/').Length - 1];
        Root = new GameObject($"{original.name}_Root").transform;
        if(original.GetComponent<Poolable>() == null) original.AddComponent<Poolable>();
        Original = original;
        Path = path;
        for (int i = 0; i < count; i++)
            Push(CreateObject());
    }

    Poolable CreateObject()
    {
        Poolable poolable;
        GameObject go = PhotonNetwork.Instantiate($"Prefabs/{Path}", Vector3.zero, Quaternion.identity);
        go.transform.SetParent(Root);

        poolable = go.GetComponent<Poolable>();
        if (poolable == null) poolable = go.AddComponent<Poolable>();
        poolable.Path = Path;

        return go.GetComponent<Poolable>();
    }

    public void Push(Poolable poolable)
    {
        if (poolable == null) return;

        poolable.transform.SetParent(Root);
        poolable.gameObject.SetActive(false);
        poolable.IsUsing = false;
        poolStack.Push(poolable);
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

    // TODO : 구현하기 (지금은 틀만 만들어둠)
    public Transform CreatePool(string path, int count) => null;

    public Transform CreatePool(GameObject original, string path, int count) => CreatePool(original, path, count, etcGroupName);

    public Transform CreatePool(GameObject original, string path, int count, string groupName)
    {
        if (_poolGroupByFolderName.TryGetValue(groupName, out PoolGroup poolGroup))
            return poolGroup.CreatePool(original, path, count);
        else
        {
            PoolGroup _newPoolGroup = new PoolGroup();
            _newPoolGroup.Init(groupName, _root);
            _poolGroupByFolderName.Add(groupName, _newPoolGroup);
            return _newPoolGroup.CreatePool(original, path, count);
        }
    }

    public void Push(Poolable poolable) => Push(poolable.gameObject, poolable.Path);
    public void Push(GameObject go, string path)
    {
        Pool pool = GetPool(path);
        if (pool == null)
        {
            Multi_Managers.Resources.PhotonDestroy(go);
            return;
        }

        go.transform.SetParent(pool.Root);
        PhotonView pv = go.GetComponent<PhotonView>();
        if (pv != null) RPC_Utility.Instance.RPC_Active(pv.ViewID, false);
        pool.Push(go.GetComponent<Poolable>());
    }

    public Poolable Pop(string path, Transform parent = null)
    {
        Poolable poolable = GetPool(path).Pop(parent);
        Debug.Assert(poolable != null, "poolable not defind");

        PhotonView pv = poolable.GetComponent<PhotonView>();
        if (pv != null) RPC_Utility.Instance.RPC_Active(pv.ViewID, true);
        return poolable;
    }

    //private Pool GetPool(string path) => 
    private Pool GetPool(string path)
    {
        string folderName = path.Split('/')[0];
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