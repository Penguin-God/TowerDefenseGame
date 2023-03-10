using System.Collections;
using System.Collections.Generic;
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

    public GameObject Instantiate(string path) => Instantiate(path, null);
    public GameObject Instantiate(string path, Transform parent = null)
    {
        path = GetPrefabPath(path);
        if (Managers.Pool.TryGetPoolObejct(GetPathName(path), out GameObject poolGo))
        {
            poolGo.SetActive(true);
            return poolGo;
        }

        var original = Load<GameObject>(path);
        GameObject go = Object.Instantiate(original, parent);
        go.name = original.name;
        return go;
    }

    string GetPathName(string path) => path.Split('/')[path.Split('/').Length - 1];
    string GetPrefabPath(string path) => path.Contains("Prefabs/") ? path : $"Prefabs/{path}";

    public void Destroy(GameObject go)
    {
        if (go.GetComponent<Poolable>() != null)
            Managers.Pool.Push(go);
        else
            Object.Destroy(go);
    }
}
