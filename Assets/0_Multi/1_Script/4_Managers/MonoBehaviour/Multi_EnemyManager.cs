using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
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

    MasterManager _master = new MasterManager();
    EnemyCountManager _count = new EnemyCountManager();
    void Awake()
    {
        Multi_SpawnManagers.NormalEnemy.OnSpawn += _master.AddEnemy;
        Multi_SpawnManagers.NormalEnemy.OnDead += _master.RemoveEnemy;
        
        _count.Init(_master);
        _count.OnEnemyCountChanged += RaiseOnEnemyCountChanged;
        _count.OnOthreEnemyCountChanged += RaiseOnOtherEnemyCountChanged;
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

    public event Action<int> OnEnemyCountChang = null;
    void RaiseOnEnemyCountChanged(int count) => OnEnemyCountChang?.Invoke(count);
    public event Action<int> OnOtherEnemyCountChanged = null;
    void RaiseOnOtherEnemyCountChanged(int count) => OnOtherEnemyCountChanged?.Invoke(count);

    // TODO : 죽이기
    Dictionary<int, List<Transform>> currentNormalEnemysById = new Dictionary<int, List<Transform>>();

    public int MyEnemyCount => _count.CurrentEnemyCount;
    public int EnemyPlayerEnemyCount => _count.OtherEnemyCount;

    RPCData<Multi_BossEnemy> _currentBoss = new RPCData<Multi_BossEnemy>();
    bool BossIsAlive(int id) => _currentBoss.Get(id) != null;

    RPCData<Multi_EnemyTower> _currentTower = new RPCData<Multi_EnemyTower>();
    public Multi_EnemyTower GetCurrnetTower(int id) => _currentTower.Get(id);

    #region editor test
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
    #endregion

    #region Find Enemy
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
            if (_enemy != null && !_enemy.GetComponent<Multi_Enemy>().IsDead)
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
    #endregion

    #region callback funtion
    void AddEnemyAtList(Multi_NormalEnemy _enemy)
    {
        if (PhotonNetwork.IsMasterClient == false) return;

        int id = _enemy.GetComponent<RPCable>().UsingId;
        currentNormalEnemysById[id].Add(_enemy.transform);
    }
    void RemoveEnemyAtList(Multi_NormalEnemy _enemy)
    {
        if (PhotonNetwork.IsMasterClient == false) return;

        int id = _enemy.GetComponent<RPCable>().UsingId;
        currentNormalEnemysById[id].Remove(_enemy.transform);
    }
    #endregion

    // TODO : Boss랑 타워도 관리하기
    class MasterManager
    {
        RPCData<List<Multi_NormalEnemy>> _enemyCountData = new RPCData<List<Multi_NormalEnemy>>();
        public IReadOnlyList<Multi_NormalEnemy> GetEnemys(int id) => _enemyCountData.Get(id);
        public RPCAction<int, int> OnEnemyCountChanged = new RPCAction<int, int>();

        public void AddEnemy(Multi_NormalEnemy _enemy)
        {
            if (PhotonNetwork.IsMasterClient == false) return;

            int id = _enemy.GetComponent<RPCable>().UsingId;
            _enemyCountData.Get(id).Add(_enemy);
            OnEnemyCountChanged.RaiseAll(id, _enemyCountData.Get(id).Count);
        }

        public void RemoveEnemy(Multi_NormalEnemy _enemy)
        {
            if (PhotonNetwork.IsMasterClient == false) return;

            int id = _enemy.GetComponent<RPCable>().UsingId;
            _enemyCountData.Get(id).Remove(_enemy);
            OnEnemyCountChanged.RaiseAll(id, _enemyCountData.Get(id).Count);
        }
    }

    class EnemyCountManager
    {
        int _currentEnemyCount;
        public int CurrentEnemyCount => _currentEnemyCount;
        public event Action<int> OnEnemyCountChanged = null;

        int _otherPlayerEnemyCount;
        public int OtherEnemyCount => _otherPlayerEnemyCount;
        public event Action<int> OnOthreEnemyCountChanged = null;
        public void Init(MasterManager master)
        {
            master.OnEnemyCountChanged += UpdateCount;
        }

        void UpdateCount(int id, int count)
        {
            if (Multi_Data.instance.Id == id)
            {
                _currentEnemyCount = count;
                OnEnemyCountChanged?.Invoke(_currentEnemyCount);
            }
            else
            {
                _otherPlayerEnemyCount = count;
                OnOthreEnemyCountChanged?.Invoke(_otherPlayerEnemyCount);
            }
        }
    }

    class EnemyFinder
    {
        MasterManager _master;
        public void Init(MasterManager master)
        {
            _master = master;
        }

        public Multi_Enemy GetProximateEnemy(Vector3 unitPos, float startDistance, int id)
        {
            return GetProximateEnemy(unitPos, startDistance, _master.GetEnemys(id));
        }

        Multi_Enemy GetProximateEnemy(Vector3 _unitPos, float _startDistance, IEnumerable<Multi_Enemy> _enemyList)
        {
            if (_enemyList == null || _enemyList.Count() == 0) return null;

            float shortDistance = _startDistance;
            Multi_Enemy _returnEnemy = null;
            foreach (Multi_Enemy _enemy in _enemyList)
            {
                if (_enemy != null && !_enemy.GetComponent<Multi_Enemy>().IsDead)
                {
                    float distanceToEnemy = Vector3.Distance(_unitPos, _enemy.transform.position);
                    if (distanceToEnemy < shortDistance)
                    {
                        shortDistance = distanceToEnemy;
                        _returnEnemy = _enemy;
                    }
                }
            }

            return _returnEnemy;
        }

        public Multi_Enemy[] GetProximateEnemys(Vector3 _unitPos, float _startDistance, int count, Multi_Enemy currentTarget, int id, bool bossIsAlive)
        {
            if (_master.GetEnemys(id).Count == 0) return null;

            List<Multi_Enemy> _enemys = new List<Multi_Enemy>(_master.GetEnemys(id));
            Multi_Enemy[] result = new Multi_Enemy[count];

            for (int i = 0; i < count; i++)
            {
                if (_enemys.Count > 0 && bossIsAlive == false)
                {
                    result[i] = GetProximateEnemy(_unitPos, _startDistance, _enemys);
                    _enemys.Remove(result[i]);
                }
                else result[i] = currentTarget;
            }

            return result;
        }
    }
}
