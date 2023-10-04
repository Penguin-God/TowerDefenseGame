using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourcesManager : IInstantiater
{
    public T Load<T>(string path) where T : Object
    {
        if(typeof(T) == typeof(GameObject))
        {
            string goPath = path.Substring(path.IndexOf('/') + 1);
            
            GameObject go = Managers.Pool.GetOriginal(goPath);
            if (go != null) return go as T;
        }

        Debug.Assert(Resources.Load<T>(path) != null, $"찾을 수 없는 리소스 경로 : {path}");
        return Resources.Load<T>(path);
    }

    PoolManager PoolManager => Managers.Pool;
    public GameObject Instantiate(string path) => Instantiate(GetPrefabPath(path), Vector3.zero);

    public GameObject Instantiate(string path, Vector3 spawnPos)
    {
        GameObject result = CreateObject(path);
        result.transform.position = spawnPos;
        result.SetActive(true);
        return result;
    }

    GameObject CreateObject(string path)
    {
        if (Managers.Pool.TryGetPoolObejct(path.Split('/').Last(), out GameObject poolGo))
            return poolGo;

        GameObject result = Object.Instantiate(Load<GameObject>(path), Vector3.zero, Load<GameObject>(path).transform.rotation);
        if (result.GetComponent<Poolable>() != null && PoolManager.ContainsPool(result.name) == false)
            PoolManager.CreatePool(result.name, 0, null, this);
        return result;
    }
    string GetPrefabPath(string path) => path.Contains("Prefabs/") ? path : $"Prefabs/{path}";

    public void Destroy(GameObject go)
    {
        if (go.GetComponent<Poolable>() != null)
            Managers.Pool.Push(go);
        else
            Object.Destroy(go);
    }
}
