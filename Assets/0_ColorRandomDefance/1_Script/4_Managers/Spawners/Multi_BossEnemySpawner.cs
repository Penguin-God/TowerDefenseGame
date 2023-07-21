using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;
using Random = UnityEngine.Random;

public class Multi_BossEnemySpawner : Multi_EnemySpawnerBase
{
    public event Action<Multi_BossEnemy> OnDead;

    public RPCAction rpcOnSpawn = new RPCAction();
    public RPCAction rpcOnDead = new RPCAction();

    protected override void SetPoolObj(GameObject go)
    {
        var enemy = go.GetComponent<Multi_BossEnemy>();
        enemy.OnDeath += () => OnDead(enemy);
        enemy.OnDeath += () => rpcOnDead.RaiseEvent(enemy.UsingId);
    }

    public void Spawn(int id)
    {
        bossLevel++;
        Spawn_RPC(PathBuilder.BuildBossMonsterPath(Random.Range(0, _spawnableObjectCount)), Vector3.zero, id);
    }

    const int SpawnableObjectCount = 4;
    string BulildBossPath() => PathBuilder.BuildBossMonsterPath(Random.Range(0, SpawnableObjectCount));

    [SerializeField] int bossLevel;

    [PunRPC]
    protected override GameObject BaseSpawn(string path, Vector3 spawnPos, Quaternion rotation, int id)
    {
        Multi_BossEnemy enemy = base.BaseSpawn(path, spawnPositions[id], rotation, id).GetComponent<Multi_BossEnemy>();
        Multi_EnemyManager.Instance.SetSpawnBoss(id, enemy);
        enemy.Spawn(bossLevel);
        SetPoolObj(enemy.gameObject);
        rpcOnSpawn?.RaiseEvent(id);
        return null;
    }

    SpeedManagerCreater _speedManagerCreater;
    public void Inject(SpeedManagerCreater speedManagerCreater) => _speedManagerCreater = speedManagerCreater;
    public Multi_BossEnemy SpawnBoss(byte id, int bossLevel)
    {
        var boss = Managers.Multi.Instantiater.PhotonInstantiateInactive(BulildBossPath(), id).GetComponent<Multi_BossEnemy>();
        Multi_EnemyManager.Instance.SetSpawnBoss(id, boss);
        var bossData = Managers.Data.BossDataByLevel[bossLevel];
        boss.Inject(bossData, _speedManagerCreater.CreateSpeedManager(bossData.Speed, boss));
        SetPoolObj(boss.gameObject);
        rpcOnSpawn?.RaiseEvent(id);
        return boss;
    }
}
