using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

class PoolGroup
{
    public Transform Root { get; set; }
    
    public void Init(string rootName)
    {
        Root = new GameObject($"{rootName}_Root").transform;
    }
}

class Pool
{
    public Transform Root { get; set; } = null;
    public GameObject Original { get; private set; }
    public string Path { get; private set;}

    Stack<Poolable> poolStack = new Stack<Poolable>();
    public int Count => poolStack.Count;

    Action<GameObject> SetupObjAct;

    public void Init(GameObject original, string path, int count, Action<GameObject> setupAct)
    {
        Root = new GameObject($"{original.name}_Root").transform;
        if(original.GetComponent<Poolable>() == null) original.AddComponent<Poolable>();
        Original = original;
        Path = path;
        SetupObjAct = setupAct;
        for (int i = 0; i < count; i++)
            Push(CreateObject());
    }

    Poolable CreateObject()
    {
        Poolable poolable;
        GameObject previewGo = Resources.Load<GameObject>($"Prefabs/{Path}");
        // TODO : 이 좆같은 코드 리팩터링하기
        previewGo.GetOrAddComponent<PhotonView>();
        GameObject go = PhotonNetwork.Instantiate($"Prefabs/{Path}", Vector3.zero, previewGo.transform.rotation);
        go.transform.SetParent(Root);
        go.name = Original.name;
        SetupObjAct?.Invoke(go);

        poolable = go.GetOrAddComponent<Poolable>();
        poolable.Path = Path;

        return poolable;
    }

    public void Push(Poolable poolable)
    {
        if (poolable == null) return;

        poolable.transform.SetParent(Root);
        poolable.IsUsing = false;
        poolStack.Push(poolable);
    }

    public Poolable Pop()
    {
        Poolable poolable;

        if (poolStack.Count > 0) poolable = poolStack.Pop();
        else poolable = CreateObject();

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
        }
    }

    public Transform CreatePool(GameObject go, string path, int count, Transform root = null, Action<GameObject> action = null)
    {
        Pool pool = new Pool();
        pool.Init(go, path, count, action);
        if(root == null) pool.Root.SetParent(_root);
        else pool.Root.SetParent(root);
        _poolByName.Add(go.name, pool);
        return pool.Root;
    }

    public Transform CreatePool_InGroup(GameObject original, string path, int count, string groupName, Action<GameObject> action = null)
    {
        PoolGroup poolGroup;
        if (_poolGroupByName.TryGetValue(groupName, out poolGroup))
            return CreatePool(original, path, count, poolGroup.Root, action);
        else // 없으면 새로운 풀 그룹 생성
        {
            poolGroup = new PoolGroup();
            poolGroup.Init(groupName);
            poolGroup.Root.SetParent(_root);
            _poolGroupByName.Add(groupName, poolGroup);
            return CreatePool(original, path, count, poolGroup.Root, action);
        }
    }


    public void Push(Poolable poolable) => Push(poolable.gameObject);
    public void Push(GameObject go)
    {
        Pool pool = FindPool(go.name);

        if (pool == null)
        {
            Multi_Managers.Resources.PhotonDestroy(go);
            return;
        }

        go.GetOrAddComponent<RPCable>().SetActive_RPC(false);
        go.GetOrAddComponent<RPCable>().SetPosition_RPC(Vector3.one * 1000);
        pool.Push(go.GetComponent<Poolable>());
    }

    public Poolable Pop(GameObject go) => FindPool(go.name).Pop();

    Pool FindPool(string name)
    {
        if (_poolByName.ContainsKey(name) == false)
        {
            Debug.Log($"{name} 풀링 오브젝트를 딕셔너리에서 찾을 수 없음");
            return null;
        }
        else return _poolByName[name];
    }

    public GameObject GetOriginal(string path)
    {
        string name = path.Split('/')[path.Split('/').Length-1];
        if (_poolByName.TryGetValue(name, out Pool pool)) return pool.Original;
        else return null;
    }

    public void Clear()
    {
        if(_root != null)
        {
            _root = null;
            _poolByName.Clear();
            _poolGroupByName.Clear();
        }
    }
}