using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Photon.Pun;

public class Multi_TowerEnemySpawner : Multi_EnemySpawnerBase
{
    public event Action<Multi_EnemyTower> OnSpawn;
    public event Action<Multi_EnemyTower> OnDead;
    RPCData<int> _towerLevel = new RPCData<int>();

    protected override void Init()
    {
        Multi_GameManager.instance.OnStart += Spawn;
    }

    protected override void MasterInit()
    {
        OnDead += AfterSpawn;
        CreatePool();
    }

    void CreatePool()
    {
        for (int i = 0; i < _enemys.Length; i++)
        {
            List<Multi_EnemyTower> towers = CreatePool_InGroup<Multi_EnemyTower>(_enemys[i], BuildPath(_rootPath, _enemys[i]), spawnCount).ToList();
            towers.ForEach(x => SetTower(x));
        }
    }

    void SetTower(Multi_EnemyTower enemy)
    {
        enemy.enemyType = EnemyType.Tower;

        if (PhotonNetwork.IsMasterClient == false) return;
        enemy.OnDeath += () => OnDead(enemy);
    }

    void AfterSpawn(Multi_EnemyTower tower)
    {
        if (_enemys.Length > tower.Level)
            StartCoroutine(Co_AfterSpawn(tower.GetComponent<RPCable>().UsingId));
    }
    IEnumerator Co_AfterSpawn(int id)
    {
        yield return new WaitForSeconds(5f);
        Spawn(id);
    }

    void Spawn() => Spawn(Multi_Data.instance.Id);
    void Spawn(int id) => Spawn_RPC(BuildPath(_rootPath, _enemys[_towerLevel.Get(id)]), Multi_Data.instance.EnemyTowerWorldPositions[id], id);

    [PunRPC]
    protected override GameObject BaseSpawn(string path, Vector3 spawnPos, Quaternion rotation, int id)
    {
        Multi_EnemyTower enemy = base.BaseSpawn(path, spawnPos, rotation, id).GetComponent<Multi_EnemyTower>();
        _towerLevel.Set(id, _towerLevel.Get(id) + 1);
        print($"ID : {id}에서 레벨 {_towerLevel.Get(id)} 짜리 타워 스폰");
        enemy.Spawn(_towerLevel.Get(id));
        OnSpawn?.Invoke(enemy);
        return null;
    }
}
