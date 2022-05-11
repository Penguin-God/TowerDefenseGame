using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[System.Serializable]
public class DrawButtonUnits
{
    public GameObject[] units;
}

public class PrefabSpawner : MonoBehaviourPun
{
    public DrawButtonUnits[] allUnit;

    [PunRPC]
    public void SpawnUnit(int classNumber, string _name)
    {
        string path = Multi_UnitManager.instance.BuildPath((UnitClass)classNumber, _name);
        Multi_Managers.Resources.PhotonInsantiate(path, Multi_WorldPosUtility.Instance.GetUnitSpawnPositon());
    }

    public void SpawnUnit_ByClient(string _name)
    {
        photonView.RPC("SpawnUnit", RpcTarget.Others, _name);
    }

    public void SpawnNormalEnemy(int _enemyNum)
    {
        Multi_SpawnManagers.NormalEnemy.Spawn(_enemyNum);
    }

    public void AllUnitSpawn_ByEditor()
    {
        GameObject[] _prefabs = Resources.LoadAll<GameObject>("");
        foreach (GameObject _unit in _prefabs)
        {
            if (_unit.GetComponentInChildren<TeamSoldier>() == null) continue;
            Instantiate(_unit);
        }
    }
}