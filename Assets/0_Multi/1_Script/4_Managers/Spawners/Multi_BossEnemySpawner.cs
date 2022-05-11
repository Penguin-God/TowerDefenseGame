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
    public override void Init()
    {
        for (int i = 0; i < _enemys.Length; i++)
        {
            Multi_BossEnemy[] enemys = CreatePool_InGroup<Multi_BossEnemy>(_enemys[i], BuildPath(_rootPath, _enemys[i]), spawnCount);

            foreach (var enemy in enemys) SetEnemy(enemy);
        }

        spawnPos = Multi_Data.instance.EnemySpawnPos;
        //Multi_StageManager.Instance.OnUpdateStage += RespawnBoss;
    }

    void SetEnemy(Multi_BossEnemy enemy)
    {
        enemy.enemyType = EnemyType.Boss;

        enemy.OnDeath += () => OnDead(enemy);
        enemy.OnDeath += () => Multi_Managers.Pool.Push(enemy.GetComponent<Poolable>());
    }

    public bool bossRespawn;
    public int bossLevel;

    [HideInInspector]
    public List<Multi_BossEnemy> currentBossList;

    void RespawnBoss(int stage)
    {
        if (stage % 10 != 0) return;

        bossLevel++;
        bossRespawn = true;
        SetBossStatus(stage);

        Multi_GameManager.instance.ChangeBGM(Multi_GameManager.instance.bossbgmClip);
    }

    void SetBossStatus(int stage)
    {
        GameObject _bossObj = Instantiate(_enemys[Random.Range(0, _enemys.Length)]);
        Multi_BossEnemy _instantBoss = _bossObj.GetComponent<Multi_BossEnemy>();
        OnSpawn?.Invoke(_instantBoss);

        // TODO : 하드코딩한 부분 개선하기
        int stageWeigh = (stage / 10) * (stage / 10) * (stage / 10);
        int hp = 10000 * (stageWeigh * Mathf.CeilToInt(150 / 15f)); // boss hp 정함

        _instantBoss.transform.position = Multi_Data.instance.EnemySpawnPos;
        _instantBoss.gameObject.SetActive(true);
        //currentBossList.Add(instantBoss.GetComponentInChildren<Multi_BossEnemy>());

        _instantBoss.photonView.RPC("SetPos", RpcTarget.All, spawnPos);
        _instantBoss.photonView.RPC("Setup", RpcTarget.All, hp, 10); // TODO : speed 값 따로 변수 만들기
        SetBossDeadAction(_instantBoss);
    }

    void SetBossDeadAction(Multi_BossEnemy boss)
    {
        boss.OnDeath += () => OnDead(boss);
        boss.OnDeath += () => SetData(boss);
    }

    void SetData(Multi_BossEnemy boss)
    {
        currentBossList.Remove(boss);
        bossRespawn = false;
    }
}
