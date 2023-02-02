using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class Multi_TowerEnemySpawner : Multi_EnemySpawnerBase
{
    public event Action<Multi_EnemyTower> OnDead;

    public RPCAction rpcOnDead = new RPCAction();
    RPCData<int> _towerLevel = new RPCData<int>();

    protected override void MasterInit() => OnDead += AfterSpawn;

    protected override void SetPoolObj(GameObject go)
    {
        var enemy = go.GetComponent<Multi_EnemyTower>();
        enemy.OnDeath += () => OnDead(enemy);
        enemy.OnDeath += () => rpcOnDead.RaiseEvent(enemy.UsingId);
    }

    void AfterSpawn(Multi_EnemyTower tower)
    {
        if (_spawnableObjectCount > tower.Level)
            StartCoroutine(Co_AfterSpawn(tower.GetComponent<RPCable>().UsingId));
    }
    IEnumerator Co_AfterSpawn(int id)
    {
        yield return new WaitForSeconds(5f);
        Spawn(id);
    }

    public void Spawn(int id) => Spawn_RPC(new SpawnPathBuilder().BuildEnemyTowerPath(_towerLevel.Get(id) + 1), spawnPositions[id], id);

    [PunRPC]
    protected override GameObject BaseSpawn(string path, Vector3 spawnPos, Quaternion rotation, int id)
    {
        Multi_EnemyTower tower = base.BaseSpawn(path, spawnPos, rotation, id).GetComponent<Multi_EnemyTower>();
        _towerLevel.Set(id, _towerLevel.Get(id) + 1);
        tower.Spawn(_towerLevel.Get(id));
        SetPoolObj(tower.gameObject);
        Multi_EnemyManager.Instance.SetSpawnTower(id, tower);
        return null;
    }
}
