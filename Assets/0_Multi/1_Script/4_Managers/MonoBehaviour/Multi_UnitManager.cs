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
        _master.OnUnitFlagChanged += RaiseOnUnitFlagChanged;

        _stat.Init(_master);
    }


    // Datas
    public IReadOnlyDictionary<UnitClass, int> CountByUnitClass => _enemyPlayer._countByUnitClass;
    public IReadOnlyDictionary<UnitFlags, int> UnitCountByFlag => _count._countByFlag;
    public int CurrentUnitCount => _count._currentCount;
    public int EnemyPlayerHasCount => _enemyPlayer.EnemyPlayerHasUnitAllCount;
    public bool HasUnit(UnitFlags flag, int needCount = 1) => _count.HasUnit(flag, needCount);


    // events
    public event Action<int> OnUnitCountChanged = null;
    void Rasie_OnUnitCountChanged(int count) => OnUnitCountChanged?.Invoke(count);

    public event Action<UnitFlags, int> OnUnitFlagCountChanged = null;
    void Rasie_OnUnitFlagCountChanged(UnitFlags flag, int count) => OnUnitFlagCountChanged?.Invoke(flag, count);

    public event Action<bool, UnitFlags> OnTryCombine = null;
    void RaiseOnTryCombine(bool isSuccess, UnitFlags flag) => OnTryCombine?.Invoke(isSuccess, flag);

    public event Action<UnitFlags, bool> OnUnitFlagChanged;
    void RaiseOnUnitFlagChanged(UnitFlags flag, bool isIncrease) => OnUnitFlagChanged?.Invoke(flag, isIncrease);

    public event Action<int> OnOtherUnitCountChanged;
    void RaiseOnOtherUnitCountChaned(int count) => OnOtherUnitCountChanged?.Invoke(count);


    // RPC Funtions
    public bool TryCombine_RPC(UnitFlags flag)
    {
        bool result = _combine.CheckCombineable(flag);
        photonView.RPC("TryCombine", RpcTarget.MasterClient, flag, Multi_Data.instance.Id, result);
        return result;
    }
    [PunRPC] void TryCombine(UnitFlags flag, int id, bool isSuccess) => _combine.TryCombine(flag, id, isSuccess);


    public void UnitDead_RPC(int id, UnitFlags unitFlag, int count = 1) => photonView.RPC("UnitDead", RpcTarget.MasterClient, id, unitFlag, count);
    [PunRPC] void UnitDead(int id, UnitFlags unitFlag, int count) => _controller.UnitDead(id, unitFlag, count);


    public void UnitWorldChanged_RPC(int id, UnitFlags flag) => Instance.photonView.RPC("UnitWorldChanged", RpcTarget.MasterClient, id, flag, Multi_Managers.Camera.IsLookEnemyTower);
    [PunRPC] void UnitWorldChanged(int id, UnitFlags flag, bool enterStroyMode) => _controller.UnitWorldChanged(id, flag, enterStroyMode);


    public void UnitStatChange_RPC(UnitStatType type, UnitFlags flag, int value) => photonView.RPC("UnitStatChange", RpcTarget.MasterClient, (int)type, flag, value, Multi_Data.instance.Id);
    [PunRPC] void UnitStatChange(int typeNum, UnitFlags flag, int value, int id) => _stat.UnitStatChange(typeNum, flag, value, id);


    // Components
    class MasterDataManager
    {
        public RPCAction<int> OnAllUnitCountChanged = new RPCAction<int>();
        public RPCAction<UnitFlags, int> OnUnitCountChanged = new RPCAction<UnitFlags, int>();
        public RPCAction<UnitFlags, bool> OnUnitFlagChanged = new RPCAction<UnitFlags, bool>();

        public RPCAction<UnitFlags, bool> OnUnitFlagChangedToOther = new RPCAction<UnitFlags, bool>();

        RPCData<Dictionary<UnitFlags, List<Multi_TeamSoldier>>> _unitListByFlag = new RPCData<Dictionary<UnitFlags, List<Multi_TeamSoldier>>>();
        RPCData<List<Multi_TeamSoldier>> _currentAllUnitsById = new RPCData<List<Multi_TeamSoldier>>();

        List<Multi_TeamSoldier> GetUnitList(Multi_TeamSoldier unit) => GetUnitList(unit.GetComponent<RPCable>().UsingId, unit.UnitFlags);
        public List<Multi_TeamSoldier> GetUnitList(int id, UnitFlags flag) => _unitListByFlag.Get(id)[flag];
        public List<Multi_TeamSoldier> GetUnitList(int id) => _currentAllUnitsById.Get(id);

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
            Multi_SpawnManagers.NormalUnit.OnDead += RemoveUnit;
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
            RaiseEvents(unit, true);
        }

        void RemoveUnit(Multi_TeamSoldier unit)
        {
            int id = unit.GetComponent<RPCable>().UsingId;
            GetUnitList(unit).Remove(unit);
            _currentAllUnitsById.Get(id).Remove(unit);
            RaiseEvents(unit, false);
        }

        void RaiseEvents(Multi_TeamSoldier unit, bool isAdd)
        {
            int id = unit.GetComponent<RPCable>().UsingId;
            OnUnitCountChanged?.RaiseEvent(id, unit.UnitFlags, GetUnitList(unit).Count);
            OnAllUnitCountChanged?.RaiseEvent(id, _currentAllUnitsById.Get(id).Count);
            OnUnitFlagChanged?.RaiseEvent(id, unit.UnitFlags, isAdd);

            OnUnitFlagChangedToOther?.RaiseEventToOther(id, unit.UnitFlags, isAdd);
        }
    }

    class EnemyPlayerDataManager
    {
        public event Action<int> OnOtherUnitCountChanged;
        public Dictionary<UnitClass, int> _countByUnitClass = new Dictionary<UnitClass, int>();
        public int EnemyPlayerHasUnitAllCount { get; private set; }

        public void Init(MasterDataManager masterData)
        {
            foreach (UnitClass _class in Enum.GetValues(typeof(UnitClass)))
                _countByUnitClass.Add(_class, 0);

            masterData.OnUnitFlagChangedToOther += SetCount;
        }

        void SetCount(UnitFlags flag, bool isIncreased)
        {
            if (isIncreased)
            {
                _countByUnitClass[flag.UnitClass]++;
                EnemyPlayerHasUnitAllCount++;
            }
            else
            {
                _countByUnitClass[flag.UnitClass]--;
                EnemyPlayerHasUnitAllCount--;
            }

            OnOtherUnitCountChanged?.Invoke(EnemyPlayerHasUnitAllCount);
        }
    }

    class UnitContorller
    {
        MasterDataManager _masterData;
        public void Init(MasterDataManager masterData)
        {
            _masterData = masterData;
            if (PhotonNetwork.IsMasterClient)
            {
                Multi_SpawnManagers.BossEnemy.OnSpawn += AllUnitTargetToBoss;
                Multi_SpawnManagers.BossEnemy.OnDead += AllUnitUpdateTarget;
            }
        }

        public void UnitDead(int id, UnitFlags unitFlag, int count = 1)
        {
            if (PhotonNetwork.IsMasterClient == false) return;

            Multi_TeamSoldier[] offerings = _masterData.GetUnitList(id, unitFlag).ToArray();
            count = Mathf.Min(count, offerings.Length);
            for (int i = 0; i < count; i++)
                offerings[i].Dead();
        }

        public void UnitWorldChanged(int id, UnitFlags flag, bool enterStroyMode)
        {
            if (_masterData.TryGetUnit_If(id, flag, out Multi_TeamSoldier unit, (_unit) => _unit.EnterStroyWorld == enterStroyMode))
                unit.ChagneWorld();
        }

        void AllUnitTargetToBoss(Multi_BossEnemy boss)
        {
            _masterData.GetUnitList(boss.GetComponent<RPCable>().UsingId)
                .Where(x => x.EnterStroyWorld == false)
                .ToList()
                .ForEach(x => x.SetChaseSetting(boss.gameObject));
        }

        void AllUnitUpdateTarget(Multi_BossEnemy boss)
            => _masterData.GetUnitList(boss.GetComponent<RPCable>().UsingId).ForEach(x => x.UpdateTarget());
    }

    class UnitCountManager
    {
        public int _currentCount = 0;
        public Dictionary<UnitFlags, int> _countByFlag = new Dictionary<UnitFlags, int>(); // 모든 플레이어가 이벤트로 받아서 각자 카운트 관리

        public event Action<int> OnUnitCountChanged = null;
        public event Action<UnitFlags, int> OnUnitFlagCountChanged = null;

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

        void Riase_OnUnitCountChanged(int count)
        {
            _currentCount = count;
            OnUnitCountChanged?.Invoke(count);
        }

        void Riase_OnUnitCountChanged(UnitFlags flag, int count)
        {
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

            int addGold = Multi_Managers.Data.Skill.CombineAdditionalGold;

            var conditions = Multi_Managers.Data.CombineConditionByUnitFalg[flag].NeedCountByFlag;
            foreach (var item in conditions)
            {
                if (item.Key == new UnitFlags(2, 0))
                {
                    for (int i = 0; i < item.Value; i++)
                        Multi_GameManager.instance.AddGold(addGold);
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