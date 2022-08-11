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
            }
            return instance;
        }
    }

    void Awake()
    {
        unitListDictById.Clear();
        _currentAllUnitsById.Clear();
        if (PhotonNetwork.IsMasterClient)
        {
            unitListDictById.Add(0, new Dictionary<UnitFlags, List<Multi_TeamSoldier>>());
            unitListDictById.Add(1, new Dictionary<UnitFlags, List<Multi_TeamSoldier>>());

            _currentAllUnitsById.Add(0, new List<Multi_TeamSoldier>());
            _currentAllUnitsById.Add(1, new List<Multi_TeamSoldier>());
        }
    }

    private void Start()
    {
        OnAllUnitCountChanged += count => _unitCount = count;

        if (PhotonNetwork.IsMasterClient)
        {
            Multi_SpawnManagers.NormalUnit.OnSpawn += AddUnit;
            Multi_SpawnManagers.NormalUnit.OnDead += RemoveUnit;

            Multi_SpawnManagers.BossEnemy.OnSpawn += AllUnitTargetChagedByBoss;
            Multi_SpawnManagers.BossEnemy.OnDead += AllUnitUpdateTarget;

            OnCombineTry += (isSuccess, flag) => print($"컴바인 시도 결과 : {isSuccess} \n 색깔 : {flag.ColorNumber}, 클래스 : {flag.ClassNumber}");
        }

        SetUnitFlagsDic();
        OnUnitCountChanged += UpdateCount;

        void SetUnitFlagsDic()
        {
            foreach (var data in Multi_SpawnManagers.NormalUnit.AllUnitDatas)
            {
                foreach (Multi_TeamSoldier unit in data.gos.Select(x => x.GetComponent<Multi_TeamSoldier>()))
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        unitListDictById[0].Add(new UnitFlags(unit.unitColor, unit.unitClass), new List<Multi_TeamSoldier>());
                        unitListDictById[1].Add(new UnitFlags(unit.unitColor, unit.unitClass), new List<Multi_TeamSoldier>());
                    }
                    _countByFlag.Add(new UnitFlags(unit.unitColor, unit.unitClass), 0);
                }
            }
        }
    }

    // 유닛 딕셔너리
    Dictionary<int, Dictionary<UnitFlags, List<Multi_TeamSoldier>>> unitListDictById = new Dictionary<int, Dictionary<UnitFlags, List<Multi_TeamSoldier>>>();
    List<Multi_TeamSoldier> GetUnitList(Multi_TeamSoldier unit) => GetUnitList(unit.GetComponent<RPCable>().UsingId, unit.UnitFlags);
    List<Multi_TeamSoldier> GetUnitList(int id, UnitFlags flags) => unitListDictById[id][flags];
    int GetUnitListCount(int id, UnitFlags flags) => GetUnitList(id, flags).Count;

    Dictionary<UnitFlags, int> _countByFlag = new Dictionary<UnitFlags, int>();
    void UpdateCount(UnitFlags flag, int count) => _countByFlag[flag] = count;

    public void UnitWorldChanged_RPC(int id, UnitFlags flag) 
        => photonView.RPC("UnitWorldChanged", RpcTarget.MasterClient, id, flag, Multi_GameManager.instance.playerEnterStoryMode);

    [PunRPC]
    void UnitWorldChanged(int id, UnitFlags flag, bool enterStroyMode)
    {
        if (Instance.TryGetUnit(id, flag, out Multi_TeamSoldier unit, (_unit) => _unit.EnterStroyWorld == enterStroyMode))
            unit.ChagneWorld();
    }


    public bool HasUnit(UnitFlags flag) => _countByFlag[flag] > 0;

    bool TryGetUnit(int id, UnitFlags flag, out Multi_TeamSoldier unit, Func<Multi_TeamSoldier, bool> condition = null)
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

    bool TryGetUnit(int id, UnitFlags flag, out Multi_TeamSoldier unit, bool enterStroyMode)
    {
        foreach (Multi_TeamSoldier loopUnit in GetUnitList(id, flag))
        {
            if (loopUnit.EnterStroyWorld == enterStroyMode)
            {
                unit = loopUnit;
                return true;
            }
        }

        unit = null;
        return false;
    }

    public RPCAction<UnitFlags, int> OnUnitCountChanged = new RPCAction<UnitFlags, int>();
    public void Raise_UnitCountChanged(UnitFlags flag) => photonView.RPC("Raise_UnitCountChanged", RpcTarget.MasterClient, Multi_Data.instance.Id, flag);
    [PunRPC]
    void Raise_UnitCountChanged(int id, UnitFlags flag) => OnUnitCountChanged.RaiseEvent(id, flag, GetUnitListCount(id, flag));

    // 유닛 전체 리스트
    Dictionary<int, List<Multi_TeamSoldier>> _currentAllUnitsById = new Dictionary<int, List<Multi_TeamSoldier>>();
    List<Multi_TeamSoldier> GetCurrentUnitList(Multi_TeamSoldier unit) => _currentAllUnitsById[unit.GetComponent<RPCable>().UsingId];

    [SerializeField] int _unitCount;
    public int UnitCount => _unitCount;

    public RPCAction<int> OnAllUnitCountChanged = new RPCAction<int>();
    void Raise_AllUnitCountChanged(int id) => OnAllUnitCountChanged.RaiseEvent(id, _currentAllUnitsById[id].Count);

    // 유닛 조합
    public RPCAction<bool, UnitFlags> OnCombineTry = new RPCAction<bool, UnitFlags>();

    public void Combine_RPC(UnitFlags flag)
        => photonView.RPC("Combine", RpcTarget.MasterClient, flag.ColorNumber, flag.ClassNumber, Multi_Data.instance.Id);

    [PunRPC]
    void Combine(int colorNumber, int classNumber, int id)
    {
        if (CheckCombineable(Multi_Managers.Data.CombineConditionByUnitFalg[new UnitFlags(colorNumber, classNumber)], id))
        {
            SacrificedUnit_ForCombine(Multi_Managers.Data.CombineConditionByUnitFalg[new UnitFlags(colorNumber, classNumber)], id);
            Multi_SpawnManagers.NormalUnit.Spawn(new UnitFlags(colorNumber, classNumber), id);

            OnCombineTry?.RaiseEvent(id, true, new UnitFlags(colorNumber, classNumber));
        }
        else
            OnCombineTry?.RaiseEvent(id, false, new UnitFlags(colorNumber, classNumber));
    }

    bool CheckCombineable(CombineCondition conditions, int id)
        => conditions.UnitFlagsCountPair.All(x => unitListDictById[id].ContainsKey(x.Key) && GetUnitList(id, x.Key).Count >= x.Value);

    void SacrificedUnit_ForCombine(CombineCondition condition, int id)
            => condition.UnitFlagsCountPair.ToList().ForEach(x => UnitDead(id, x.Key, x.Value));

    public void UnitDead_RPC(int id, UnitFlags unitFlag, int count = 1) => photonView.RPC("UnitDead", RpcTarget.MasterClient, id, unitFlag, count);
    [PunRPC]
    void UnitDead(int id, UnitFlags unitFlag, int count)
    {
        Multi_TeamSoldier[] offerings = GetUnitList(id, unitFlag).ToArray();
        for (int i = 0; i < count; i++)
            offerings[i].Dead();
    }

    #region Only Callback
    void AddUnit(Multi_TeamSoldier unit)
    {
        GetUnitList(unit).Add(unit);
        GetCurrentUnitList(unit).Add(unit);

        Raise_UnitCountChanged(unit.GetComponent<RPCable>().UsingId, unit.UnitFlags);
        Raise_AllUnitCountChanged(unit.GetComponent<RPCable>().UsingId);
    }

    void RemoveUnit(Multi_TeamSoldier unit)
    {
        GetUnitList(unit).Remove(unit);
        GetCurrentUnitList(unit).Remove(unit);

        Raise_UnitCountChanged(unit.GetComponent<RPCable>().UsingId, unit.UnitFlags);
        Raise_AllUnitCountChanged(unit.GetComponent<RPCable>().UsingId);
    }

    void AllUnitTargetChagedByBoss(Multi_BossEnemy boss)
    {
        _currentAllUnitsById[boss.GetComponent<RPCable>().UsingId]
            .Where(x => x.EnterStroyWorld == false)
            .ToList()
            .ForEach(x => x.SetChaseSetting(boss.gameObject));
    }

    void AllUnitUpdateTarget(Multi_BossEnemy boss) => AllUnitUpdateTarget(boss.GetComponent<RPCable>().UsingId);

    #endregion

    void AllUnitUpdateTarget(int id) => _currentAllUnitsById[id].ForEach(x => x.UpdateTarget());

    [SerializeField] List<int> test = new List<int>();
    void Update()
    {
        test = _countByFlag.Values.ToList();
    }
}
