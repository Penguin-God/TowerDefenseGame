using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;
using System;

public class MultiManager
{
    MultiInstantiater _multiInstantiater = new MultiInstantiater();
    public MultiInstantiater Instantiater => _multiInstantiater;

    MultiDataManager _multiDataManager;
    public MultiDataManager Data => _multiDataManager;

    public void Init()
    {
        _multiDataManager = new MultiDataManager();
        if (PhotonNetwork.IsMasterClient)
            _multiDataManager.Init();
    }

    public void Clear()
    {
        _multiDataManager = null;
    }

    public Transform GetPhotonViewTransfrom(int viewID)
    {
        return PhotonView.Find(viewID).transform;
    }

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

        public GameObject PhotonInstantiate(string path, int id) => PhotonInstantiate(path, Vector3.zero, id);

        public GameObject PhotonInstantiate(string path, Vector3 spawnPos, int id = -1)  => PhotonInstantiate(path, spawnPos, Quaternion.identity, id);
        
        public GameObject PhotonInstantiate(string path, Vector3 spawnPos, Quaternion spawnRot, int id = -1)
        {
            return PhotonInstantiate(path, spawnPos, spawnRot, true, id);
            //path = GetPrefabPath(path);
            //var result = Managers.Pool.TryGetPoolObejct(GetPathName(path), out GameObject poolGo) ? poolGo : Instantiate(path);
            //var rpc = result.GetComponent<RPCable>();
            //if (spawnPos != Vector3.zero) rpc.SetPosition_RPC(spawnPos);
            //if (spawnRot != Quaternion.identity) rpc.SetRotate_RPC(spawnRot.eulerAngles);
            //if (id != -1) rpc.SetId_RPC(id);
            //rpc.SetActive_RPC(true);
            //return result;
        }

        public void PhotonInstantiateInactive(string path, Vector3 spawnPos, Quaternion spawnRot, int id) => PhotonInstantiate(path, spawnPos, spawnRot, false, id);

        GameObject PhotonInstantiate(string path, Vector3 spawnPos, Quaternion spawnRot, bool activeFlag, int id = -1)
        {
            path = GetPrefabPath(path);
            var result = Managers.Pool.TryGetPoolObejct(GetPathName(path), out GameObject poolGo) ? poolGo : Instantiate(path);
            var rpc = result.GetComponent<RPCable>();
            if (spawnPos != Vector3.zero) rpc.SetPosition_RPC(spawnPos);
            if (spawnRot != Quaternion.identity) rpc.SetRotate_RPC(spawnRot.eulerAngles);
            if (id != -1) rpc.SetId_RPC(id);
            if(activeFlag) rpc.SetActive_RPC(true);
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
}

public class MultiDataManager
{
    public byte PlayerID => (byte)(PhotonNetwork.IsMasterClient ? 0 : 1);

    RPCData<Dictionary<UnitFlags, UnitStat>> _unitStatData = new RPCData<Dictionary<UnitFlags, UnitStat>>();

    public void Init()
    {
        _unitStatData.Set(0, GetUnitStatData());
        _unitStatData.Set(1, GetUnitStatData());

        Dictionary<UnitFlags, UnitStat> GetUnitStatData() => Managers.Data.Unit.UnitStatByFlag.ToDictionary(x => x.Key, x => x.Value.GetClone());
    }

    public UnitStat GetUnitStat(UnitFlags flag) => GetUnitStat(PlayerID, flag);
    public UnitStat GetUnitStat(byte id, UnitFlags flag) => _unitStatData.Get(id)[flag].GetClone();
    public IEnumerable<UnitStat> GetUnitStats(Func<UnitFlags, bool> condition)
        => GetUnitStats(PlayerID, condition);
    public IEnumerable<UnitStat> GetUnitStats(byte id, Func<UnitFlags, bool> condition)
        => _unitStatData.Get(id).Keys.Where(condition).Select(x => GetUnitStat(x));

    // 외부로 열어둔 GetUnitStats는 클론 주는 거라서 바꿔봤자 의미 없음
    public void ChangeUnitStat(int id, Action<UnitStat> action, Func<UnitFlags, bool> conditoin)
        => _unitStatData
            .Get(id)
            .Values
            .Where(x => conditoin(x.Flag))
            .ToList().ForEach(action);
}