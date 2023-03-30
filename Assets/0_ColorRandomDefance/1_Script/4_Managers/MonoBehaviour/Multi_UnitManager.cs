using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Photon.Pun;

public class Multi_UnitManager : SingletonPun<Multi_UnitManager>
{
    EnemyPlayerDataManager _enemyPlayer = new EnemyPlayerDataManager();
    MasterDataManager _master = new MasterDataManager();

    public MasterDataManager Master => _master;

    [SerializeField] List<Multi_TeamSoldier> _units;
    public int CurrentUnitCount => _units.Count;
    public HashSet<UnitFlags> ExsitUnitFlags => new HashSet<UnitFlags>(_units.Select(x => x.UnitFlags));

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
        OnUnitCountChangeByClass?.Invoke(unit.UnitClass, FindUnits(x => x.UnitClass == unit.UnitClass).Count());
    }

    protected override void Init()
    {
        // 이 지옥의 꽃같은 코드 제거를 위해 싱글턴 씬 이동 처리를 잘할 것
        // if (Managers.Scene.IsBattleScene == false) return;
        base.Init();
        _combineSystem = new UnitCombineSystem(Managers.Data.CombineConditionByUnitFalg);
        
        _enemyPlayer.Init(_master);
        _enemyPlayer.OnOtherUnitCountChanged += RaiseOnOtherUnitCountChaned;

        if (PhotonNetwork.IsMasterClient == false) return;
        _master.Init();
    }

    UnitCombineSystem _combineSystem;
    public event Action<UnitFlags> OnCombine = null;
    public event Action OnFailedCombine = null;
    public IEnumerable<UnitFlags> CombineableUnitFlags => _combineSystem.GetCombinableUnitFalgs(GetUnitCount);

    public bool TryCombine(UnitFlags flag)
    {
        if (_combineSystem.CheckCombineable(flag, GetUnitCount))
        {
            Combine(flag);
            OnCombine?.Invoke(flag);
            return true;
        }
        else
        {
            OnFailedCombine?.Invoke();
            return false;
        }
    }

    void Combine(UnitFlags flag)
    {
        foreach (var needFlag in _combineSystem.GetNeedFlags(flag))
            FindUnit(needFlag).Dead();

        Multi_SpawnManagers.NormalUnit.Spawn(flag, PlayerIdManager.Id);
    }


    public Multi_TeamSoldier FindUnit(UnitFlags flag) => FindUnit(x => x.UnitFlags == flag);
    public bool TryFindUnit(Func<Multi_TeamSoldier, bool> condition, out Multi_TeamSoldier result)
    {
        result = FindUnit(condition);
        return result != null;
    }
    public Multi_TeamSoldier FindUnit(Func<Multi_TeamSoldier, bool> condition) => FindUnits(condition).FirstOrDefault();
    public int GetUnitCount(UnitFlags flag) => FindUnits(x => x.UnitFlags == flag).Count();
    public IEnumerable<Multi_TeamSoldier> FindUnits(Func<Multi_TeamSoldier, bool> condition) => _units.Where(condition);


    // Lagacys
    public event Action<int> OnOtherUnitCountChanged;
    void RaiseOnOtherUnitCountChaned(int count) => OnOtherUnitCountChanged?.Invoke(count);
    public IReadOnlyDictionary<UnitClass, int> EnemyPlayerUnitCountByClass => _enemyPlayer._countByUnitClass;

    // Components
    public class MasterDataManager
    {
        public RPCAction<byte, UnitFlags, byte> OnUnitCountChanged = new RPCAction<byte, UnitFlags, byte>();

        RPCData<Dictionary<UnitFlags, List<Multi_TeamSoldier>>> _unitListByFlag = new RPCData<Dictionary<UnitFlags, List<Multi_TeamSoldier>>>();
        RPCData<List<Multi_TeamSoldier>> _currentAllUnitsById = new RPCData<List<Multi_TeamSoldier>>();

        List<Multi_TeamSoldier> GetUnitList(Multi_TeamSoldier unit) => GetUnitList(unit.GetComponent<RPCable>().UsingId, unit.UnitFlags);
        List<Multi_TeamSoldier> GetUnitList(int id, UnitFlags flag) => _unitListByFlag.Get(id)[flag];

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
}