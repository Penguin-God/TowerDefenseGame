using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_ResourcesManager
{
    public T Load<T>(string path) where T : Object
    {
        if(typeof(T) == typeof(GameObject))
        {
            string goPath = path.Substring(path.IndexOf('/') + 1);
            
            GameObject go = Multi_Managers.Pool.GetOriginal(goPath);
            //if (go != null) Debug.Log("Get Original");
            if (go != null) return go as T;
        }

        Debug.Assert(Resources.Load<T>(path) != null, $"찾을 수 없는 리소스 경로 : {path}");
        return Resources.Load<T>(path);
    }

    public GameObject PhotonInsantiate(string path, Transform parent = null) => PhotonInsantiate(path, Vector3.zero, Quaternion.identity, parent);

    public GameObject PhotonInsantiate(string path, Vector3 position, Transform parent = null) 
        => PhotonInsantiate(path, position, Quaternion.identity, parent);

    public GameObject PhotonInsantiate(string path, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");

        if (prefab.GetComponent<Poolable>() != null)
            return Multi_Managers.Pool.Pop(prefab, position, rotation, parent).gameObject;

        prefab = PhotonNetwork.Instantiate($"Prefabs/{path}", position, rotation);
        if (parent != null) prefab.transform.SetParent(parent);
        return prefab;
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject original = Load<GameObject>($"Prefabs/{path}");
        GameObject go = Object.Instantiate(original, parent);
        go.name = original.name;
        return go;
    }

    public void PhotonDestroy(GameObject go)
    {
        PhotonNetwork.Destroy(go);
    }

    public void Destroy(GameObject go) => Object.Destroy(go);
}
