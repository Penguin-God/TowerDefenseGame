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

    void Init()
    {
        if (Multi_Managers.Scene.CurrentSceneType != SceneTyep.New_Scene) return;

        _count.Init();
        _enemyPlayer.Init();

        if (PhotonNetwork.IsMasterClient == false) return;
        _controller.Init();
        _master.Init();
    }

    public IReadOnlyDictionary<UnitClass, int> CountByUnitClass => _enemyPlayer._countByUnitClass;
    public IReadOnlyDictionary<UnitFlags, int> UnitCountByFlag => _count._countByFlag;
    public int CurrentUnitCount => _count._currentCount;
    public int EnemyPlayerHasCount => _enemyPlayer.EnemyPlayerHasUnitAllCount;

    public RPCAction<bool, UnitFlags> OnTryCombine = new RPCAction<bool, UnitFlags>();

    public event Action<int> OnUnitCountChanged = null;
    void Rasie_OnUnitCountChanged(int count) => OnUnitCountChanged?.Invoke(count);
    
    public event Action<UnitFlags, int> OnUnitFlagCountChanged = null;
    void Rasie_OnUnitFlagCountChanged(UnitFlags flag, int count) => OnUnitFlagCountChanged?.Invoke(flag, count);
    

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

    public bool HasUnit(UnitFlags flag, int needCount = 1) => UnitCountByFlag[flag] >= needCount;

    class MasterDataManager
    {
        public RPCAction<UnitFlags, int> OnUnitCountChanged = new RPCAction<UnitFlags, int>();
        RPCData<Dictionary<UnitFlags, List<Multi_TeamSoldier>>> _unitListByFlag = new RPCData<Dictionary<UnitFlags, List<Multi_TeamSoldier>>>();

        List<Multi_TeamSoldier> GetUnitList(Multi_TeamSoldier unit) => GetUnitList(unit.GetComponent<RPCable>().UsingId, unit.UnitFlags);
        public List<Multi_TeamSoldier> GetUnitList(int id, UnitFlags flag) => _unitListByFlag.Get(id)[flag];
        public List<Multi_TeamSoldier> GetUnitList(int id) => _currentAllUnitsById.Get(id);

        public RPCAction<int> OnAllUnitCountChanged = new RPCAction<int>();
        RPCData<List<Multi_TeamSoldier>> _currentAllUnitsById = new RPCData<List<Multi_TeamSoldier>>();

        public RPCAction<bool, UnitClass> OnUnitClassCountChanged = new RPCAction<bool, UnitClass>();

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
            
            // 적 데이터도 알아야 되니까 반대로도 한 번 쏴줌 (이러거면 그냥 이벤트 하지?)
            OnUnitClassCountChanged?.RaiseEvent((id == 0) ? 1 : 0, isAdd, unit.unitClass);
        }
    }

    class EnemyPlayerDataManager
    {
        public Dictionary<UnitClass, int> _countByUnitClass = new Dictionary<UnitClass, int>();
        public int EnemyPlayerHasUnitAllCount;

        public void Init()
        {
            foreach (UnitClass _class in Enum.GetValues(typeof(UnitClass)))
                _countByUnitClass.Add(_class, 0);

            Instance._master.OnUnitClassCountChanged += SetCount;
        }

        void SetCount(bool isAdd, UnitClass unitClass)
        {
            if(isAdd)
                _countByUnitClass[unitClass]++;
            else
                _countByUnitClass[unitClass]--;

            // 리팩터링 존나 필요
            int count = 0;
            foreach (var item in _countByUnitClass)
                count += item.Value;
            EnemyPlayerHasUnitAllCount = count;
        }
    }

    class UnitContorller
    {
        public void Init()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Multi_SpawnManagers.BossEnemy.OnSpawn += AllUnitTargetChagedByBoss;
                Multi_SpawnManagers.BossEnemy.OnDead += AllUnitUpdateTarget;
            }
        }

        public void UnitDead(int id, UnitFlags unitFlag, int count = 1)
        {
            if (PhotonNetwork.IsMasterClient == false) return;

            Multi_TeamSoldier[] offerings = Instance._master.GetUnitList(id, unitFlag).ToArray();
            count = Mathf.Min(count, offerings.Length);
            for (int i = 0; i < count; i++)
                offerings[i].Dead();
        }

        public void UnitWorldChanged(int id, UnitFlags flag, bool enterStroyMode)
        {
            if (Instance._master.TryGetUnit_If(id, flag, out Multi_TeamSoldier unit, (_unit) => _unit.EnterStroyWorld == enterStroyMode))
                unit.ChagneWorld();
        }

        void AllUnitTargetChagedByBoss(Multi_BossEnemy boss)
        {
            Instance._master.GetUnitList(boss.GetComponent<RPCable>().UsingId)
                .Where(x => x.EnterStroyWorld == false)
                .ToList()
                .ForEach(x => x.SetChaseSetting(boss.gameObject));
        }

        void AllUnitUpdateTarget(Multi_BossEnemy boss) => AllUnitUpdateTarget(boss.GetComponent<RPCable>().UsingId);
        void AllUnitUpdateTarget(int id) => Instance._master.GetUnitList(id).ForEach(x => x.UpdateTarget());
    }

    class UnitCountManager
    {
        public int _currentCount = 0;
        public Dictionary<UnitFlags, int> _countByFlag = new Dictionary<UnitFlags, int>(); // 모든 플레이어가 이벤트로 받아서 각자 카운트 관리

        public void Init()
        {
            foreach (var data in Multi_SpawnManagers.NormalUnit.AllUnitDatas)
            {
                foreach (Multi_TeamSoldier unit in data.gos.Select(x => x.GetComponent<Multi_TeamSoldier>()))
                    _countByFlag.Add(new UnitFlags(unit.unitColor, unit.unitClass), 0);
            }

            Instance._master.OnAllUnitCountChanged += Riase_OnUnitCountChanged;
            Instance._master.OnUnitCountChanged += Riase_OnUnitCountChanged;
        }

        void Riase_OnUnitCountChanged(int count)
        {
            _currentCount = count;
            Instance.Rasie_OnUnitCountChanged(count);
        }

        void Riase_OnUnitCountChanged(UnitFlags flag, int count)
        {
            _countByFlag[flag] = count;
            Instance.Rasie_OnUnitFlagCountChanged(flag, count);
        }
    }

    class CombineSystem
    {
        public bool CheckCombineable(UnitFlags flag)
            => Multi_Managers.Data.CombineConditionByUnitFalg[flag].NeedCountByFlag.All(x => Instance.HasUnit(x.Key, x.Value));

        public void TryCombine(UnitFlags flag, int id, bool isSuccess)
        {
            Debug.Assert(PhotonNetwork.IsMasterClient, "마스터가 아닌데 유닛 조합을 하려고 함");

            if (isSuccess)
                Combine(flag, id);
            else
                Instance.OnTryCombine?.RaiseEvent(id, false, flag);
        }

        void Combine(UnitFlags flag, int id)
        {
            if (PhotonNetwork.IsMasterClient == false) return;

            SacrificedUnits_ForCombine(Multi_Managers.Data.CombineConditionByUnitFalg[flag]);
            Multi_SpawnManagers.NormalUnit.Spawn(flag, id);

            Instance.OnTryCombine?.RaiseEvent(id, true, flag);

            void SacrificedUnits_ForCombine(CombineCondition condition)
                => condition.NeedCountByFlag.ToList().ForEach(x => SacrificedUnit_ForCombine(x));

            void SacrificedUnit_ForCombine(KeyValuePair<UnitFlags, int> flagCountPair)
            {
                for (int i = 0; i < flagCountPair.Value; i++)
                {
                    Instance._controller.UnitDead(id, flagCountPair.Key);
                    if (flagCountPair.Key == new UnitFlags(2, 0))
                        Multi_GameManager.instance.AddGold(1, id);
                }
            }
        }
    }

    class UnitStatChanger
    {
        public void UnitStatChange(int typeNum, UnitFlags flag, int value, int id)
        {
            switch (typeNum)
            {
                case 0: ChangeDamage(flag, value, id); break;
                case 1: ChangeBossDamage(flag, value, id); break;
            }
        }

        void ChangeDamage(UnitFlags flag, int value, int id)
        {
            foreach (var unit in Instance._master.GetUnitList(id, flag))
                unit.Damage = value;
        }

        void ChangeBossDamage(UnitFlags flag, float value, int id)
        {
            foreach (var unit in Instance._master.GetUnitList(id, flag))
                unit.BossDamage += Mathf.FloorToInt(unit.BossDamage * (value - 1));
        }
    }
}

public enum UnitStatType
{
    Damage,
    BossDamage,
}