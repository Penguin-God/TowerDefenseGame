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
    Vector3 spawnPos;

    // Init용 코드
    // TODO : 클래스로 옮기기
    #region Init

    protected override void Init()
    {
        spawnPos = Multi_Data.instance.EnemySpawnPos;
        Multi_StageManager.Instance.OnUpdateStage += RespawnBoss;
    }

    protected override void MasterInit()
    {
        CreatePool();
    }

    void CreatePool()
    {
        for (int i = 0; i < _enemys.Length; i++)
            CreatePool_InGroup<Multi_BossEnemy>(_enemys[i], BuildPath(_rootPath, _enemys[i]), spawnCount);
    }

    void Spawn()
    {
        bossLevel++;
        Spawn_RPC(BuildPath(_rootPath, _enemys[0]), spawnPos);
    }

    [SerializeField] int bossLevel;
    [PunRPC]
    protected override GameObject BaseSpawn(string path, Vector3 spawnPos, int id)
    {
        Multi_BossEnemy enemy = base.BaseSpawn(path, spawnPos, id).GetComponent<Multi_BossEnemy>();
        enemy.Spawn(10000000, 5, bossLevel);
        OnSpawn?.Invoke(enemy);
        return null;
    }

    public override void SettingPoolObject(object obj)
    {
        Multi_BossEnemy enemy = obj as Multi_BossEnemy;
        Debug.Assert(enemy != null, "캐스팅 실패!!");
        SetEnemy(enemy);
    }

    void SetEnemy(Multi_BossEnemy enemy)
    {
        enemy.enemyType = EnemyType.Boss;

        if (PhotonNetwork.IsMasterClient == false) return;
        enemy.OnDeath += () => OnDead(enemy);
        enemy.OnDeath += () => Multi_Managers.Pool.Push(enemy.GetComponent<Poolable>());
    }

    #endregion


    void RespawnBoss(int stage)
    {
        if (stage % 10 != 0) return;

        Spawn();
    }
}
