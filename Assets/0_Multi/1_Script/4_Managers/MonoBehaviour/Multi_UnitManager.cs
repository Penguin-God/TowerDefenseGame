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
    UnitContorller _contorller = new UnitContorller();
    MasterDataManager _master = new MasterDataManager();

    public static CombineSystem Combine => Instance._combine;
    public static UnitCountManager Count => Instance._count;
    public static UnitContorller Contorller => Instance._contorller;

    void Init()
    {
        _count.Init();

        if (PhotonNetwork.IsMasterClient == false) return;
        _contorller.Init();
        _master.Init();
    }

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            //Multi_SpawnManagers.BossEnemy.OnSpawn += AllUnitTargetChagedByBoss;
            //Multi_SpawnManagers.BossEnemy.OnDead += AllUnitUpdateTarget;

            Combine.OnTryCombine += (isSuccess, flag) => print($"컴바인 시도 결과 : {isSuccess} \n 색깔 : {flag.ColorNumber}, 클래스 : {flag.ClassNumber}");
        }
    }


    public void UnitWorldChanged_RPC(int id, UnitFlags flag) 
        => photonView.RPC("UnitWorldChanged", RpcTarget.MasterClient, id, flag, Multi_Managers.Camera.IsLookEnemyTower);

    [PunRPC]
    void UnitWorldChanged(int id, UnitFlags flag, bool enterStroyMode)
    {
        if (Instance.TryGetUnit(id, flag, out Multi_TeamSoldier unit, (_unit) => _unit.EnterStroyWorld == enterStroyMode))
            unit.ChagneWorld();
    }

    public bool HasUnit(UnitFlags flag, int needCount = 1) => _count.UnitCountByFlag[flag] >= needCount;

    bool TryGetUnit(int id, UnitFlags flag, out Multi_TeamSoldier unit, Func<Multi_TeamSoldier, bool> condition = null)
    {
        foreach (Multi_TeamSoldier loopUnit in _master.GetUnitList(id, flag))
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

    [PunRPC] // CombineSystem에서 사용
    void UnitCombine(UnitFlags flag, int id) => Combine.Combine(flag, id);


    [PunRPC] // Controller에서 사용
    void UnitDead(int id, UnitFlags unitFlag, int count) => _contorller.UnitDead(id, unitFlag, count);

    //void AllUnitTargetChagedByBoss(Multi_BossEnemy boss)
    //{
    //    _master.GetUnitList(boss.GetComponent<RPCable>().UsingId)
    //        .Where(x => x.EnterStroyWorld == false)
    //        .ToList()
    //        .ForEach(x => x.SetChaseSetting(boss.gameObject));
    //}

    //void AllUnitUpdateTarget(Multi_BossEnemy boss) => AllUnitUpdateTarget(boss.GetComponent<RPCable>().UsingId);
    //void AllUnitUpdateTarget(int id) => _master.GetUnitList(id).ForEach(x => x.UpdateTarget());

    [SerializeField] List<int> test = new List<int>();
    void Update()
    {
        test = Count.UnitCountByFlag.Values.ToList();
    }

    public class MasterDataManager
    {
        public RPCAction<UnitFlags, int> OnUnitCountChanged = new RPCAction<UnitFlags, int>();

        RPCData<Dictionary<UnitFlags, List<Multi_TeamSoldier>>> _unitListByFlag = new RPCData<Dictionary<UnitFlags, List<Multi_TeamSoldier>>>();
        List<Multi_TeamSoldier> GetUnitList(Multi_TeamSoldier unit)
        {
            Debug.Log($"ID : {unit.GetComponent<RPCable>().UsingId}");
            Debug.Log($"{unit.unitColor} : {unit.unitClass}");
            Debug.Log($"크기 : {_unitListByFlag.Count}");
            return GetUnitList(unit.GetComponent<RPCable>().UsingId, unit.UnitFlags);
        }
        public List<Multi_TeamSoldier> GetUnitList(int id, UnitFlags flag) => _unitListByFlag.Get(id)[flag];
        public List<Multi_TeamSoldier> GetUnitList(int id) => _currentAllUnitsById.Get(id);

        public RPCAction<int> OnAllUnitCountChanged = new RPCAction<int>();
        RPCData<List<Multi_TeamSoldier>> _currentAllUnitsById = new RPCData<List<Multi_TeamSoldier>>();

        public void Init()
        {
            foreach (var data in Multi_SpawnManagers.NormalUnit.AllUnitDatas)
            {
                foreach (Multi_TeamSoldier unit in data.gos.Select(x => x.GetComponent<Multi_TeamSoldier>()))
                {
                    _unitListByFlag.Get(0).Add(new UnitFlags(unit.unitColor, unit.unitClass), new List<Multi_TeamSoldier>());
                    _unitListByFlag.Get(1).Add(new UnitFlags(unit.unitColor, unit.unitClass), new List<Multi_TeamSoldier>());

                    Debug.Log($"{unit.unitColor} : {unit.unitClass}");
                }
            }

            Multi_SpawnManagers.NormalUnit.OnSpawn += AddUnit;
            Multi_SpawnManagers.NormalUnit.OnDead += RemoveUnit;
        }

        void AddUnit(Multi_TeamSoldier unit)
        {
            int id = unit.GetComponent<RPCable>().UsingId;
            GetUnitList(unit).Add(unit);
            _currentAllUnitsById.Get(id).Add(unit);
            RaiseEvents(unit);
        }

        void RemoveUnit(Multi_TeamSoldier unit)
        {
            int id = unit.GetComponent<RPCable>().UsingId;
            GetUnitList(unit).Remove(unit);
            _currentAllUnitsById.Get(id).Remove(unit);
            RaiseEvents(unit);
        }

        void RaiseEvents(Multi_TeamSoldier unit)
        {
            int id = unit.GetComponent<RPCable>().UsingId;
            OnUnitCountChanged?.RaiseEvent(id, unit.UnitFlags, GetUnitList(unit).Count);
            OnAllUnitCountChanged?.RaiseEvent(id, _currentAllUnitsById.Get(id).Count);
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

        public void TryCombine_RPC(UnitFlags flag, int id = 0)
        {
            if (CheckCombineable(Multi_Managers.Data.CombineConditionByUnitFalg[flag]))
                Instance.photonView.RPC("UnitCombine", RpcTarget.MasterClient, flag, Multi_Data.instance.Id);
            else
                OnTryCombine?.RaiseEvent(id, false, flag);
        }

        bool CheckCombineable(CombineCondition conditions)
            => conditions.UnitFlagsCountPair.All(x => Instance._count.UnitCountByFlag[x.Key] >= x.Value);

        public void Combine(UnitFlags flag, int id)
        {
            if (PhotonNetwork.IsMasterClient == false) return;

            SacrificedUnit_ForCombine(Multi_Managers.Data.CombineConditionByUnitFalg[flag]);
            Multi_SpawnManagers.NormalUnit.Spawn(flag, id);

            OnTryCombine?.RaiseEvent(id, true, flag);


            void SacrificedUnit_ForCombine(CombineCondition condition)
                    => condition.UnitFlagsCountPair.ToList().ForEach(x => Instance._contorller.UnitDead(id, x.Key, x.Value));
        }
    }
}