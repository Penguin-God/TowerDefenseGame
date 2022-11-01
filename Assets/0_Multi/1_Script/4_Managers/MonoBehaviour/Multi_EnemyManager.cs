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
    EnemyCountManager _counter = new EnemyCountManager();
    EnemyFinder _finder = new EnemyFinder();
    
    void Awake()
    {
        Multi_SpawnManagers.NormalEnemy.OnSpawn += _master.AddEnemy;
        Multi_SpawnManagers.NormalEnemy.OnDead += _master.RemoveEnemy;
        
        _counter.Init(_master);
        _counter.OnEnemyCountChanged += RaiseOnEnemyCountChanged;
        _counter.OnOthreEnemyCountChanged += RaiseOnOtherEnemyCountChanged;
    }

    public event Action<int> OnEnemyCountChanged = null;
    void RaiseOnEnemyCountChanged(int count) => OnEnemyCountChanged?.Invoke(count);

    public event Action<int> OnOtherEnemyCountChanged = null;
    void RaiseOnOtherEnemyCountChanged(int count) => OnOtherEnemyCountChanged?.Invoke(count);

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Multi_SpawnManagers.BossEnemy.OnSpawn += boss => _currentBoss.Set(boss, boss);
            Multi_SpawnManagers.BossEnemy.OnDead += boss => _currentBoss.Set(boss, null);

            Multi_SpawnManagers.TowerEnemy.OnSpawn += tower => _currentTower.Set(tower, tower);
            Multi_SpawnManagers.TowerEnemy.OnDead += tower => _currentTower.Set(tower, null);
        }
    }

    public int MyEnemyCount => _counter.CurrentEnemyCount;
    public int EnemyPlayerEnemyCount => _counter.OtherEnemyCount;

    RPCData<Multi_BossEnemy> _currentBoss = new RPCData<Multi_BossEnemy>();
    public bool TryGetCurrentBoss(int id, out Multi_BossEnemy boss)
    {
        boss = _currentBoss.Get(id);
        return boss != null;
    }

    RPCData<Multi_EnemyTower> _currentTower = new RPCData<Multi_EnemyTower>();
    public Multi_EnemyTower GetCurrnetTower(int id) => _currentTower.Get(id);

    public Multi_Enemy GetProximateEnemy(Vector3 unitPos, int unitId) => _finder.GetProximateEnemy(unitPos, _master.GetEnemys(unitId));

    public Multi_Enemy[] __GetProximateEnemys(Vector3 _unitPos, int maxCount, int unitId)
    {
        if (maxCount >= _master.GetEnemys(unitId).Count) return _master.GetEnemys(unitId).ToArray();
        return _finder.GetProximateEnemys(_unitPos, maxCount, _master.GetEnemys(unitId));
    }

    public Transform[] GetProximateEnemys(Vector3 _unitPos, int maxCount, int unitId) 
        => __GetProximateEnemys(_unitPos, maxCount, unitId).Select(x => x?.transform).ToArray();

    #region editor test
    [Header("테스트 인스팩터")]
    [SerializeField] List<Transform> test_0 = new List<Transform>();
    [SerializeField] List<Transform> test_1 = new List<Transform>();
    [SerializeField] Multi_BossEnemy testBoss = new Multi_BossEnemy();
    [SerializeField] Multi_EnemyTower testTower = new Multi_EnemyTower();
    void Update()
    {
#if UNITY_EDITOR
        if (PhotonNetwork.IsMasterClient)
        {
            testBoss = _currentBoss.Get(1);
            testTower = _currentTower.Get(1);
            test_0 = _master.GetEnemys(0).Select(x => x.transform).ToList();
            test_1 = _master.GetEnemys(1).Select(x => x.transform).ToList();
        }
#endif
    }
    #endregion




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
        public int CurrentEnemyCount { get; private set; }
        public event Action<int> OnEnemyCountChanged = null;

        public int OtherEnemyCount { get; private set; }
        public event Action<int> OnOthreEnemyCountChanged = null;
        public void Init(MasterManager master)
        {
            master.OnEnemyCountChanged += UpdateCount;
        }

        void UpdateCount(int id, int count) // id에 따라 어느쪽 count인지 구분
        {
            if (Multi_Data.instance.Id == id)
            {
                CurrentEnemyCount = count;
                OnEnemyCountChanged?.Invoke(CurrentEnemyCount);
            }
            else
            {
                OtherEnemyCount = count;
                OnOthreEnemyCountChanged?.Invoke(OtherEnemyCount);
            }
        }
    }


    class EnemyFinder
    {
        public Multi_Enemy GetProximateEnemy(Vector3 _unitPos, IEnumerable<Multi_Enemy> _enemyList)
        {
            if (_enemyList == null || _enemyList.Count() == 0) return null;

            float shortDistance = Mathf.Infinity;

            Multi_Enemy _returnEnemy = null;
            foreach (Multi_Enemy _enemy in _enemyList)
            {
                if (_enemy != null && _enemy.IsDead == false)
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

        public Multi_Enemy[] GetProximateEnemys(Vector3 _unitPos, int count, IReadOnlyList<Multi_Enemy> enemys)
        {
            Debug.Assert(enemys.Count > count, $"적 카운트 수가 {enemys.Count}이 배열의 크기인 {count}보다 \n 크지 않은 상태에서 함수가 실행됨.");

            List<Multi_Enemy> targets = new List<Multi_Enemy>(enemys);
            Multi_Enemy[] result = new Multi_Enemy[count];

            for (int i = 0; i < count; i++)
            {
                result[i] = GetProximateEnemy(_unitPos, targets);
                targets.Remove(result[i]);
            }
            return result;
        }
    }
}
