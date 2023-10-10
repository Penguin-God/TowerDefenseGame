using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IInstantiater
{
    GameObject Instantiate(string path);
}

public class PoolGroup
{
    public Transform Root { get; set; }
    
    public void Init(string rootName)
    {
        Root = new GameObject($"{rootName}_Root").transform;
    }
}

public class Pool
{
    public Transform Root { get; set; } = null;
    public GameObject Original { get; private set; }
    readonly string Path;
    public string Name => Path.Split('/').Last();

    Queue<Poolable> poolStack = new Queue<Poolable>();

    IInstantiater _instantiater;

    public Pool(string path, int count, IInstantiater instantiater)
    {
        Path = path;
        _instantiater = instantiater;
        Original = Resources.Load<GameObject>(path);
        Root = new GameObject($"{Name}_Root").transform;
        for (int i = 0; i < count; i++)
            Push(CreateObject());
    }

    Poolable CreateObject()
    {
        var go = _instantiater == null ? GameObject.Instantiate(Resources.Load<GameObject>(Path)) : _instantiater.Instantiate(Path);
        go.SetActive(false);
        go.transform.SetParent(Root);
        go.name = Name;

        Poolable poolable = go.GetOrAddComponent<Poolable>();
        poolable.Path = Path;

        return poolable;
    }

    public void Push(Poolable poolable)
    {
        if (poolable == null) return;

        poolable.transform.SetParent(Root);
        poolable.IsUsing = false;
        poolStack.Enqueue(poolable);
    }

    public Poolable Pop()
    {
        Poolable poolable;

        if (poolStack.Count > 0) poolable = poolStack.Dequeue();
        else poolable = CreateObject();

        poolable.transform.SetParent(null);
        poolable.IsUsing = true;
        return poolable;
    }
}

public class PoolManager
{
    Transform _root;

    Dictionary<string, PoolGroup> _poolGroupByName = new Dictionary<string, PoolGroup>();
    Dictionary<string, Pool> _poolByName = new Dictionary<string, Pool>();

    public PoolManager(string name) => _root = new GameObject(name).transform;

    public Pool CreatePool(string path, int count, IInstantiater instantiater = null, Transform root = null)
    {
        Pool pool = new Pool(path, count, instantiater);
        if (root == null) pool.Root.SetParent(_root);
        else pool.Root.SetParent(root);
        _poolByName.Add(pool.Name, pool);
        return pool;
    }

    public void CreatePool_InGroup(string path, int count, string groupName, IInstantiater instantiater = null)
    {
        PoolGroup poolGroup;
        if(_poolGroupByName.ContainsKey(groupName))
            poolGroup = _poolGroupByName[groupName];
        else // 없으면 새로운 풀 그룹 생성
        {
            poolGroup = new PoolGroup();
            poolGroup.Init(groupName);
            poolGroup.Root.SetParent(_root);
            _poolGroupByName.Add(groupName, poolGroup);
        }
        CreatePool(path, count, instantiater, poolGroup.Root);
    }

    public void Push(Poolable poolable) => Push(poolable.gameObject);
    public void Push(GameObject go)
    {
        Pool pool = FindPool(go.name);
        Debug.Assert(pool != null, $"{go.name} 오브젝트의 풀이 없는데 푸쉬를 시도함");

        go.SetActive(false);
        pool.Push(go.GetComponent<Poolable>());
    }

    Pool FindPool(string name)
    {
        if (_poolByName.ContainsKey(name) == false) return null;
        else return _poolByName[name];
    }

    public bool ContainsPool(string name) => _poolByName.ContainsKey(name);

    public bool TryGetPoolObejct(string name, out GameObject poolGo)
    {
        Pool pool = FindPool(name);
        if (pool == null)
        {
            poolGo = null;
            return false;
        }
        else
        {
            poolGo = pool.Pop().gameObject;
            return true;
        }
    }

    public GameObject GetOriginal(string path)
    {
        string name = path.Split('/')[path.Split('/').Length-1];
        if (_poolByName.TryGetValue(name, out Pool pool)) return pool.Original;
        else return null;
    }
}
