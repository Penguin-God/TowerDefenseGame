using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourcesManager
{
    public T Load<T>(string path) where T : Object
    {
        if(typeof(T) == typeof(GameObject))
        {
            string goPath = path.Substring(path.IndexOf('/') + 1);
            
            GameObject go = _poolManager.GetOriginal(goPath);
            if (go != null) return go as T;
        }

        Debug.Assert(Resources.Load<T>(path) != null, $"찾을 수 없는 리소스 경로 : {path}");
        return Resources.Load<T>(path);
    }

    public T[] LoadCsv<T>(string path) => CsvUtility.CsvToArray<T>(Load<TextAsset>($"Data/{path}").text);

    PoolManager _poolManager;
    public void DependencyInject(PoolManager poolManager) => _poolManager = poolManager;

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
        GameObject prefab = Load<GameObject>(GetPrefabPath(path));
        if (_poolManager.TryGetPoolObejct(path.Split('/').Last(), out GameObject poolGo))
            return poolGo;
        else if (prefab.GetComponent<Poolable>() != null && _poolManager.ContainsPool(prefab.name) == false)
            return _poolManager.CreatePool(path, 1).Pop().gameObject;
        else return Object.Instantiate(prefab, Vector3.zero, prefab.transform.rotation);
    }
    string GetPrefabPath(string path) => path.Contains("Prefabs/") ? path : $"Prefabs/{path}";

    public void Destroy(GameObject go)
    {
        if (go.GetComponent<Poolable>() != null)
            _poolManager.Push(go);
        else
            Object.Destroy(go);
    }
}
