using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using Photon.Pun;

public class Multi_BossEnemySpawner : Multi_SpawnerBase
{
    public event Action<Multi_BossEnemy> OnDead;
    public RPCAction rpcOnDead = new RPCAction();

    protected override void SetSpawnObj(GameObject go)
    {
        var enemy = go.GetComponent<Multi_BossEnemy>();
        enemy.OnDeath += () => OnDead?.Invoke(enemy);
        enemy.OnDeath += () => rpcOnDead.RaiseEvent(enemy.UsingId);
    }

    const int SpawnableObjectCount = 4;
    string BulildBossPath() => PathBuilder.BuildBossMonsterPath(Random.Range(0, SpawnableObjectCount));

    MonsterDecorator _monsterDecorator;
    public void Inject(MonsterDecorator monsterDecorator) => _monsterDecorator = monsterDecorator;
    public Multi_BossEnemy SpawnBoss(byte id, int bossLevel)
    {
        var boss = Managers.Multi.Instantiater.PhotonInstantiateInactive(BulildBossPath(), id).GetComponent<Multi_BossEnemy>();
        photonView.RPC(nameof(InjectMonster), RpcTarget.All, (byte)bossLevel, boss.GetComponent<PhotonView>().ViewID);
        SetSpawnObj(boss.gameObject);
        return boss;
    }

    [PunRPC]
    void InjectMonster(byte level, int viewId)
    {
        var monster = Managers.Multi.GetPhotonViewComponent<Multi_BossEnemy>(viewId);
        var bossData = Managers.Data.BossDataByLevel[level];
        Multi_EnemyManager.Instance.SetSpawnBoss(monster.UsingId, monster);
        Managers.Sound.PlayBgm(BgmType.Boss);

        _monsterDecorator.DecorateSpeedSystem(bossData.Speed, monster);
        monster.Inject(bossData);
    }
}
