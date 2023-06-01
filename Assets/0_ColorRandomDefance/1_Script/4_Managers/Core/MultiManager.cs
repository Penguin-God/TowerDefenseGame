using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;
using System;
using System.IO;

public class MultiManager
{
    MultiInstantiater _multiInstantiater = new MultiInstantiater();
    public MultiInstantiater Instantiater => _multiInstantiater;

    public Transform GetPhotonViewTransfrom(int viewID)
    {
        return PhotonView.Find(viewID).transform;
    }

    public T GetPhotonViewComponent<T>(int viewID) => GetPhotonViewTransfrom(viewID).GetComponent<T>();

    public class MultiInstantiater : IInstantiater
    {
        public GameObject Instantiate(string path) // interface
        {
            path = GetPrefabPath(path);
            var prefab = Managers.Resources.Load<GameObject>(path);
            var go = PhotonNetwork.Instantiate(path, Vector3.zero * 1000, prefab.transform.rotation);
            go.GetOrAddComponent<RPCable>();
            return go;
        }

        public GameObject PhotonInstantiate(string path) => PhotonInstantiate(path, Vector3.zero, -1);

        public GameObject PhotonInstantiate(string path, Vector3 spawnPos, int id = -1)  => PhotonInstantiate(path, spawnPos, Quaternion.identity, id);
        
        public GameObject PhotonInstantiate(string path, Vector3 spawnPos, Quaternion spawnRot, int id = -1)
            => PhotonInstantiate(path, spawnPos, spawnRot, true, (byte)(id == -1 ? PlayerIdManager.InVaildId : id));

        public GameObject PhotonInstantiateInactive(string path, byte id) => PhotonInstantiate(path, Vector3.zero, Quaternion.identity, false, id);

        GameObject PhotonInstantiate(string path, Vector3 spawnPos, Quaternion spawnRot, bool activeFlag, byte id)
        {
            path = GetPrefabPath(path);
            var result = Managers.Pool.TryGetPoolObejct(GetPathName(path), out GameObject poolGo) ? poolGo : Instantiate(path);
            var rpc = result.GetComponent<RPCable>();
            if (spawnPos != Vector3.zero) rpc.SetPosition_RPC(spawnPos);
            if (spawnRot != Quaternion.identity) rpc.SetRotate_RPC(spawnRot.eulerAngles);
            if (id != PlayerIdManager.InVaildId) rpc.SetId_RPC(id);
            if(activeFlag) rpc.SetActive_RPC(true);
            return result;
        }

        // TODO : 소환이랑 정보 설정 분리하기
        public void SetMultiInfo()
        {

        }

        string GetPrefabPath(string path) => path.Contains("Prefabs/") ? path : $"Prefabs/{path}";
        string GetPathName(string path) => path.Split('/').Last();

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
}
