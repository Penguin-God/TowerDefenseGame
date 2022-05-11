using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using Random = UnityEngine.Random;

[Serializable]
public struct NormalEnemyData
{
    public int number;
    public int hp;
    public float speed;

    public NormalEnemyData(int _number, int _hp, float _speed)
    {
        number = _number;
        hp = _hp;
        speed = _speed;
    }
}

public class Multi_NormalEnemySpawner : Multi_EnemySpawnerBase
{
    public event Action<Multi_NormalEnemy> OnSpawn;
    public event Action<Multi_NormalEnemy> OnDead;

    Dictionary<int, NormalEnemyData> _enemyDataByStage = new Dictionary<int, NormalEnemyData>();

    [SerializeField] float _spawnDelayTime = 2f;
    [SerializeField] int _stageSpawnCount = 15;
    public float EnemySpawnTime => _spawnDelayTime * _stageSpawnCount;
    Vector3 _spawnPos;

    public override void Init()
    {
        for (int i = 0; i < _enemys.Length; i++)
        {
            Multi_NormalEnemy[] enemys = CreatePool_InGroup<Multi_NormalEnemy>(_enemys[i], BuildPath(_rootPath, _enemys[i]), spawnCount);

            foreach (var enemy in enemys) SetEnemy(enemy);
        }

        _spawnPos = Multi_Data.instance.EnemySpawnPos;

        SetNormalEnemyData();
        Multi_StageManager.Instance.OnUpdateStage += StageSpawn;
    }


    public void Spawn()
    {
        NormalEnemyData data = _enemyDataByStage[Multi_StageManager.Instance.CurrentStage];
        Spawn(data.number, data.hp, data.speed);
    }
    public void Spawn(int number)
    {
        NormalEnemyData data = _enemyDataByStage[Multi_StageManager.Instance.CurrentStage];
        Spawn(number, data.hp, data.speed);
    }
    public void Spawn(int number, int hp, float speed) => Spawn(BuildPath(_rootPath, _enemys[number]), hp, speed);
    public void Spawn(string path, int hp, float speed)
    {
        Multi_NormalEnemy enemy = Multi_Managers.Resources.PhotonInsantiate(path).GetComponent<Multi_NormalEnemy>();
        RPC_Utility.Instance.RPC_Position(enemy.PV.ViewID, _spawnPos);
        RPC_Utility.Instance.RPC_Active(enemy.PV.ViewID, true);
        enemy.SetStatus(RpcTarget.All, hp, speed, false);
        OnSpawn?.Invoke(enemy);
    }

    // init 용 코드
    #region Init Funtions
    void SetEnemy(Multi_NormalEnemy enemy)
    {
        enemy.enemyType = EnemyType.Normal;

        enemy.OnDeath += () => OnDead(enemy);
        enemy.OnDeath += () => Multi_Managers.Pool.Push(enemy.GetComponent<Poolable>());
    }

    int minHp = 200;
    int enemyHpWeight;
    int plusEnemyHpWeight = 20;
    void SetNormalEnemyData()
    {
        int maxNumber = _enemys.Length;

        for (int stage = 1; stage < 200; stage++)
        {
            enemyHpWeight += plusEnemyHpWeight;

            NormalEnemyData enemyData = new NormalEnemyData
            {
                number = Random.Range(0, maxNumber),
                hp = SetRandomHp(stage, enemyHpWeight),
                speed = SetRandomSeepd(stage),
            };

            _enemyDataByStage.Add(stage, enemyData);
        }
    }

    int SetRandomHp(int _stage, int _weight)
    {
        // satge에 따른 가중치 변수들
        int stageHpWeight = _stage * _stage * _weight;

        int hp = minHp + stageHpWeight;
        return hp;
    }

    private float maxSpeed = 5f;
    private float minSpeed = 3f;
    float SetRandomSeepd(int _stage)
    {
        // satge에 따른 가중치 변수들
        float stageSpeedWeight = _stage / 6;

        float enemyMinSpeed = minSpeed + stageSpeedWeight;
        float enemyMaxSpeed = maxSpeed + stageSpeedWeight;
        float speed = Random.Range(enemyMinSpeed, enemyMaxSpeed);
        return speed;
    }
    #endregion


    // 콜백용 코드
    #region callbacks
    void StageSpawn(int stage)
    {
        if (stage % 10 == 0) return;
        StartCoroutine(Co_StageSpawn(stage));
    }

    IEnumerator Co_StageSpawn(int stage)
    {
        NormalEnemyData data = _enemyDataByStage[stage];
        for (int i = 0; i < _stageSpawnCount; i++)
        {
            Spawn(data.number, data.hp, data.speed);
            yield return new WaitForSeconds(_spawnDelayTime);
        }
    }
    #endregion

}
