using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Photon.Pun;
using Random = UnityEngine.Random;

public class Multi_BossEnemySpawner : Multi_EnemySpawnerBase
{
    public event Action<Multi_BossEnemy> OnSpawn;
    public event Action<Multi_BossEnemy> OnDead;

    public RPCAction rpcOnSpawn = new RPCAction();
    public RPCAction rpcOnDead = new RPCAction();


    // Init용 코드
    #region Init
    protected override void Init()
    {
        Multi_StageManager.Instance.OnUpdateStage += RespawnBoss;
    }

    protected override void MasterInit()
    {
        CreatePool();
    }

    void CreatePool()
    {
        for (int i = 0; i < _enemys.Length; i++)
            CreatePoolGroup(_enemys[i], BuildPath(_rootPath, _enemys[i]), spawnCount);
    }

    protected override void SetPoolObj(GameObject go)
    {
        var enemy = go.GetComponent<Multi_BossEnemy>();
        enemy.enemyType = EnemyType.Boss;
        enemy.OnDeath += () => OnDead(enemy);
        enemy.OnDeath += () => rpcOnDead.RaiseEvent(enemy.UsingId);
        enemy.OnDeath += () => Multi_Managers.Pool.Push(enemy.GetComponent<Poolable>());
    }

    #endregion

    void Spawn()
    {
        bossLevel++;
        Spawn_RPC(BuildPath(_rootPath, _enemys[Random.Range(0, _enemys.Length)]), Vector3.zero);
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

    void RespawnBoss(int stage)
    {
        if (stage % 10 != 0) return;

        Spawn();
    }
}
