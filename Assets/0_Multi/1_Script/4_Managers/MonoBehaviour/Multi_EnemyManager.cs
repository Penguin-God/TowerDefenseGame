using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class Multi_EnemyManager : MonoBehaviourPun
{
    private static Multi_EnemyManager instance;
    public static Multi_EnemyManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Multi_EnemyManager>();
                if (instance == null)
                    instance = new GameObject("Multi_EnemyManager").AddComponent<Multi_EnemyManager>();
            }

            return instance;
        }
    }

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            currentNormalEnemysById.Add(0, new List<Transform>());
            currentNormalEnemysById.Add(1, new List<Transform>());

            Multi_SpawnManagers.NormalEnemy.OnSpawn += AddEnemyAtList;
            Multi_SpawnManagers.NormalEnemy.OnDead += RemoveEnemyAtList;

            Multi_SpawnManagers.BossEnemy.OnSpawn += boss => currentBoss = boss;
            Multi_SpawnManagers.BossEnemy.OnSpawn += boss => currentBossLevel = boss.Level;
            Multi_SpawnManagers.BossEnemy.OnDead += boss => currentBoss = null;

            Multi_SpawnManagers.TowerEnemy.OnSpawn += tower => currentEnemyTower = tower;
            Multi_SpawnManagers.TowerEnemy.OnSpawn += tower => currentEnemyTowerLevel = tower.Level;
            Multi_SpawnManagers.TowerEnemy.OnDead += tower => currentEnemyTower = null;
        }

        // TODO : 나중에 boss랑 타워 작업하면 부활 예정

        //Multi_SpawnManagers.TowerEnemy.OnSpawn += SetTower;
        //Multi_SpawnManagers.TowerEnemy.OnDead += SetTowerDead;
    }

    Dictionary<int, List<Transform>> currentNormalEnemysById = new Dictionary<int, List<Transform>>();

    public RPCAction<int> OnEnemyCountChanged = new RPCAction<int>();
    void Raise_EnemyCountChanged(int id) => OnEnemyCountChanged.RaiseEvent(id, currentNormalEnemysById[id].Count);

    [SerializeField] List<Transform> test_0 = new List<Transform>();
    [SerializeField] List<Transform> test_1 = new List<Transform>();
    void Update()
    {
#if UNITY_EDITOR
        if (PhotonNetwork.IsMasterClient && currentNormalEnemysById.ContainsKey(0))
        {
            test_0 = currentNormalEnemysById[0];
            test_1 = currentNormalEnemysById[1];
        }
#endif
    }

    // TODO : 타워항 보스 구현하면 부활함
    [Header("Boss Enemy")]
    [SerializeField] Multi_BossEnemy currentBoss; // TODO : Player 스크립트 만들고 거기로 옮기기
    public Multi_BossEnemy CurrentBoss => currentBoss;
    [SerializeField] int currentBossLevel = 0;
    public int CurrentBossLevel => currentBossLevel;
    public bool IsBossAlive => currentBoss != null;

    [Header("Enemy Tower")]
    [SerializeField] Multi_EnemyTower currentEnemyTower; // TODO : Player 스크립트 만들고 거기로 옮기기
    public Multi_EnemyTower CurrentEnemyTower => currentEnemyTower;
    [SerializeField] int currentEnemyTowerLevel;
    public int CurrentEnemyTowerLevel => currentEnemyTowerLevel;

    public Transform GetProximateEnemy(Vector3 unitPos, float startDistance, int unitId)
        => GetProximateEnemy(unitPos, startDistance, currentNormalEnemysById[unitId]);
    public Transform GetProximateEnemy(Vector3 _unitPos, float _startDistance, List<Transform> _enemyList)
    {
        Transform[] _enemys = _enemyList.ToArray();
        if (_enemys == null || _enemys.Length == 0) return null;
        float shortDistance = _startDistance;
        Transform _returnEnemy = null;
        foreach (Transform _enemy in _enemys)
        {
            if (_enemy != null && !_enemy.GetComponent<Multi_Enemy>().isDead)
            {
                float distanceToEnemy = Vector3.Distance(_unitPos, _enemy.position);
                if (distanceToEnemy < shortDistance)
                {
                    shortDistance = distanceToEnemy;
                    _returnEnemy = _enemy;
                }
            }
        }

        return _returnEnemy;
    }

    public Transform[] GetProximateEnemys(Vector3 _unitPos, float _startDistance, int count, Transform currentTarget, int unitId)
    {
        if (currentNormalEnemysById[unitId].Count == 0) return null;

        List<Transform> _enemys = new List<Transform>(currentNormalEnemysById[unitId]);
        Transform[] result = new Transform[count];

        for (int i = 0; i < count; i++)
        {
            if (_enemys.Count > 0)
            {
                result[i] = GetProximateEnemy(_unitPos, _startDistance, _enemys);
                _enemys.Remove(result[i]);
            }
            else result[i] = currentTarget;
        }
        
        return result;
    }

    // TODO : 빨간 마법사 스킬 강화 구현하고 죽이기
    //public Multi_Enemy GetRandom_CurrentEnemy()
    //{
    //    int index = Random.Range(0, allNormalEnemys.Count);
    //    Multi_Enemy enemy = allNormalEnemys[index].GetComponent<Multi_Enemy>();
    //    return enemy;
    //}

    #region callback funtion
    void AddEnemyAtList(Multi_NormalEnemy _enemy)
    {
        if (PhotonNetwork.IsMasterClient == false) return;

        int id = _enemy.GetComponent<RPCable>().UsingId;
        currentNormalEnemysById[id].Add(_enemy.transform);
        Raise_EnemyCountChanged(id);
    }
    void RemoveEnemyAtList(Multi_Enemy _enemy)
    {
        if (PhotonNetwork.IsMasterClient == false) return;

        int id = _enemy.GetComponent<RPCable>().UsingId;
        currentNormalEnemysById[id].Remove(_enemy.transform);
        Raise_EnemyCountChanged(id);
    }

    //// TODO : 리펙토링 할 수 있나 생각해보기
    //void GetBossReward(Multi_BossEnemy _spawnBoss)
    //{
    //    int _level = _spawnBoss.Level;
    //    Multi_GameManager.instance.AddGold(bossGoldReward * _level);
    //    Multi_GameManager.instance.AddFood(BossFoodReward * _level);
    //}

    //void SetTower(Multi_EnemyTower _spawnTower) => currentEnemyTower = _spawnTower;
    //void SetTowerDead(Multi_EnemyTower _spawnTower) => currentEnemyTower = null;

    #endregion
}
