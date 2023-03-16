using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Photon.Pun;
using static UnityEngine.UI.CanvasScaler;

public class Multi_UnitManager : SingletonPun<Multi_UnitManager>
{
    UnitCountManager _count = new UnitCountManager();
    EnemyPlayerDataManager _enemyPlayer = new EnemyPlayerDataManager();
    MasterDataManager _master = new MasterDataManager();
    
    UnitStatChangeFacade _statFacade;
    public UnitStatChangeFacade Stat => _statFacade;

    public MasterDataManager Master => _master;

    [SerializeField] List<Multi_TeamSoldier> _units;
    public Action<int> OnUnitCountChange = null;
    public Action<UnitFlags, int> OnUnitCountChangeByFlag = null;
    public Action<UnitClass, int> OnUnitCountChangeByClass = null;
    public void AddUnit(Multi_TeamSoldier unit)
    {
        _units.Add(unit);
        NotifyChangeUnitCount(unit);
        unit.OnDead += RemoveUnit;
    }

    void RemoveUnit(Multi_TeamSoldier unit)
    {
        _units.Remove(unit);
        NotifyChangeUnitCount(unit);
    }

    void NotifyChangeUnitCount(Multi_TeamSoldier unit)
    {
        OnUnitCountChange?.Invoke(_units.Count);
        OnUnitCountChangeByFlag?.Invoke(unit.UnitFlags, FindUnits(x => x.UnitFlags == unit.UnitFlags).Count());
        OnUnitCountChangeByClass?.Invoke(unit.unitClass, FindUnits(x => x.unitClass == unit.unitClass).Count());
    }

    protected override void Init()
    {
        // 이 지옥의 꽃같은 코드 제거를 위해 싱글턴 씬 이동 처리를 잘할 것
        // if (Managers.Scene.IsBattleScene == false) return;
        base.Init();
        _combineSystem = new UnitCombineSystem(Managers.Data.CombineConditionByUnitFalg);
        _count.Init(_master);
        _count.OnUnitFlagCountChanged += Rasie_OnUnitFlagCountChanged;

        _enemyPlayer.Init(_master);
        _enemyPlayer.OnOtherUnitCountChanged += RaiseOnOtherUnitCountChaned;

        _statFacade = gameObject.AddComponent<UnitStatChangeFacade>();
        _statFacade.Init(Managers.Multi.Data, Instance);

        if (PhotonNetwork.IsMasterClient == false) return;
        _master.Init();
    }

    UnitCombineSystem _combineSystem;

    // Datas
    public IReadOnlyDictionary<UnitClass, int> EnemyPlayerUnitCountByClass => _enemyPlayer._countByUnitClass;
    public IReadOnlyDictionary<UnitFlags, int> UnitCountByFlag => _count._countByFlag;
    public int CurrentUnitCount => _count._currentCount;
    public IEnumerable<UnitFlags> CombineableUnitFlags
      => _combineSystem.GetCombinableUnitFalgs((flag) => UnitCountByFlag[flag]);


    public event Action<UnitFlags> OnCombine = null;
    public event Action OnFailedCombine = null;

    // events

    public event Action<UnitFlags, int> OnUnitFlagCountChanged = null;
    void Rasie_OnUnitFlagCountChanged(UnitFlags flag, byte count) => OnUnitFlagCountChanged?.Invoke(flag, count);

    public event Action<int> OnOtherUnitCountChanged;
    void RaiseOnOtherUnitCountChaned(int count) => OnOtherUnitCountChanged?.Invoke(count);

    public bool TryCombine(UnitFlags flag)
    {
        if (_combineSystem.CheckCombineable(flag, (conditionFlag) => _count.GetUnitCount(conditionFlag)))
        {
            Combine(flag, PlayerIdManager.Id);
            OnCombine?.Invoke(flag);
            return true;
        }
        else
        {
            OnFailedCombine?.Invoke();
            return false;
        }
    }

    void Combine(UnitFlags flag, byte id)
    {
        foreach (var needFlag in _combineSystem.GetNeedFlags(flag))
            FindUnit(needFlag).Dead();

        Multi_SpawnManagers.NormalUnit.Spawn(flag, id);
    }


    public Multi_TeamSoldier FindUnit(UnitFlags flag) => FindUnit(x => x.UnitFlags == flag);
    public bool TryFindUnit(Func<Multi_TeamSoldier, bool> condition, out Multi_TeamSoldier result)
    {
        result = FindUnit(condition);
        return result != null;
    }
    public Multi_TeamSoldier FindUnit(Func<Multi_TeamSoldier, bool> condition) => FindUnits(condition).FirstOrDefault();
    public IEnumerable<Multi_TeamSoldier> FindUnits(Func<Multi_TeamSoldier, bool> condition) => _units.Where(condition);


    public Multi_TeamSoldier FindUnit(byte id, UnitClass unitClass)
    {
        if (PhotonNetwork.IsMasterClient == false) return null;
        var units = _master.GetUnits(id, (unit) => unit.unitClass == unitClass);
        return units.Count() == 0 ? null : units.First();
    }

    // Components
    public class MasterDataManager
    {
        public RPCAction<byte> OnAllUnitCountChanged = new RPCAction<byte>();
        public RPCAction<byte, UnitFlags, byte> OnUnitCountChanged = new RPCAction<byte, UnitFlags, byte>();

        RPCData<Dictionary<UnitFlags, List<Multi_TeamSoldier>>> _unitListByFlag = new RPCData<Dictionary<UnitFlags, List<Multi_TeamSoldier>>>();
        RPCData<List<Multi_TeamSoldier>> _currentAllUnitsById = new RPCData<List<Multi_TeamSoldier>>();

        List<Multi_TeamSoldier> GetUnitList(Multi_TeamSoldier unit) => GetUnitList(unit.GetComponent<RPCable>().UsingId, unit.UnitFlags);
        public List<Multi_TeamSoldier> GetUnitList(int id, UnitFlags flag) => _unitListByFlag.Get(id)[flag];

        public IEnumerable<Multi_TeamSoldier> GetUnits(byte id, Func<Multi_TeamSoldier, bool> condition = null)
        {
            if (condition == null) return _currentAllUnitsById.Get(id);
            return _currentAllUnitsById.Get(id).Where(condition);
        }

        public void Init()
        {
            foreach (UnitColor color in Enum.GetValues(typeof(UnitColor)))
            {
                foreach (UnitClass unitClass in Enum.GetValues(typeof(UnitClass)))
                {
                    _unitListByFlag.Get(0).Add(new UnitFlags(color, unitClass), new List<Multi_TeamSoldier>());
                    _unitListByFlag.Get(1).Add(new UnitFlags(color, unitClass), new List<Multi_TeamSoldier>());
                }
            }
        }

        public void AddUnit(Multi_TeamSoldier unit)
        {
            int id = unit.GetComponent<RPCable>().UsingId;
            GetUnitList(unit).Add(unit);
            _currentAllUnitsById.Get(id).Add(unit);
            unit.OnDead += RemoveUnit;
            UpdateUnitCount(unit);
        }

        void RemoveUnit(Multi_TeamSoldier unit)
        {
            int id = unit.GetComponent<RPCable>().UsingId;
            GetUnitList(unit).Remove(unit);
            _currentAllUnitsById.Get(id).Remove(unit);
            UpdateUnitCount(unit);
        }

        void UpdateUnitCount(Multi_TeamSoldier unit)
        {
            int id = unit.GetComponent<RPCable>().UsingId;
            OnAllUnitCountChanged?.RaiseEvent(id, (byte)_currentAllUnitsById.Get(id).Count);
            OnUnitCountChanged?.RaiseAll((byte)id, unit.UnitFlags, (byte)GetUnitList(unit).Count);
        }
    }

    class EnemyPlayerDataManager
    {
        public event Action<int> OnOtherUnitCountChanged;
        Dictionary<UnitFlags, byte> _countByFlag = new Dictionary<UnitFlags, byte>();
        public Dictionary<UnitClass, int> _countByUnitClass = new Dictionary<UnitClass, int>();
        public int EnemyPlayerHasUnitAllCount { get; private set; } 

        public void Init(MasterDataManager masterData)
        {
            foreach (UnitClass _class in Enum.GetValues(typeof(UnitClass)))
                _countByUnitClass.Add(_class, 0);

            masterData.OnUnitCountChanged += SetCount;
        }

        void SetCount(byte id, UnitFlags flag, byte count)
        {
            if (PlayerIdManager.Id == id) return;

            if (_countByFlag.ContainsKey(flag) == false)
                _countByFlag.Add(flag, 0);

            _countByFlag[flag] = count;
            _countByUnitClass[flag.UnitClass] = GetUnitClassCount(flag.UnitClass);
            EnemyPlayerHasUnitAllCount = _countByUnitClass.Values.Sum();
            OnOtherUnitCountChanged?.Invoke(EnemyPlayerHasUnitAllCount);
        }

        int GetUnitClassCount(UnitClass unitClass)
        {
            int result = 0;
            foreach (var item in _countByFlag)
            {
                if (unitClass == item.Key.UnitClass)
                    result += item.Value;
            }
            return result;
        }
    }

    class UnitCountManager
    {
        public int _currentCount = 0;
        public Dictionary<UnitFlags, int> _countByFlag = new Dictionary<UnitFlags, int>(); // 모든 플레이어가 이벤트로 받아서 각자 카운트 관리

        public event Action<byte> OnUnitCountChanged = null;
        public event Action<UnitFlags, byte> OnUnitFlagCountChanged = null;

        public void Init(MasterDataManager masterData)
        {
            foreach (UnitColor color in Enum.GetValues(typeof(UnitColor)))
            {
                foreach (UnitClass unitClass in Enum.GetValues(typeof(UnitClass)))
                    _countByFlag.Add(new UnitFlags(color, unitClass), 0);
            }
            
            masterData.OnAllUnitCountChanged += Riase_OnUnitCountChanged;
            masterData.OnUnitCountChanged += Riase_OnUnitCountChanged;
        }

        void Riase_OnUnitCountChanged(byte count)
        {
            _currentCount = count;
            OnUnitCountChanged?.Invoke(count);
        }

        void Riase_OnUnitCountChanged(byte id, UnitFlags flag, byte count)
        {
            if (PlayerIdManager.Id != id) return;

            _countByFlag[flag] = count;
            OnUnitFlagCountChanged?.Invoke(flag, count);
        }

        public int GetUnitCount(UnitFlags flag) => _countByFlag[flag];
    }
}

public enum UnitStatType
{
    Damage,
    BossDamage,
    All,
}