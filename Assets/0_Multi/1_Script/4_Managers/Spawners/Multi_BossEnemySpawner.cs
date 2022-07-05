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
        CreatePool();

        spawnPos = Multi_Data.instance.EnemySpawnPos;
        Multi_StageManager.Instance.OnUpdateStage += RespawnBoss;
    }

    void CreatePool()
    {
        for (int i = 0; i < _enemys.Length; i++)
            CreatePool_InGroup<Multi_BossEnemy>(_enemys[i], BuildPath(_rootPath, _enemys[i]), spawnCount);
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

        enemy.OnDeath += () => OnDead(enemy);
        enemy.OnDeath += () => Multi_Managers.Pool.Push(enemy.GetComponent<Poolable>());
    }

    #endregion


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
        boss.OnDeath += () => SetVaryiable(boss);
    }

    void SetVaryiable(Multi_BossEnemy boss)
    {
        currentBossList.Remove(boss);
        bossRespawn = false;
    }
}
