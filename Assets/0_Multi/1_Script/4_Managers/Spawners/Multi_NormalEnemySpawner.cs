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
    public float EnemySpawnTime => _spawnDelayTime * _stageSpawnCount + 10;

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
            CreatePoolGroup(_enemys[i], BuildPath(_rootPath, _enemys[i]), spawnCount);
    }

    [SerializeField] bool isTest;
    void Spawn(int enemyNum)
    {
        int targetId = (Multi_Data.instance.Id == 0) ? 1 : 0;
        if (isTest) targetId = 0;
        SpawnEnemy_RPC(GetCurrentEnemyPath(enemyNum), Multi_StageManager.Instance.CurrentStage, targetId);
    }


    void EenmySpawnToOtherWorld(Multi_NormalEnemy enemy, int enemyNum)
    {
        if (enemy.resurrection.IsResurrection) return;

        int id = enemy.UsingId == 0 ? 1 : 0;
        if(enemy.UsingId == Multi_Data.instance.Id)
            EenmySpawnToOtherWorld(enemyNum, id, enemy.resurrection.SpawnStage);
        else
            pv.RPC("EenmySpawnToOtherWorld", RpcTarget.Others, otherEnemySpawnNumber, id, enemy.resurrection.SpawnStage);
    }

    [PunRPC]
    void EenmySpawnToOtherWorld(int enemyNum, int id, int stage)
    {
        if (PhotonNetwork.IsMasterClient)
            SpawnEnemy(GetCurrentEnemyPath(enemyNum), stage, id).Resurrection_RPC();
        else
            pv.RPC("EenmySpawnToOtherWorld", RpcTarget.MasterClient, otherEnemySpawnNumber, id, stage);
    }

    void SpawnEnemy_RPC(string path, int stage, int id) => pv.RPC("SpawnEnemy", RpcTarget.MasterClient, path, stage, id);

    [PunRPC]
    Multi_NormalEnemy SpawnEnemy(string path, int stage, int id)
    {
        var enemy = base.BaseSpawn(path, spawnPositions[id], Quaternion.identity, id).GetComponent<Multi_NormalEnemy>();
        NormalEnemyData data = Multi_Managers.Data.NormalEnemyDataByStage[stage];
        enemy.SetStatus_RPC(data.Hp, data.Speed, false);
        enemy.resurrection.SetSpawnStage(stage);
        OnSpawn?.Invoke(enemy);
        return enemy;
    }

    protected override void SetPoolObj(GameObject go)
    {
        var enemy = go.GetComponent<Multi_NormalEnemy>();
        enemy.enemyType = EnemyType.Normal;

        if (PhotonNetwork.IsMasterClient == false) return;
        enemy.OnDeath += () => OnDead(enemy);
        enemy.OnDeath += () => EenmySpawnToOtherWorld(enemy, otherEnemySpawnNumber);
        enemy.OnDeath += () => Multi_Managers.Pool.Push(enemy.GetComponent<Poolable>());
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
