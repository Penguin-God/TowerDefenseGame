using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using Photon.Pun;

public class Multi_EnemyManager : MonoBehaviour
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
        }

        Multi_SpawnManagers.NormalEnemy.OnSpawn += _AddEnemyAtList;
        Multi_SpawnManagers.NormalEnemy.OnDead += _RemoveEnemyAtList;

        Multi_SpawnManagers.NormalEnemy.OnSpawn += AddEnemyAtList;
        Multi_SpawnManagers.NormalEnemy.OnDead += RemoveEnemyAtList;

        // TODO : 나중에 boss랑 타워 작업하면 부활 예정
        //Multi_SpawnManagers.BossEnemy.OnSpawn += SetBoss;
        //Multi_SpawnManagers.BossEnemy.OnDead += SetBossDead;
        //Multi_SpawnManagers.BossEnemy.OnDead += GetBossReward;

        //Multi_SpawnManagers.TowerEnemy.OnSpawn += SetTower;
        //Multi_SpawnManagers.TowerEnemy.OnDead += SetTowerDead;
    }

    Dictionary<int, List<Transform>> currentNormalEnemysById = new Dictionary<int, List<Transform>>();
    
    public event Action<int> OnEnemyCountChanged;

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

    [Header("Normal Enemy")]
    [SerializeField] List<Transform> allNormalEnemys = new List<Transform>();
    public IReadOnlyList<Transform> AllNormalEnemys => allNormalEnemys;
    public int EnemyCount => allNormalEnemys.Count;
    public event Action<int> OnListChanged = null;

    //[Header("Boss Enemy")]
    //[SerializeField] Multi_BossEnemy currentBoss;
    //public Multi_BossEnemy CurrentBoss => currentBoss;
    //[SerializeField] int currentBossLevel;
    //public int CurrentBossLevel => currentBossLevel;
    //public bool IsBossAlive => currentBoss != null;

    //[SerializeField] int bossGoldReward;
    //public int BossGoldReward => bossGoldReward;
    //[SerializeField] int bossFoodReward;
    //public int BossFoodReward => bossFoodReward;

    //[Header("Enemy Tower")]
    //[SerializeField] Multi_EnemyTower currentEnemyTower;
    //public Multi_EnemyTower CurrentEnemyTower => currentEnemyTower;
    //[SerializeField] int currentEnemyTowerLevel;
    //public int CurrentEnemyTowerLevel => currentEnemyTowerLevel;

    public Transform GetProximateEnemy(Vector3 unitPos, float startDistance, int unitId)
        => GetProximateEnemy(unitPos, startDistance, currentNormalEnemysById[unitId]);

    public Transform GetProximateEnemy(Vector3 _unitPos, float _startDistance)
        => GetProximateEnemy(_unitPos, _startDistance, allNormalEnemys);
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

    public Transform[] GetProximateEnemys(Vector3 _unitPos, float _startDistance, int count, Transform currentTarget)
    {
        if (allNormalEnemys.Count == 0) return null;

        List<Transform> _enemys = new List<Transform>(allNormalEnemys);
        Transform[] targets = new Transform[count];

        for (int i = 0; i < count; i++)
        {
            if (_enemys.Count > 0)
            {
                targets[i] = GetProximateEnemy(_unitPos, _startDistance, _enemys);
                _enemys.Remove(targets[i]);
            }
            else targets[i] = currentTarget;
        }
        
        return targets;
    }

    public Multi_Enemy GetRandom_CurrentEnemy()
    {
        int index = Random.Range(0, allNormalEnemys.Count);
        Multi_Enemy enemy = allNormalEnemys[index].GetComponent<Multi_Enemy>();
        return enemy;
    }

    #region callback funtion
    void _AddEnemyAtList(Multi_NormalEnemy _enemy)
    {
        int count = 0;
        if (PhotonNetwork.IsMasterClient)
        {
            int id = _enemy.GetComponent<Poolable>().UsingId;
            currentNormalEnemysById[id].Add(_enemy.transform);
            count = currentNormalEnemysById[id].Count;
        }
        
        OnEnemyCountChanged?.Invoke(count);
    }
    void _RemoveEnemyAtList(Multi_Enemy _enemy)
    {
        int count = 0;
        if (PhotonNetwork.IsMasterClient)
        {
            int id = _enemy.GetComponent<Poolable>().UsingId;
            currentNormalEnemysById[id].Remove(_enemy.transform);
            count = currentNormalEnemysById[id].Count;
        }

        OnEnemyCountChanged?.Invoke(count);
    }

    void AddEnemyAtList(Multi_NormalEnemy _enemy)
    {
        allNormalEnemys.Add(_enemy.transform);
        OnListChanged?.Invoke(EnemyCount);
    }
    void RemoveEnemyAtList(Multi_Enemy _enemy)
    {
        allNormalEnemys.Remove(_enemy.transform);
        OnListChanged?.Invoke(EnemyCount);
    }

    //void SetBoss(Multi_BossEnemy _spawnBoss) => currentBoss = _spawnBoss;
    //void SetBossDead(Multi_BossEnemy _spawnBoss) => currentBoss = null;

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
