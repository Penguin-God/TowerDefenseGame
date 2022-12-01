using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MultiManager
{
    MultiInstantiater _multiInstantiater = new MultiInstantiater();
    public MultiInstantiater Instantiater => _multiInstantiater;

    public class MultiInstantiater : IInstantiater
    {
        public GameObject Instantiate(string path)
        {
            path = GetPrefabPath(path);
            var prefab = Managers.Resources.Load<GameObject>(path);
            var go = PhotonNetwork.Instantiate(path, Vector3.zero * 1000, prefab.transform.rotation);
            go.GetOrAddComponent<RPCable>();
            return go;
        }

        public GameObject PhotonInstantiate(string path, Vector3 spawnPos, int id = -1)
        {
            path = GetPrefabPath(path);
            var result = Managers.Pool.TryGetPoolObejct(GetPathName(path), out GameObject poolGo) ? poolGo : Instantiate(path);
            var rpc = result.GetComponent<RPCable>();
            rpc.SetPosition_RPC(spawnPos);
            rpc.SetActive_RPC(true);
            if (id != -1) rpc.SetId_RPC(id);
            return result;
        }
        public GameObject PhotonInstantiate(string path, Vector3 spawnPos, Quaternion spawnRot, int id = -1)
            => PhotonInstantiate(path, spawnPos, spawnRot.eulerAngles, id);

        public GameObject PhotonInstantiate(string path, Vector3 spawnPos, Vector3 spawnEuler, int id = -1)
        {
            var result = PhotonInstantiate(path, spawnPos, id);
            result.GetComponent<RPCable>().SetRotate_RPC(spawnEuler);
            return result;
        }

        string GetPrefabPath(string path) => path.Contains("Prefabs/") ? path : $"Prefabs/{path}";
        string GetPathName(string path) => path.Split('/')[path.Split('/').Length - 1];

        public void PhotonDestroy(GameObject go)
        {
            if (PhotonNetwork.IsMasterClient == false) return;
            if (go.GetComponent<Poolable>() != null)
            {
                go.GetComponent<RPCable>().SetActive_RPC(false);
                Managers.Pool.Push(go.GetComponent<Poolable>());
            }
            else
                PhotonNetwork.Destroy(go);
        }
    }

    public Transform GetPhotonViewTransfrom(int viewID)
    {
        return PhotonView.Find(viewID).transform;
    }
}
