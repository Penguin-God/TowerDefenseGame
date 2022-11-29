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
            if (go != null) return go as T;
        }

        Debug.Assert(Resources.Load<T>(path) != null, $"찾을 수 없는 리소스 경로 : {path}");
        return Resources.Load<T>(path);
    }

    public GameObject PhotonInsantiate(GameObject PoolObj, Vector3 position) 
        => SetPhotonObject(Multi_Managers.Pool.Pop(PoolObj).gameObject, position, PoolObj.transform.rotation);

    public GameObject PhotonInsantiate(string path, Vector3 position, int id = -1, Transform parent = null)
    {
        GameObject result = GetObject(path);
        if (result != null)
            return SetPhotonObject(result, position, result.transform.rotation, id, parent);

        return result;
    }

    public GameObject PhotonInsantiate(string path, Vector3 position, Quaternion rotation, int id = -1, Transform parent = null)
    {
        GameObject result = GetObject(path);
        if (result != null)
            return SetPhotonObject(result, position, rotation, id, parent);

        return result;
    }

    GameObject GetObject(string path)
    {
        path = GetPrefabPath(path); // TODO : 프리팹에 poolable 있어야 풀링되는거 수정하기
        GameObject prefab = Load<GameObject>(path);
        if (prefab.GetComponent<Poolable>() != null)
            return Multi_Managers.Pool.Pop(prefab).gameObject;
        else
            return PhotonNetwork.Instantiate(path, Vector3.zero, prefab.transform.rotation);
    }

    GameObject SetPhotonObject(GameObject go, Vector3 position, Quaternion rotation, int id = -1, Transform parent = null)
    {
        if (go == null) return null;

        go.transform.SetParent(parent);
        SetInfo_RPC();
        return go;

        void SetInfo_RPC()
        {
            RPCable rpcable = go.GetOrAddComponent<RPCable>();
            rpcable.SetId_RPC(id);
            rpcable.SetPosition_RPC(position);
            rpcable.SetRotation_RPC(rotation);
            rpcable.SetActive_RPC(true);
        }
    }


    public GameObject Instantiate(string path, Transform parent = null)
    {
        path = GetPrefabPath(path);
        var original = Multi_Managers.Pool.GetOriginal(path);
        if (original != null)
            return Multi_Managers.Pool.Pop(original).gameObject;

        original = Load<GameObject>(path);
        GameObject go = Object.Instantiate(original, parent);
        go.name = original.name;
        return go;
    }

    string GetPrefabPath(string path) => path.Contains("Prefabs/") ? path : $"Prefabs/{path}";

    public void PhotonDestroy(GameObject go)
    {
        PhotonNetwork.Destroy(go);
    }

    public void Destroy(GameObject go) => Object.Destroy(go);
}
