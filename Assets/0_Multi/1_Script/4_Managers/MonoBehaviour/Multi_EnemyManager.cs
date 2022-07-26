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

            Multi_SpawnManagers.BossEnemy.OnSpawn += boss => _currentBoss.Set(boss, boss);
            Multi_SpawnManagers.BossEnemy.OnDead += boss => _currentBoss.Set(boss, null);

            Multi_SpawnManagers.TowerEnemy.OnSpawn += tower => _currentTower.Set(tower, tower);
            Multi_SpawnManagers.TowerEnemy.OnDead += tower => _currentTower.Set(tower, null);
        }
    }

    Dictionary<int, List<Transform>> currentNormalEnemysById = new Dictionary<int, List<Transform>>();

    public RPCAction<int> OnEnemyCountChanged = new RPCAction<int>();
    void Raise_EnemyCountChanged(int id) => OnEnemyCountChanged.RaiseEvent(id, currentNormalEnemysById[id].Count);

    RPCData<Multi_BossEnemy> _currentBoss = new RPCData<Multi_BossEnemy>();
    bool BossIsAlive(int id) => _currentBoss.Get(id) != null;

    RPCData<Multi_EnemyTower> _currentTower = new RPCData<Multi_EnemyTower>();
    public Multi_EnemyTower GetCurrnetTower(int id) => _currentTower.Get(id);

    [Header("테스트 인스팩터")]
    [SerializeField] List<Transform> test_0 = new List<Transform>();
    [SerializeField] List<Transform> test_1 = new List<Transform>();
    [SerializeField] Multi_BossEnemy testBoss = new Multi_BossEnemy();
    [SerializeField] Multi_EnemyTower testTower = new Multi_EnemyTower();
    void Update()
    {
#if UNITY_EDITOR
        if (PhotonNetwork.IsMasterClient && currentNormalEnemysById.ContainsKey(0))
        {
            testBoss = _currentBoss.Get(1);
            testTower = _currentTower.Get(1);
            test_0 = currentNormalEnemysById[0];
            test_1 = currentNormalEnemysById[1];
        }
#endif
    }

    
    public Transform GetProximateEnemy(Vector3 unitPos, float startDistance, int unitId)
    {
        if (_currentBoss.Get(unitId) != null) return _currentBoss.Get(unitId).transform;

        return GetProximateEnemy(unitPos, startDistance, currentNormalEnemysById[unitId]);
    }

    Transform GetProximateEnemy(Vector3 _unitPos, float _startDistance, List<Transform> _enemyList)
    {
        if (_enemyList == null || _enemyList.Count == 0) return null;

        Transform[] _enemys = _enemyList.ToArray();
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
            if (_enemys.Count > 0 && BossIsAlive(unitId) == false)
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
    #endregion
}
