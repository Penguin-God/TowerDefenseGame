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

    string GetCurrentEnemyPath() => BuildPath(_rootPath, _enemys[currentSpawnEnemyNum]);

    [SerializeField] int currentSpawnEnemyNum = 0; // 테스트용 변수
    [SerializeField] float _spawnDelayTime = 2f;
    [SerializeField] int _stageSpawnCount = 15;
    public float EnemySpawnTime => _spawnDelayTime * _stageSpawnCount;

    protected override void Init()
    {
        Multi_StageManager.Instance.OnUpdateStage += StageSpawn;
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

    // path는 BaseSpawn 안에서 만들어서 씀
    void Spawn() => Spawn_RPC("", Multi_Data.instance.EnemySpawnPos);

    [PunRPC]
    protected override GameObject BaseSpawn(string path, Vector3 spawnPos, Quaternion rotation, int id)
    {
        Multi_NormalEnemy enemy = base.BaseSpawn(GetCurrentEnemyPath(), spawnPos, rotation, id).GetComponent<Multi_NormalEnemy>();
        NormalEnemyData data = Multi_Managers.Data.NormalEnemyDataByStage[Multi_StageManager.Instance.CurrentStage];
        enemy.SetStatus_RPC(data.Hp, data.Speed, false);
        OnSpawn?.Invoke(enemy);
        return null;
    }

    void SetEnemy(Multi_NormalEnemy enemy)
    {
        enemy.enemyType = EnemyType.Normal;

        if (PhotonNetwork.IsMasterClient == false) return;
        enemy.OnDeath += () => OnDead(enemy);
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
        for (int i = 0; i < _stageSpawnCount; i++)
        {
            //Spawn(stage);
            Spawn();
            yield return new WaitForSeconds(_spawnDelayTime);
        }
    }
    #endregion

    // TODO : #if 조건문으로 빼기
    public void Spawn(int number) => Spawn_RPC(BuildPath(_rootPath, _enemys[number]), Multi_Data.instance.EnemySpawnPos);
}
