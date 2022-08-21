using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using System.Linq;

public class Multi_NormalEnemySpawner : Multi_EnemySpawnerBase
{
    public event Action<Multi_NormalEnemy> OnSpawn;
    public event Action<Multi_NormalEnemy> OnDead;

    string GetCurrentEnemyPath(int enemyNum) => BuildPath(_rootPath, _enemys[enemyNum]);

    [SerializeField] int otherEnemySpawnNumber = 0;
    public void SetOtherEnemyNumber(int num) => otherEnemySpawnNumber = num;

    [SerializeField] float _spawnDelayTime = 2f;
    [SerializeField] int _stageSpawnCount = 15;
    public float EnemySpawnTime => _spawnDelayTime * _stageSpawnCount;

    protected override void Init()
    {
        Multi_StageManager.Instance.OnUpdateStage += StageSpawn;
        isTest = false;
    }

    protected override void MasterInit()
    {
        CreatePool();
    }

    void CreatePool()
    {
        for (int i = 0; i < _enemys.Length; i++)
        {
            List<Multi_NormalEnemy> enemys = CreatePool_InGroup<Multi_NormalEnemy>(_enemys[i], BuildPath(_rootPath, _enemys[i]), spawnCount).ToList();
            enemys.ForEach(x => SetEnemy(x));
        }
    }

    [SerializeField] bool isTest;
    void Spawn(int enemyNum)
    {
        int targetId = (Multi_Data.instance.Id == 0) ? 1 : 0;
        if (isTest) targetId = 0;
        Spawn_RPC(GetCurrentEnemyPath(enemyNum), spawnPositions[targetId], targetId);
    }

    [PunRPC]
    protected override GameObject BaseSpawn(string path, Vector3 spawnPos, Quaternion rotation, int id)
    {
        Multi_NormalEnemy enemy = base.BaseSpawn(path, spawnPos, rotation, id).GetComponent<Multi_NormalEnemy>();
        NormalEnemyData data = Multi_Managers.Data.NormalEnemyDataByStage[Multi_StageManager.Instance.CurrentStage];
        enemy.SetStatus_RPC(data.Hp, data.Speed, false);
        OnSpawn?.Invoke(enemy);
        return enemy.gameObject;
    }

    void EenmySpawnToOtherWorld(Multi_NormalEnemy enemy)
    {
        if (enemy.IsResurrection) return;

        int id = enemy.rpcable.UsingId == 0 ? 1 : 0;
        var _newEnemy = BaseSpawn(GetCurrentEnemyPath(enemy.GetEnemyNumber), spawnPositions[id], Quaternion.identity, id).GetComponent<Multi_NormalEnemy>();
        _newEnemy.SetStatus_RPC(enemy.maxHp, enemy.maxSpeed, false);
        _newEnemy.Resurrection();
    }

    void SetEnemy(Multi_NormalEnemy enemy)
    {
        enemy.enemyType = EnemyType.Normal;

        if (PhotonNetwork.IsMasterClient == false) return;
        enemy.OnDeath += () => OnDead(enemy);
        enemy.OnDeath += () =>
        {
            EenmySpawnToOtherWorld(enemy);
            Multi_Managers.Pool.Push(enemy.GetComponent<Poolable>());
        };
    }

    // 콜백용 코드
    #region callbacks
    void StageSpawn(int stage)
    {
        if (stage % 10 == 0) return;
        StartCoroutine(Co_StageSpawn());
    }

    IEnumerator Co_StageSpawn()
    {
        int enemyNum = otherEnemySpawnNumber;
        for (int i = 0; i < _stageSpawnCount; i++)
        {
            Spawn(enemyNum);
            yield return new WaitForSeconds(_spawnDelayTime);
        }
    }
    #endregion

    // TODO : #if 조건문으로 빼기
    public void Spawn(int enemyNum, int id) => Spawn_RPC(GetCurrentEnemyPath(enemyNum), spawnPositions[id], id);
}
