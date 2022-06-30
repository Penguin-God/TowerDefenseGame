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

    string GetCurrentEnemyPath(int stage) => BuildPath(_rootPath, _enemys[_enemyDataByStage[stage].number]);
    int GetCurrentEnemyHp(int stage) => _enemyDataByStage[stage].hp;
    float GetCurrentEnemySpeed(int stage) => _enemyDataByStage[stage].speed;

    [SerializeField] int currentSpawnEnemyNum = 0; // 테스트용 변수
    [SerializeField] float _spawnDelayTime = 2f;
    [SerializeField] int _stageSpawnCount = 15;
    public float EnemySpawnTime => _spawnDelayTime * _stageSpawnCount;
    PhotonView pv;

    int minHp = 200;
    int enemyHpWeight;
    int plusEnemyHpWeight = 20;
    
    private float maxSpeed = 5f;
    private float minSpeed = 3f;
    public override void Init()
    {
        Multi_StageManager.Instance.OnUpdateStage += StageSpawn;
        pv = GetComponent<PhotonView>();
    }

    public override void MasterInit()
    {
        CreatePool();
        SetNormalEnemyData();

        void SetNormalEnemyData()
        {
            int maxNumber = _enemys.Length;
            int maxStage = 200; // 일단 200 스테이지까지만 설정

            for (int stage = 1; stage < maxStage; stage++)
            {
                enemyHpWeight += plusEnemyHpWeight;

                NormalEnemyData enemyData = new NormalEnemyData(Random.Range(0, maxNumber), SetRandomHp(stage, enemyHpWeight), SetRandomSeepd(stage));
                _enemyDataByStage.Add(stage, enemyData);
            }


            int SetRandomHp(int _stage, int _weight)
            {
                // satge에 따른 가중치 변수들
                int stageHpWeight = _stage * _stage * _weight;
                int hp = minHp + stageHpWeight;
                return hp;
            }

            float SetRandomSeepd(int _stage)
            {
                // satge에 따른 가중치 변수들
                float stageSpeedWeight = _stage / 6;

                float enemyMinSpeed = minSpeed + stageSpeedWeight;
                float enemyMaxSpeed = maxSpeed + stageSpeedWeight;
                float speed = Random.Range(enemyMinSpeed, enemyMaxSpeed);
                return speed;
            }
        }
    }

    void CreatePool()
    {
        for (int i = 0; i < _enemys.Length; i++)
            CreatePool_InGroup<Multi_NormalEnemy>(_enemys[i], BuildPath(_rootPath, _enemys[i]), spawnCount);
    }

    public void Spawn(int number, int stage)
    {
        Multi_NormalEnemy enemy =
            Multi_Managers.Resources.PhotonInsantiate(BuildPath(_rootPath, _enemys[number]), Multi_Data.instance.EnemySpawnPos, Multi_Data.instance.Id).GetComponent<Multi_NormalEnemy>();
        enemy.SetStatus_RPC(GetCurrentEnemyHp(stage), GetCurrentEnemySpeed(stage), false);
    }

    void Spawn(int stage) 
        => pv.RPC("NormalEnemySpawn", RpcTarget.MasterClient, stage, Multi_Data.instance.EnemySpawnPos, Multi_Data.instance.Id);

    [PunRPC]
    void NormalEnemySpawn(int stage, Vector3 spawnPos, int id)
    {
        Multi_NormalEnemy enemy = 
                    Multi_Managers.Resources.PhotonInsantiate(GetCurrentEnemyPath(stage), spawnPos, id).GetComponent<Multi_NormalEnemy>();
        enemy.SetStatus_RPC(GetCurrentEnemyHp(stage), GetCurrentEnemySpeed(stage), false);
    }

    public override void SettingPoolObject(object obj)
    {
        Multi_NormalEnemy enemy = obj as Multi_NormalEnemy;
        Debug.Assert(enemy != null, "캐스팅 실패!!");
        SetEnemy(enemy);
    }

    void SetEnemy(Multi_NormalEnemy enemy)
    {
        enemy.enemyType = EnemyType.Normal;
        enemy.OnSpawn += OnSpawn;
        enemy.OnDeath += () => OnDead(enemy);

        if (PhotonNetwork.IsMasterClient == false) return;
        enemy.OnDeath += () => Multi_Managers.Pool.Push(enemy.GetComponent<Poolable>());
    }

    // 콜백용 코드
    #region callbacks
    void StageSpawn(int stage)
    {
        if (stage % 10 == 0) return;
        StartCoroutine(Co_StageSpawn(stage));
    }

    IEnumerator Co_StageSpawn(int stage)
    {
        for (int i = 0; i < _stageSpawnCount; i++)
        {
            Spawn(stage);
            yield return new WaitForSeconds(_spawnDelayTime);
        }
    }
    #endregion
}
