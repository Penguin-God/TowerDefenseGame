using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Random = UnityEngine.Random;
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
        _currentUnitsById.Clear();
        if (PhotonNetwork.IsMasterClient)
        {
            unitListDictById.Add(0, new Dictionary<UnitFlags, List<Multi_TeamSoldier>>());
            unitListDictById.Add(1, new Dictionary<UnitFlags, List<Multi_TeamSoldier>>());

            _currentUnitsById.Add(0, new List<Multi_TeamSoldier>());
            _currentUnitsById.Add(1, new List<Multi_TeamSoldier>());
        }
    }

    private void Start()
    {
        OnAllUnitCountChanged += count => _unitCount = count;

        if (PhotonNetwork.IsMasterClient)
        {
            Multi_SpawnManagers.NormalUnit.OnSpawn += AddUnit;
            Multi_SpawnManagers.NormalUnit.OnDead += RemoveUnit;

            SetUnitFlagsDic();

            OnCombineTry += (isSuccess, flag) => print($"컴바인 시도 결과 : {isSuccess} \n 색깔 : {flag.ColorNumber}, 클래스 : {flag.ClassNumber}");
        }

        void SetUnitFlagsDic()
        {
            foreach (var data in Multi_SpawnManagers.NormalUnit.AllUnitDatas)
            {
                foreach (Multi_TeamSoldier unit in data.gos.Select(x => x.GetComponent<Multi_TeamSoldier>()))
                {
                    if (unit == null) continue; // TODO : 하얀 유닛 때문에 임시로 넘김
                    unitListDictById[0].Add(new UnitFlags(unit.unitColor, unit.unitClass), new List<Multi_TeamSoldier>());
                    unitListDictById[1].Add(new UnitFlags(unit.unitColor, unit.unitClass), new List<Multi_TeamSoldier>());
                }
            }
        }
    }

    // 유닛 딕셔너리
    Dictionary<int, Dictionary<UnitFlags, List<Multi_TeamSoldier>>> unitListDictById = new Dictionary<int, Dictionary<UnitFlags, List<Multi_TeamSoldier>>>();
    List<Multi_TeamSoldier> GetUnitList(Multi_TeamSoldier unit) => GetUnitList(unit.GetComponent<RPCable>().UsingId, unit.UnitFlags);
    List<Multi_TeamSoldier> GetUnitList(int id, UnitFlags flags) => unitListDictById[id][flags];
    int GetUnitListCount(int id, UnitFlags flags) => GetUnitList(id, flags).Count;

    public RPCAction<UnitFlags, int> OnUnitCountChanged = new RPCAction<UnitFlags, int>();
    public void Raise_UnitCountChanged(UnitFlags flag) => photonView.RPC("Raise_UnitCountChanged", RpcTarget.MasterClient, Multi_Data.instance.Id, flag);
    [PunRPC]
    void Raise_UnitCountChanged(int id, UnitFlags flag) => OnUnitCountChanged.RaiseEvent(id, flag, GetUnitListCount(id, flag));

    // 유닛 전체 리스트
    Dictionary<int, List<Multi_TeamSoldier>> _currentUnitsById = new Dictionary<int, List<Multi_TeamSoldier>>();
    List<Multi_TeamSoldier> GetCurrentUnitList(Multi_TeamSoldier unit) => _currentUnitsById[unit.GetComponent<RPCable>().UsingId];

    [SerializeField] int _unitCount;
    public int UnitCount => _unitCount;

    public RPCAction<int> OnAllUnitCountChanged = new RPCAction<int>();
    void Raise_AllUnitCountChanged(int id) => OnAllUnitCountChanged.RaiseEvent(id, _currentUnitsById[id].Count);

    
    // 유닛 조합
    public RPCAction<bool, UnitFlags> OnCombineTry = new RPCAction<bool, UnitFlags>();

    public void Combine_RPC(CombineData data)
        => photonView.RPC("Combine", RpcTarget.MasterClient, data.UnitFlags.ColorNumber, data.UnitFlags.ClassNumber, Multi_Data.instance.Id);

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
            => condition.UnitFlagsCountPair.ToList().ForEach(x => SacrificedUnit_ForCombine(id, x.Key, x.Value));
    void SacrificedUnit_ForCombine(int id, UnitFlags unitFlag, int count)
    {
        Multi_TeamSoldier[] offerings = GetUnitList(id, unitFlag).ToArray();
        for (int i = 0; i < count; i++)
            offerings[i].Dead();
    }


    // 리스트 갱신
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


    // 아래는 쭉 리팩터링 전 코드들
    [SerializeField] private GameObject[] tp_Effects;
    int current_TPEffectIndex;
    Vector3 effectPoolPosition = new Vector3(1000, 1000, 1000);
    public void ShowTpEffect(Transform tpUnit)
    {
        StartCoroutine(ShowTpEffect_Coroutine(tpUnit));
    }

    IEnumerator ShowTpEffect_Coroutine(Transform tpUnit) // tp 이펙트 풀링
    {
        tp_Effects[current_TPEffectIndex].transform.position = tpUnit.position + Vector3.up;
        tp_Effects[current_TPEffectIndex].SetActive(true);
        yield return new WaitForSeconds(0.25f);
        tp_Effects[current_TPEffectIndex].SetActive(false);
        tp_Effects[current_TPEffectIndex].transform.position = effectPoolPosition;
        current_TPEffectIndex++;
        if (current_TPEffectIndex >= tp_Effects.Length) current_TPEffectIndex = 0;
    }
}
