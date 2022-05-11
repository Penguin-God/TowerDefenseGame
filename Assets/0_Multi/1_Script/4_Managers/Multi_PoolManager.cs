using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

class PoolGroup
{
    public Transform Root { get; set; }
    
    public void Init(string rootName)
    {
        Root = new GameObject($"{rootName}_Root").transform;
    }

    //public Transform CreatePool(GameObject original, string path, int count)
    //{
    //    Pool pool = new Pool();
    //    pool.Init(original, path, count);
    //    pool.Root.SetParent(Root);
        
    //    return pool.Root;
    //}
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
        go.name = Original.name;

        poolable = go.GetOrAddComponent<Poolable>();
        poolable.Path = Path;

        return poolable;
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

    Dictionary<string, PoolGroup> _poolGroupByName = new Dictionary<string, PoolGroup>();
    Dictionary<string, Pool> _poolByName = new Dictionary<string, Pool>();

    public void Init()
    {
        if(_root == null)
        {
            _root = new GameObject("Mulit PoolManager").transform;
            //DontDestroyOnLoad(_root.gameObject);
        }
    }

    public Transform CreatePool(GameObject go, string path, int count, Transform root = null)
    {
        Pool pool = new Pool();
        pool.Init(go, path, count);
        if(root == null) pool.Root.SetParent(_root);
        else pool.Root.SetParent(root);
        _poolByName.Add(go.name, pool);
        return pool.Root;
    }

    public Transform CreatePool_InGroup(GameObject original, string path, int count, string groupName)
    {
        PoolGroup poolGroup;
        if (_poolGroupByName.TryGetValue(groupName, out poolGroup))
            return CreatePool(original, path, count, poolGroup.Root);
        else // 없으면 새로운 풀 그룹 생성
        {
            poolGroup = new PoolGroup();
            poolGroup.Init(groupName);
            poolGroup.Root.SetParent(_root);
            _poolGroupByName.Add(groupName, poolGroup);
            return CreatePool(original, path, count, poolGroup.Root);
        }
    }

    public void Push(Poolable poolable) => Push(poolable.gameObject, poolable.Path);
    public void Push(GameObject go, string path)
    {
        Pool pool = _poolByName[go.name];

        if (pool == null)
        {
            Multi_Managers.Resources.PhotonDestroy(go);
            return;
        }

        go.transform.SetParent(pool.Root);
        PhotonView pv = go.GetOrAddComponent<PhotonView>();
        RPC_Utility.Instance.RPC_Active(pv.ViewID, false);
        pool.Push(go.GetComponent<Poolable>());
    }

    public Poolable Pop(GameObject go, Transform parent = null)
    {
        Poolable poolable = _poolByName[go.name].Pop(parent);
        Debug.Assert(poolable != null, "poolable not defind");

        PhotonView pv = poolable.gameObject.GetOrAddComponent<PhotonView>();
        RPC_Utility.Instance.RPC_Active(pv.ViewID, true);
        return poolable;
    }

    public GameObject GetOriginal(string path)
    {
        string name = path.Split('/')[path.Split('/').Length-1];
        Debug.Log(name);
        Pool pool = _poolByName[name];
        if (pool != null) return pool.Original;
        else return null;
    }
}