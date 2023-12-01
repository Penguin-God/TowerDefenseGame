using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using Photon.Pun;

public class Multi_BossEnemySpawner : MonoBehaviourPun
{
    public event Action<Multi_BossEnemy> OnDead;

    const int SpawnableObjectCount = 4;
    string BulildBossPath() => new ResourcesPathBuilder().BuildBossMonsterPath(Random.Range(0, SpawnableObjectCount));

    SpeedManagerCreator _monsterDecorator;
    UnitManagerController _unitManagerController;
    public void DependencyInject(SpeedManagerCreator monsterDecorator, UnitManagerController unitManagerController)
    {
        _monsterDecorator = monsterDecorator;
        _unitManagerController = unitManagerController;
    }
    public Multi_BossEnemy SpawnBoss(byte id, int bossLevel)
    {
        var boss = Managers.Multi.Instantiater.PhotonInstantiateInactive(BulildBossPath(), id).GetComponent<Multi_BossEnemy>();
        photonView.RPC(nameof(InjectBoss), RpcTarget.All, (byte)bossLevel, boss.GetComponent<PhotonView>().ViewID);
        boss.OnDeath += () => OnDead?.Invoke(boss);
        return boss;
    }

    [PunRPC]
    void InjectBoss(byte level, int viewId)
    {
        var monster = Managers.Multi.GetPhotonViewComponent<Multi_BossEnemy>(viewId);
        var bossData = Managers.Data.BossDataByLevel[level];
        Multi_EnemyManager.Instance.SetSpawnBoss(monster.UsingId, monster);
        Managers.Sound.PlayBgm(BgmType.Boss);
        monster.Inject(bossData, _unitManagerController, _monsterDecorator.CreateSlowController(monster), new SpeedManager(bossData.Speed));
    }
}
