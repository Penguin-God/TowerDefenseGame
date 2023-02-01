using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;
using Random = UnityEngine.Random;

public class Multi_BossEnemySpawner : Multi_EnemySpawnerBase
{
    public event Action<Multi_BossEnemy> OnSpawn;
    public event Action<Multi_BossEnemy> OnDead;

    public RPCAction rpcOnSpawn = new RPCAction();
    public RPCAction rpcOnDead = new RPCAction();

    protected override void MasterInit()
    {
        CreatePool();
    }

    void CreatePool()
    {
        for (int i = 0; i <  _spawnableObjectCount; i++)
            CreatePoolGroup(new SpawnPathBuilder().BuildBossMonsterPath(i), spawnCount);
    }

    protected override void SetPoolObj(GameObject go)
    {
        var enemy = go.GetComponent<Multi_BossEnemy>();
        enemy.enemyType = EnemyType.Boss;
        enemy.OnDeath += () => OnDead(enemy);
        enemy.OnDeath += () => rpcOnDead.RaiseEvent(enemy.UsingId);
        enemy.OnDeath += () => Managers.Multi.Instantiater.PhotonDestroy(enemy.gameObject);
    }

    public void Spawn(int id)
    {
        bossLevel++; 
        Spawn_RPC(new SpawnPathBuilder().BuildBossMonsterPath(Random.Range(0, _spawnableObjectCount)), Vector3.zero, id);
    }

    [SerializeField] int bossLevel;

    [PunRPC]
    protected override GameObject BaseSpawn(string path, Vector3 spawnPos, Quaternion rotation, int id)
    {
        Multi_BossEnemy enemy = base.BaseSpawn(path, spawnPositions[id], rotation, id).GetComponent<Multi_BossEnemy>();
        enemy.Spawn(bossLevel);
        OnSpawn?.Invoke(enemy);
        rpcOnSpawn?.RaiseEvent(id);
        return null;
    }
}
