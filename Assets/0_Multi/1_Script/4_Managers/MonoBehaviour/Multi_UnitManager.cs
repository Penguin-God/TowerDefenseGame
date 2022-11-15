using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Photon.Pun;

public class Multi_UnitManager : MonoBehaviourPun
{
    private static Multi_UnitManager instance = null;
    public static Multi_UnitManager Instance
    {
        get
        {
            if (_isDestory) return null;

            if(instance == null)
            {
                instance = FindObjectOfType<Multi_UnitManager>();
                if (instance == null)
                    instance = new GameObject("Multi_UnitManager").AddComponent<Multi_UnitManager>();
                instance.Init();
            }
            return instance;
        }
    }

    CombineSystem _combine = new CombineSystem();
    UnitCountManager _count = new UnitCountManager();
    UnitContorller _controller = new UnitContorller();
    EnemyPlayerDataManager _enemyPlayer = new EnemyPlayerDataManager();
    MasterDataManager _master = new MasterDataManager();
    UnitStatChanger _stat = new UnitStatChanger();
    UnitPassiveManager _passive = new UnitPassiveManager();

    void Awake()
    {
        _isDestory = false;
    }

    private static bool _isDestory;
    void OnDestroy()
    {
        _isDestory = true;    
    }

    void Init()
    {
        if (Multi_Managers.Scene.IsBattleScene == false) return;

        _count.Init(_master);
        _count.OnUnitCountChanged += Rasie_OnUnitCountChanged;
        _count.OnUnitFlagCountChanged += Rasie_OnUnitFlagCountChanged;

        _enemyPlayer.Init(_master);
        _enemyPlayer.OnOtherUnitCountChanged += RaiseOnOtherUnitCountChaned;

        _passive.Init();

        _combine.Init(_count, _controller);
        _combine.OnTryCombine += RaiseOnTryCombine;

        if (PhotonNetwork.IsMasterClient == false) return;

        _controller.Init(_master);

        _master.Init();
        _stat.Init(_master);
    }

    // Datas
    public IReadOnlyDictionary<UnitClass, int> EnemyPlayerUnitCountByClass => _enemyPlayer._countByUnitClass;
    public IReadOnlyDictionary<UnitFlags, int> UnitCountByFlag => _count._countByFlag;
    public int CurrentUnitCount => _count._currentCount;
    public int EnemyPlayerHasCount => _enemyPlayer.EnemyPlayerHasUnitAllCount;
    public bool HasUnit(UnitFlags flag, int needCount = 1) => _count.HasUnit(flag, needCount);


    // events
    public event Action<int> OnUnitCountChanged = null;
    void Rasie_OnUnitCountChanged(byte count) => OnUnitCountChanged?.Invoke(count);

    public event Action<UnitFlags, int> OnUnitFlagCountChanged = null;
    void Rasie_OnUnitFlagCountChanged(UnitFlags flag, byte count) => OnUnitFlagCountChanged?.Invoke(flag, count);

    public event Action<bool, UnitFlags> OnTryCombine = null;
    void RaiseOnTryCombine(bool isSuccess, UnitFlags flag) => OnTryCombine?.Invoke(isSuccess, flag);

    public event Action<int> OnOtherUnitCountChanged;
    void RaiseOnOtherUnitCountChaned(int count) => OnOtherUnitCountChanged?.Invoke(count);


    // RPC Funtions....
    public bool TryCombine_RPC(UnitFlags flag)
    {
        bool result = _combine.CheckCombineable(flag);
        photonView.RPC(nameof(TryCombine), RpcTarget.MasterClient, flag, Multi_Data.instance.Id, result);
        return result;
    }
    [PunRPC] void TryCombine(UnitFlags flag, int id, bool isSuccess) => _combine.TryCombine(flag, id, isSuccess);


    public void UnitDead_RPC(int id, UnitFlags unitFlag, int count = 1) => photonView.RPC(nameof(UnitDead), RpcTarget.MasterClient, id, unitFlag, count);
    [PunRPC] void UnitDead(int id, UnitFlags unitFlag, int count) => _controller.UnitDead(id, unitFlag, count);
    
    public void UnitColorChanged_RPC(int id, UnitFlags flag, int changeTargetColor) => photonView.RPC(nameof(UnitColorChanged), RpcTarget.MasterClient, id, flag, changeTargetColor);
    [PunRPC] void UnitColorChanged(int id, UnitFlags flag, int changeTargetColor) => _controller.UnitColorChange(id, flag, changeTargetColor);

    public void UnitWorldChanged_RPC(int id, UnitFlags flag) => Instance.photonView.RPC(nameof(UnitWorldChanged), RpcTarget.MasterClient, id, flag, Multi_Managers.Camera.IsLookEnemyTower);
    [PunRPC] void UnitWorldChanged(int id, UnitFlags flag, bool enterStroyMode) => _controller.UnitWorldChange(id, flag, enterStroyMode);


    public void UnitStatChange_RPC(UnitStatType type, UnitFlags flag, int value) => photonView.RPC(nameof(UnitStatChange), RpcTarget.MasterClient, (int)type, flag, value, Multi_Data.instance.Id);
    [PunRPC] void UnitStatChange(int typeNum, UnitFlags flag, int value, int id) => _stat.UnitStatChange(typeNum, flag, value, id);


    // Components
    class MasterDataManager
    {
        public RPCAction<byte> OnAllUnitCountChanged = new RPCAction<byte>();
        public RPCAction<byte, UnitFlags, byte> OnUnitCountChanged = new RPCAction<byte, UnitFlags, byte>();

        RPCData<Dictionary<UnitFlags, List<Multi_TeamSoldier>>> _unitListByFlag = new RPCData<Dictionary<UnitFlags, List<Multi_TeamSoldier>>>();
        RPCData<List<Multi_TeamSoldier>> _currentAllUnitsById = new RPCData<List<Multi_TeamSoldier>>();

        List<Multi_TeamSoldier> GetUnitList(Multi_TeamSoldier unit) => GetUnitList(unit.GetComponent<RPCable>().UsingId, unit.UnitFlags);
        public List<Multi_TeamSoldier> GetUnitList(int id, UnitFlags flag) => _unitListByFlag.Get(id)[flag];
        public Multi_TeamSoldier GetRandomUnit(int id, Func<Multi_TeamSoldier, bool> condition = null)
        {
            if (condition == null) return _currentAllUnitsById.Get(id).GetRandom();
            var list = _currentAllUnitsById.Get(id).Where(x => condition(x)).ToList();
            return list.Count == 0 ? null : list.GetRandom();
        }

        public void Init()
        {
            foreach (var data in Multi_SpawnManagers.NormalUnit.AllUnitDatas)
            {
                foreach (Multi_TeamSoldier unit in data.gos.Select(x => x.GetComponent<Multi_TeamSoldier>()))
                {
                    _unitListByFlag.Get(0).Add(new UnitFlags(unit.unitColor, unit.unitClass), new List<Multi_TeamSoldier>());
                    _unitListByFlag.Get(1).Add(new UnitFlags(unit.unitColor, unit.unitClass), new List<Multi_TeamSoldier>());
                }
            }

            Multi_SpawnManagers.NormalUnit.OnSpawn += AddUnit;
            // Multi_SpawnManagers.NormalUnit.OnDead += RemoveUnit;
        }

        public bool TryGetUnit_If(int id, UnitFlags flag, out Multi_TeamSoldier unit, Func<Multi_TeamSoldier, bool> condition = null)
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

        void AddUnit(Multi_TeamSoldier unit)
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
        }

        public void UpdateUnitCount(Multi_TeamSoldier unit)
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
            if (Multi_Data.instance.Id == id) return;

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

        public void UnitDead(int id, UnitFlags unitFlag, int count = 1)
        {
            if (PhotonNetwork.IsMasterClient == false) return;

            Multi_TeamSoldier[] units = _masterData.GetUnitList(id, unitFlag).ToArray();
            count = Mathf.Min(count, units.Length);
            for (int i = 0; i < count; i++)
            {
                units[i].Dead();
                _masterData.RemoveUnit(units[i]);
            }
            _masterData.UpdateUnitCount(units[0]);
        }

        public void UnitWorldChange(int id, UnitFlags flag, bool enterStroyMode)
        {
            if (_masterData.TryGetUnit_If(id, flag, out Multi_TeamSoldier unit, (_unit) => _unit.EnterStroyWorld == enterStroyMode))
                unit.ChagneWorld();
        }

        public void UnitColorChange(int id, UnitFlags dieUnitFlag, int changeTargetColor)
        {
            var unit = _masterData.GetRandomUnit(id,
                    (_unit) => _unit.unitClass == dieUnitFlag.UnitClass && _unit.unitColor != UnitColor.black && _unit.unitColor != UnitColor.white);
            if (unit == null) return;

            Multi_SpawnManagers.NormalUnit.Spawn(changeTargetColor, (int)unit.unitClass, unit.transform.position, unit.transform.rotation, id);
            UnitDead(unit);
        }

        void UnitDead(Multi_TeamSoldier unit)
        {
            unit.Dead();
            _masterData.RemoveUnit(unit);
            _masterData.UpdateUnitCount(unit);
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
            foreach (var data in Multi_SpawnManagers.NormalUnit.AllUnitDatas)
            {
                foreach (Multi_TeamSoldier unit in data.gos.Select(x => x.GetComponent<Multi_TeamSoldier>()))
                    _countByFlag.Add(new UnitFlags(unit.unitColor, unit.unitClass), 0);
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
            if (Multi_Data.instance.Id != id) return;

            _countByFlag[flag] = count;
            OnUnitFlagCountChanged?.Invoke(flag, count);
        }

        public bool HasUnit(UnitFlags flag, int needCount = 1) => _countByFlag[flag] >= needCount;
    }

    class CombineSystem
    {
        public RPCAction<bool, UnitFlags> OnTryCombine = new RPCAction<bool, UnitFlags>();
        UnitCountManager _countManager;
        UnitContorller _unitContorller;

        public void Init(UnitCountManager countManager, UnitContorller unitContorller)
        {
            _countManager = countManager;
            _unitContorller = unitContorller;
        }

        public bool CheckCombineable(UnitFlags flag)
            => Multi_Managers.Data.CombineConditionByUnitFalg[flag].NeedCountByFlag.All(x => _countManager.HasUnit(x.Key, x.Value));

        public void TryCombine(UnitFlags flag, int id, bool isSuccess)
        {
            Debug.Assert(PhotonNetwork.IsMasterClient, "마스터가 아닌데 유닛 조합을 하려고 함");

            if (isSuccess)
                Combine(flag, id);
            else
                OnTryCombine?.RaiseEvent(id, false, flag);
        }

        void Combine(UnitFlags flag, int id)
        {
            if (PhotonNetwork.IsMasterClient == false) return;

            SacrificedUnits_ForCombine(Multi_Managers.Data.CombineConditionByUnitFalg[flag]);
            Multi_SpawnManagers.NormalUnit.Spawn(flag, id);
            OnTryCombine?.RaiseEvent(id, true, flag);

            void SacrificedUnits_ForCombine(CombineCondition condition) => condition.NeedCountByFlag.ToList().ForEach(x => _unitContorller.UnitDead(id, x.Key, x.Value));
        }
    }

    class UnitStatChanger
    {
        MasterDataManager _masterData;
        public void Init(MasterDataManager masterData)
        {
            _masterData = masterData;
        }

        public void UnitStatChange(int typeNum, UnitFlags flag, int value, int id)
        {
            switch (typeNum)
            {
                case 0: ChangeDamage(flag, value, id); break;
                case 1: ChangeBossDamage(flag, value, id); break;
                case 2: ChangeAllDamage(flag, value, id); break;
            }
        }

        void ChangeDamage(UnitFlags flag, int value, int id)
        {
            foreach (var unit in _masterData.GetUnitList(id, flag))
                unit.Damage = value;
        }

        void ChangeBossDamage(UnitFlags flag, int value, int id)
        {
            foreach (var unit in _masterData.GetUnitList(id, flag))
                unit.BossDamage = value;
        }

        void ChangeAllDamage(UnitFlags flag, int value, int id)
        {
            ChangeDamage(flag, value, id);
            ChangeBossDamage(flag, value, id);
        }
    }

    class UnitPassiveManager
    {
        void CombineGold(bool isSuccess, UnitFlags flag)
        {
            if (isSuccess == false) return;

            var conditions = Multi_Managers.Data.CombineConditionByUnitFalg[flag].NeedCountByFlag;
            foreach (var item in conditions)
            {
                if (item.Key == new UnitFlags(2, 0))
                {
                    var manager = Multi_GameManager.instance;
                    for (int i = 0; i < item.Value; i++)
                        manager.AddGold(manager.BattleData.YellowKnightRewardGold);
                }
            }
        }

        public void Init()
        {
            Instance.OnTryCombine += CombineGold;
        }
    }
}

public enum UnitStatType
{
    Damage,
    BossDamage,
    All,
}