using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class Multi_TowerEnemySpawner : Multi_EnemySpawnerBase
{
    public event Action<Multi_EnemyTower> OnSpawn;
    public event Action<Multi_EnemyTower> OnDead;
    Vector3 spawnPos;

    protected override void Init()
    {
        spawnPos = Multi_Data.instance.EnemyTowerSpawnPos;
        Multi_GameManager.instance.OnStart += Spawn;
    }

    protected override void MasterInit()
    {
        CreatePool();
    }

    void CreatePool()
    {
        for (int i = 0; i < _enemys.Length; i++)
            CreatePool_InGroup<Multi_EnemyTower>(_enemys[i], BuildPath(_rootPath, _enemys[i]), spawnCount);
    }

    public override void SettingPoolObject(object obj)
    {
        Multi_EnemyTower enemy = obj as Multi_EnemyTower;
        Debug.Assert(enemy != null, "캐스팅 실패!!");
        SetEnemy(enemy);
    }

    void SetEnemy(Multi_EnemyTower enemy)
    {
        enemy.enemyType = EnemyType.Tower;

        if (PhotonNetwork.IsMasterClient == false) return;
        enemy.OnDeath += () => OnDead(enemy);
        enemy.OnDeath += () => Multi_Managers.Pool.Push(enemy.GetComponent<Poolable>());
        enemy.OnDeath += () => StartCoroutine(Co_AfterSpawn());
    }

    IEnumerator Co_AfterSpawn()
    {
        yield return new WaitForSeconds(5f);
        Spawn();
    }

    [SerializeField] int towerLevel = 0;
    void Spawn()
    {
        towerLevel++;
        Spawn_RPC(BuildPath(_rootPath, _enemys[towerLevel-1]), spawnPos);
    }

    [PunRPC]
    protected override GameObject BaseSpawn(string path, Vector3 spawnPos, int id)
    {
        Multi_EnemyTower enemy = base.BaseSpawn(path, spawnPos, id).GetComponent<Multi_EnemyTower>();
        enemy.Spawn(towerLevel);
        OnSpawn?.Invoke(enemy);
        return null;
    }
}
