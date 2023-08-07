using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class Multi_BossEnemySpawner : Multi_SpawnerBase
{
    public event Action<Multi_BossEnemy> OnDead;

    public RPCAction rpcOnSpawn = new RPCAction();
    public RPCAction rpcOnDead = new RPCAction();

    protected override void SetSpawnObj(GameObject go)
    {
        var enemy = go.GetComponent<Multi_BossEnemy>();
        enemy.OnDeath += () => OnDead(enemy);
        enemy.OnDeath += () => rpcOnDead.RaiseEvent(enemy.UsingId);
    }

    const int SpawnableObjectCount = 4;
    string BulildBossPath() => PathBuilder.BuildBossMonsterPath(Random.Range(0, SpawnableObjectCount));

    SpeedManagerCreater _speedManagerCreater;
    public void Inject(SpeedManagerCreater speedManagerCreater) => _speedManagerCreater = speedManagerCreater;
    public Multi_BossEnemy SpawnBoss(byte id, int bossLevel)
    {
        var boss = Managers.Multi.Instantiater.PhotonInstantiateInactive(BulildBossPath(), id).GetComponent<Multi_BossEnemy>();
        Multi_EnemyManager.Instance.SetSpawnBoss(id, boss);
        var bossData = Managers.Data.BossDataByLevel[bossLevel];
        boss.Inject(bossData, _speedManagerCreater.CreateSpeedManager(bossData.Speed, boss));
        SetSpawnObj(boss.gameObject);
        rpcOnSpawn?.RaiseEvent(id);
        return boss;
    }
}
