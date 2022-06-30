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
    public void SpawnUnit(int colorNumber, int classNumber)
    {
        Multi_SpawnManagers.NormalUnit.Spawn(colorNumber, classNumber);
    }

    public void SpawnUnit_ByClient(int colorNumber, int classNumber)
    {
        photonView.RPC("SpawnUnit", RpcTarget.Others, colorNumber, classNumber);
    }

    public void SpawnNormalEnemy(int _enemyNum)
    {
        Multi_SpawnManagers.NormalEnemy.Spawn(_enemyNum, Multi_StageManager.Instance.CurrentStage);
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