using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Photon.Pun;

public class Multi_UnitManager : SingletonPun<Multi_UnitManager>
{
    UnitCountManager _count = new UnitCountManager();
    UnitContorller _controller = new UnitContorller();
    EnemyPlayerDataManager _enemyPlayer = new EnemyPlayerDataManager();
    MasterDataManager _master = new MasterDataManager();
    UnitPassiveManager _passive = new UnitPassiveManager();

    UnitStatChangeFacade _statFacade;
    public UnitStatChangeFacade Stat => _statFacade;

    public MasterDataManager Master => _master;

    UnitManagerController _unitManagerController;

    [SerializeField] List<Multi_TeamSoldier> _units;
    public void AddUnit(Multi_TeamSoldier unit)
    {
        _units.Add(unit);
        unit.OnDead += (dieUnit) => _units.Remove(dieUnit);
    }

    protected override void Init()
    {
        // 이 지옥의 꽃같은 코드 제거를 위해 싱글턴 씬 이동 처리를 잘할 것
        // if (Managers.Scene.IsBattleScene == false) return;
        base.Init();

        _count.Init(_master);
        _count.OnUnitCountChanged += Rasie_OnUnitCountChanged;
        _count.OnUnitFlagCountChanged += Rasie_OnUnitFlagCountChanged;

        _enemyPlayer.Init(_master);
        _enemyPlayer.OnOtherUnitCountChanged += RaiseOnOtherUnitCountChaned;

        _passive.Init();

        _statFacade = gameObject.AddComponent<UnitStatChangeFacade>();
        _statFacade.Init(Managers.Multi.Data, Instance);

        _unitManagerController = new UnitManagerController();

        if (PhotonNetwork.IsMasterClient == false) return;
        _controller.Init(_master);

        _master.Init();
    }

    // Datas
    public IReadOnlyDictionary<UnitClass, int> EnemyPlayerUnitCountByClass => _enemyPlayer._countByUnitClass;
    public IReadOnlyDictionary<UnitFlags, int> UnitCountByFlag => _count._countByFlag;
    public int CurrentUnitCount => _count._currentCount;
    public IEnumerable<UnitFlags> CombineableUnitFlags
      => new UnitCombineSystem().GetCombinableUnitFalgs((flag) => UnitCountByFlag[flag]);
    public bool HasUnit(UnitFlags flag, int needCount = 1) => _count.HasUnit(flag, needCount);


    public event Action<UnitFlags> OnCombine = null;
    public event Action OnFailedCombine = null;

    // events
    public event Action<int> OnUnitCountChanged = null;
    void Rasie_OnUnitCountChanged(byte count) => OnUnitCountChanged?.Invoke(count);

    public event Action<UnitFlags, int> OnUnitFlagCountChanged = null;
    void Rasie_OnUnitFlagCountChanged(UnitFlags flag, byte count) => OnUnitFlagCountChanged?.Invoke(flag, count);

    public event Action<int> OnOtherUnitCountChanged;
    void RaiseOnOtherUnitCountChaned(int count) => OnOtherUnitCountChanged?.Invoke(count);

    public void KillUnit(Multi_TeamSoldier unit) // 이게 맞나?
    {
        if (unit == null) return;

        unit.Dead();
        _master.RemoveUnit(unit);
    }

    // RPC Funtions....

    public bool TryCombine_RPC(UnitFlags flag)
    {
        if (new UnitCombineSystem().CheckCombineable(flag, (conditionFlag) => _count.GetUnitCount(conditionFlag)))
        {
            photonView.RPC(nameof(Combine), RpcTarget.MasterClient, flag, PlayerIdManager.Id);
            OnCombine?.Invoke(flag);
            return true;
        }
        else
        {
            OnFailedCombine?.Invoke();
            return false;
        }
    }

    [PunRPC]
    public void Combine(UnitFlags flag, byte id)
    {
        SacrificedUnits_ForCombine(Managers.Data.CombineConditionByUnitFalg[flag]);
        Multi_SpawnManagers.NormalUnit.Spawn(flag, id);

        void SacrificedUnits_ForCombine(CombineCondition condition)
            => condition.NeedCountByFlag
            .ToList()
            .ForEach(x => _controller.UnitDead(id, x.Key, x.Value));
    }

    public Multi_TeamSoldier FindUnit(Func<Multi_TeamSoldier, bool> condition) => _units
        .Where(condition)
        .FirstOrDefault();
    public Multi_TeamSoldier FindUnit(UnitFlags flag) => FindUnit(x => x.UnitFlags == flag);
    public bool TryFindUnit(Func<Multi_TeamSoldier, bool> condition, out Multi_TeamSoldier result)
    {
        result = FindUnit(condition);
        return result != null;
    }

    public void UnitDead_RPC(byte id, UnitFlags unitFlag, int count = 1) => photonView.RPC(nameof(UnitDead), RpcTarget.MasterClient, id, unitFlag, (byte)count);
    [PunRPC] void UnitDead(byte id, UnitFlags unitFlag, byte count) => _controller.UnitDead(id, unitFlag, count);

    public void UnitWorldChanged_RPC(byte id, UnitFlags flag) => Instance.photonView.RPC(nameof(UnitWorldChanged), RpcTarget.MasterClient, id, flag, Managers.Camera.IsLookEnemyTower);
    [PunRPC] void UnitWorldChanged(byte id, UnitFlags flag, bool enterStroyMode) => _controller.UnitWorldChange(id, flag, enterStroyMode);

    public Multi_TeamSoldier FindUnit(byte id, UnitClass unitClass)
    {
        if (PhotonNetwork.IsMasterClient == false) return null;
        var units = _master.GetUnits(id, (unit) => unit.unitClass == unitClass);
        return units.Count() == 0 ? null : units.First();
    }

    public Multi_TeamSoldier FindUnit(int id, UnitFlags flag)
    {
        if (PhotonNetwork.IsMasterClient == false) return null;
        return _master.GetUnitList(id, flag).FirstOrDefault();
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

        public bool TryGetUnit_If(byte id, UnitFlags flag, out Multi_TeamSoldier unit, Func<Multi_TeamSoldier, bool> condition = null)
        {
            foreach (Multi_TeamSoldier loopUnit in GetUnitList(id, flag))
            {
                if (condition == null || condition(loopUnit))
                {
                    unit = loopUnit;
                    return true;
                }
            }

            unit = null;
            return false;
        }

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
            UpdateUnitCount(unit);
        }

        public void RemoveUnit(Multi_TeamSoldier unit) // Remove는 최적화때문에 여기서 Count 갱신 안 함
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

    class UnitContorller
    {
        MasterDataManager _masterData;
        public void Init(MasterDataManager masterData) => _masterData = masterData;

        public void UnitDead(byte id, UnitFlags unitFlag, int count = 1)
        {
            if (PhotonNetwork.IsMasterClient == false) return;

            Multi_TeamSoldier[] units = _masterData.GetUnitList(id, unitFlag).ToArray();
            count = Mathf.Min(count, units.Length);
            for (int i = 0; i < count; i++)
                Instance.KillUnit(units[i]);
        }

        public void UnitWorldChange(byte id, UnitFlags flag, bool enterStroyMode)
        {
            if (_masterData.TryGetUnit_If(id, flag, out Multi_TeamSoldier unit, (_unit) => _unit.EnterStroyWorld == enterStroyMode))
                unit.ChangeWorld();
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

        public bool HasUnit(UnitFlags flag, int needCount = 1) => _countByFlag[flag] >= needCount;
        public int GetUnitCount(UnitFlags flag) => _countByFlag[flag];
    }

    class UnitPassiveManager
    {
        void CombineGold(UnitFlags flag)
        {
            var conditions = Managers.Data.CombineConditionByUnitFalg[flag].NeedCountByFlag;
            foreach (var item in conditions)
            {
                if (item.Key == new UnitFlags(2, 0))
                {
                    var manager = Multi_GameManager.Instance;
                    for (int i = 0; i < item.Value; i++)
                        manager.AddGold(manager.BattleData.YellowKnightRewardGold);
                }
            }
        }

        public void Init()
        {
            Instance.OnCombine += CombineGold;
        }
    }
}

public enum UnitStatType
{
    Damage,
    BossDamage,
    All,
}