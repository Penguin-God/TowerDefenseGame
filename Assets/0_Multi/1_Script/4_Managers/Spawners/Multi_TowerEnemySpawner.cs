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

    public RPCAction rpcOnDead = new RPCAction();
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
            CreatePoolGroup(_enemys[i], BuildPath(_rootPath, _enemys[i]), spawnCount);
    }

    protected override void SetPoolObj(GameObject go)
    {
        var enemy = go.GetComponent<Multi_EnemyTower>();
        enemy.enemyType = EnemyType.Tower;
        enemy.OnDeath += () => OnDead(enemy);
        enemy.OnDeath += () => rpcOnDead.RaiseEvent(enemy.UsingId);
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
    void Spawn(int id) => Spawn_RPC(BuildPath(_rootPath, _enemys[_towerLevel.Get(id)]), spawnPositions[id], id);

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
