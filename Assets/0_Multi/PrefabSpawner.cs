﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PrefabSpawner : MonoBehaviourPun
{
    public void SpawnUnit(string _name)
    {
        PhotonNetwork.Instantiate(_name, Vector3.zero, Quaternion.identity);
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
}