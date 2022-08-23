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

    public static CombineSystem Combine => Instance._combine;
    public static UnitCountManager Count => Instance._count;
    public static UnitContorller Controller => Instance._controller;
    public static EnemyPlayerDataManager EnemyPlayer => Instance._enemyPlayer;
    public static UnitStatChanger Stat => Instance._stat;

    void Init()
    {
        _count.Init();
        _enemyPlayer.Init();
        _stat.Init();

        if (PhotonNetwork.IsMasterClient == false) return;
        _controller.Init();
        _master.Init();
    }

    #region PunRPC functions
    [PunRPC]
    void UnitWorldChanged(int id, UnitFlags flag, bool enterStroyMode) => Controller.UnitWorldChanged(id, flag, enterStroyMode);

    [PunRPC] // CombineSystem에서 사용
    void TryCombine(UnitFlags flag, int id, bool isSuccess) => Combine.TryCombine(flag, id, isSuccess);

    [PunRPC] // Controller에서 사용
    void UnitDead(int id, UnitFlags unitFlag, int count) => Controller.UnitDead(id, unitFlag, count);
    #endregion


    public class MasterDataManager
    {
        public RPCAction<UnitFlags, int> OnUnitCountChanged = new RPCAction<UnitFlags, int>();
        RPCData<Dictionary<UnitFlags, List<Multi_TeamSoldier>>> _unitListByFlag = new RPCData<Dictionary<UnitFlags, List<Multi_TeamSoldier>>>();

        List<Multi_TeamSoldier> GetUnitList(Multi_TeamSoldier unit)
        {
            //Debug.Log($"ID : {unit.GetComponent<RPCable>().UsingId}");
            //Debug.Log($"{unit.unitColor} : {unit.unitClass}");
            //Debug.Log($"크기 : {_unitListByFlag.Count}");
            return GetUnitList(unit.GetComponent<RPCable>().UsingId, unit.UnitFlags);
        }
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
            
            // 적 데이터 덮어쓰는 용
            OnUnitClassCountChanged?.RaiseEvent((id == 0) ? 1 : 0, isAdd, unit.unitClass);
        }
    }

    public class EnemyPlayerDataManager
    {
        Dictionary<UnitClass, int> _countByUnitClass = new Dictionary<UnitClass, int>();
        public IReadOnlyDictionary<UnitClass, int> CountByUnitClass => _countByUnitClass;

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
            Debug.Log(_countByUnitClass[unitClass]);
        }
    }

    public class UnitContorller
    {
        public void Init()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Multi_SpawnManagers.BossEnemy.OnSpawn += AllUnitTargetChagedByBoss;
                Multi_SpawnManagers.BossEnemy.OnDead += AllUnitUpdateTarget;
            }
        }

        public void UnitDead_RPC(int id, UnitFlags unitFlag, int count = 1) => Instance.photonView.RPC("UnitDead", RpcTarget.MasterClient, id, unitFlag, count);
        
        public void UnitDead(int id, UnitFlags unitFlag, int count)
        {
            if (PhotonNetwork.IsMasterClient == false) return;

            Multi_TeamSoldier[] offerings = Instance._master.GetUnitList(id, unitFlag).ToArray();
            count = Mathf.Min(count, offerings.Length);
            for (int i = 0; i < count; i++)
                offerings[i].Dead();
        }

        public void UnitWorldChanged_RPC(int id, UnitFlags flag)
            => Instance.photonView.RPC("UnitWorldChanged", RpcTarget.MasterClient, id, flag, Multi_Managers.Camera.IsLookEnemyTower);

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

    public class UnitCountManager
    {
        int _currentCount = 0;
        public int CurrentUnitCount => _currentCount;

        Dictionary<UnitFlags, int> _countByFlag = new Dictionary<UnitFlags, int>(); // 모든 플레이어가 이벤트로 받아서 각자 카운트 관리
        public IReadOnlyDictionary<UnitFlags, int> UnitCountByFlag => _countByFlag;
        public bool HasUnit(UnitFlags flag, int needCount = 1) => UnitCountByFlag[flag] >= needCount;

        public event Action<int> OnUnitCountChanged = null;
        public event Action<UnitFlags, int> OnUnitFlagCountChanged = null;

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
            OnUnitCountChanged?.Invoke(count);
        }

        void Riase_OnUnitCountChanged(UnitFlags flag, int count)
        {
            _countByFlag[flag] = count;
            OnUnitFlagCountChanged?.Invoke(flag, count);
        }
    }

    public class CombineSystem
    {
        public RPCAction<bool, UnitFlags> OnTryCombine = new RPCAction<bool, UnitFlags>();

        public bool TryCombine_RPC(UnitFlags flag)
        {
            bool result = CheckCombineable(flag);
            Instance.photonView.RPC("TryCombine", RpcTarget.MasterClient, flag, Multi_Data.instance.Id, result);
            return result;
        }

        bool CheckCombineable(UnitFlags flag)
        {
            Multi_Managers.Data.CombineConditionByUnitFalg[flag].NeedCountByFlag.ToList()
                .ForEach(x => Debug.Log($"{x.Key.UnitClass}, {x.Key.UnitColor} : {x.Value}"));
            return Multi_Managers.Data.CombineConditionByUnitFalg[flag].NeedCountByFlag.All(x => Instance._count.HasUnit(x.Key, x.Value));
        }

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

            SacrificedUnit_ForCombine(Multi_Managers.Data.CombineConditionByUnitFalg[flag]);
            Multi_SpawnManagers.NormalUnit.Spawn(flag, id);

            OnTryCombine?.RaiseEvent(id, true, flag);


            void SacrificedUnit_ForCombine(CombineCondition condition)
                    => condition.NeedCountByFlag.ToList().ForEach(x => Instance._controller.UnitDead(id, x.Key, x.Value));
        }
    }

    public class UnitStatChanger
    {
        int _id;

        public void Init()
        {
            _id = Multi_Data.instance.Id;
        }

        public void UnitStatChange(UnitStatType type, UnitFlags flag, float value)
        {
            switch (type)
            {
                case UnitStatType.Damage: ChangeDamage(flag, value); break;
                case UnitStatType.BossDamage: ChangeBossDamage(flag, value); break;
            }
        }

        void ChangeDamage(UnitFlags flag, float value)
        {
            foreach (var unit in Instance._master.GetUnitList(_id, flag))
                unit.Damage = Mathf.FloorToInt(unit.Damage * value);
        }

        void ChangeBossDamage(UnitFlags flag, float value)
        {
            foreach (var unit in Instance._master.GetUnitList(_id, flag))
                unit.BossDamage = Mathf.FloorToInt(unit.BossDamage * value);
        }
    }
}

public enum UnitStatType
{
    Damage,
    BossDamage,
}