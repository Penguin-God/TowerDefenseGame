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
            Debug.Log(goPath);
            GameObject go = Multi_Managers.Pool.GetOriginal(goPath);
            if (go != null) return go as T;
        }

        return Resources.Load<T>(path);
    }

    public GameObject PhotonInsantiate(string path, Transform parent = null) => PhotonInsantiate(path, Vector3.zero, Quaternion.identity, parent);

    public GameObject PhotonInsantiate(string path, Vector3 position, Transform parent = null) 
        => PhotonInsantiate(path, position, Quaternion.identity, parent);

    public GameObject PhotonInsantiate(string path, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");
        if(prefab == null)
        {
            Debug.LogWarning($"찾을 수 없는 리소스 경로 {path}");
            return null;
        }

        if (prefab.GetComponent<Poolable>() != null)
            return Multi_Managers.Pool.Pop(path, parent).gameObject;
        
        prefab = PhotonNetwork.Instantiate($"Prefabs/{path}", position, rotation);
        if (parent != null) prefab.transform.SetParent(parent);
        return prefab;
    }

    public void PhotonDestroy(GameObject go)
    {
        PhotonNetwork.Destroy(go);
    }
}
