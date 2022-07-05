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

    public GameObject PhotonInsantiate(GameObject PoolObj, Vector3 position, Transform parent = null) 
        => SetPhotonObject(Multi_Managers.Pool.Pop(PoolObj).gameObject, position, Quaternion.identity);

    // public GameObject PhotonInsantiate(string path, Transform parent = null) => PhotonInsantiate(path, Vector3.zero, Quaternion.identity, parent);

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

    public GameObject PhotonInsantiate(string path, Vector3 position, int id, Transform parent = null)
    {
        GameObject result = GetObject(path);

        if (result != null)
        {
            return SetPhotonObject(result, position, Quaternion.identity, id, parent);
            //result.transform.SetParent(parent);
            //result.GetOrAddComponent<Poolable>().SetId_RPC(id);
            //Spawn_RPC(result, position, Quaternion.identity);
        }

        return result;
    }

    GameObject GetObject(string path)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");
        if (prefab.GetComponent<Poolable>() != null)
            return Multi_Managers.Pool.Pop(prefab).gameObject;
        else
            return PhotonNetwork.Instantiate($"Prefabs/{path}", Vector3.zero, Quaternion.identity);
    }

    GameObject SetPhotonObject(GameObject go, Vector3 position, Quaternion rotation, int id = -1, Transform parent = null)
    {
        if (go == null) return null;

        go.transform.SetParent(parent);
        // TODO : Poolable말고 동기화 전용 컴포넌트 하나 만들기
        go.GetOrAddComponent<Poolable>().SetId_RPC(id);
        Spawn_RPC();
        return go;

        void Spawn_RPC()
        {
            PhotonView pv = go.GetOrAddComponent<PhotonView>();

            RPC_Utility.Instance.RPC_Position(pv.ViewID, position);
            RPC_Utility.Instance.RPC_Rotation(pv.ViewID, rotation);
            RPC_Utility.Instance.RPC_Active(pv.ViewID, true);
        }
    }


    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject original = Load<GameObject>($"Prefabs/{path}");
        GameObject go = Object.Instantiate(original, parent);
        go.name = original.name;
        return go;
    }

    public GameObject Instantiate_UI(string path, Transform parent = null)
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
