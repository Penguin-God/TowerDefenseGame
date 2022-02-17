using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PrefabSpawner : MonoBehaviourPun
{
    [PunRPC]
    public void SpawnUnit(string _name)
    {
        PhotonNetwork.Instantiate(_name, Multi_Data.instance.UnitSpawnPos, Quaternion.identity);
    }

    public void SpawnUnit_ByClient(string _name)
    {
        photonView.RPC("SpawnUnit", RpcTarget.Others, _name);
    }

    public void SpawnNormalEnemy(string _name, int _enemyNum)
    {
        Vector3 _spawnPos = Multi_Data.instance.EnemySpawnPos;

        int _stage = Multi_EnemySpawner.instance.stageNumber;
        if (_stage < 1) _stage = 1;

        int _hp = Multi_EnemySpawner.instance.debugData[_stage - 1].hp;
        float _speed = Multi_EnemySpawner.instance.debugData[_stage - 1].speed;
        
        GameObject _enemy = Multi_EnemySpawner.instance.GetPoolEnemy(_enemyNum);
        _enemy.GetComponent<Multi_NormalEnemy>().photonView.RPC("SetPos", RpcTarget.All, _spawnPos);
        _enemy.GetComponent<Multi_NormalEnemy>().photonView.RPC("Setup", RpcTarget.All, _hp, _speed);
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