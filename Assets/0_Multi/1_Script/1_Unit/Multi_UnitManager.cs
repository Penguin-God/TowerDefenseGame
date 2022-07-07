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

    public event Action<int> OnCurrentUnitChanged = null;
    public event Action<UnitFlags, int> OnUnitFlagDictChanged = null;
    //public event Action<KeyValuePair<UnitFlags, int>> OnUnitCombined = null;

    void Awake()
    {
        unitListDictById.Clear();
        if (PhotonNetwork.IsMasterClient)
        {
            unitListDictById.Add(0, new Dictionary<UnitFlags, List<Multi_TeamSoldier>>());
            unitListDictById.Add(1, new Dictionary<UnitFlags, List<Multi_TeamSoldier>>());
        }
        unitDB = GetComponent<Multi_UnitDataBase>();
    }

    private void Start()
    {
        Multi_SpawnManagers.NormalUnit.OnSpawn += AddList;
        Multi_SpawnManagers.NormalUnit.OnDead += RemoveList;

        SetUnitFlagsDic();
    }

    private List<Multi_TeamSoldier> _currentUnits = new List<Multi_TeamSoldier>();
    IReadOnlyList<Multi_TeamSoldier> CurrentUnits => _currentUnits;
    public int CurrentUnitCount => _currentUnits.Count;

    public void AddList(Multi_TeamSoldier unit)
    {
        GetUnitList(unit).Add(unit);

        _currentUnits.Add(unit);
        _unitListByUnitFlags[unit.UnitFlags].Add(unit);
        Raise_UnitCountChangedEvents(unit);
        print($"{unit.name} : {GetUnitList(unit).Count}마리 있음");
    }

    public void RemoveList(Multi_TeamSoldier unit)
    {
        GetUnitList(unit).Remove(unit);

        _currentUnits.Remove(unit);
        _unitListByUnitFlags[unit.UnitFlags].Remove(unit);
        Raise_UnitCountChangedEvents(unit);
    }

    void Raise_UnitCountChangedEvents(Multi_TeamSoldier unit)
    {
        OnCurrentUnitChanged?.Invoke(_currentUnits.Count);
        OnUnitFlagDictChanged?.Invoke(unit.UnitFlags, GetUnitFlagCount(unit.UnitFlags));
    }


    Dictionary<int, Dictionary<UnitFlags, List<Multi_TeamSoldier>>> unitListDictById = new Dictionary<int, Dictionary<UnitFlags, List<Multi_TeamSoldier>>>();
    List<Multi_TeamSoldier> GetUnitList(Multi_TeamSoldier unit) => GetUnitList(unit.GetComponent<RPCable>().UsingId, unit.UnitFlags);
    List<Multi_TeamSoldier> GetUnitList(int id, UnitFlags flags) => unitListDictById[id][flags];


    private Dictionary<UnitFlags, List<Multi_TeamSoldier>> _unitListByUnitFlags = new Dictionary<UnitFlags, List<Multi_TeamSoldier>>();
    public IReadOnlyDictionary<UnitFlags, List<Multi_TeamSoldier>> UnitListByUnitFlags => _unitListByUnitFlags;
    public int GetUnitFlagCount(UnitFlags unitFlag)
    {
        if (UnitListByUnitFlags.TryGetValue(unitFlag, out List<Multi_TeamSoldier> units))
            return units.Count;
        else
            return 0;
    }
    void SetUnitFlagsDic()
    {
        _unitListByUnitFlags.Clear();
        foreach (var data in Multi_SpawnManagers.NormalUnit.AllUnitDatas)
        {
            foreach (Multi_TeamSoldier unit in data.gos.Select(x => x.GetComponent<Multi_TeamSoldier>()))
            {
                if(unit == null) continue; // TODO : 하얀 유닛 때문에 임시로 넘김
                // 이미 있으면 로그 띄우고 넘김
                if(_unitListByUnitFlags.ContainsKey(new UnitFlags(unit.unitColor, unit.unitClass)))
                {
                    print($"{unit.unitColor} : {unit.unitClass}");
                    print(unit.gameObject.name);
                    continue;
                }

                _unitListByUnitFlags.Add(new UnitFlags(unit.unitColor, unit.unitClass), new List<Multi_TeamSoldier>());

                unitListDictById[0].Add(new UnitFlags(unit.unitColor, unit.unitClass), new List<Multi_TeamSoldier>());
                unitListDictById[1].Add(new UnitFlags(unit.unitColor, unit.unitClass), new List<Multi_TeamSoldier>());
            }
        }
    }

    public void Combine_RPC(CombineData data)
    {
        photonView.RPC("Combine", RpcTarget.MasterClient, data.UnitFlags.ColorNumber, data.UnitFlags.ClassNumber);
    }

    public void Combine(CombineData data)
    {
        print($"컴바인 시도 : 색깔 : {data.UnitFlags.ColorNumber}, 클래스 : {data.UnitFlags.ClassNumber}");
        if (CheckCombineable(data.Condition))
        {
            SacrificedUnit_ForCombine(data.Condition);
            Multi_SpawnManagers.NormalUnit.Spawn(data.UnitFlags);
            //OnUnitCombined?.Invoke(new KeyValuePair<UnitFlags, int>(data.UnitFlags, GetUnitFlagCount(data.UnitFlags)));
        }
    }

    [PunRPC]
    public void Combine(int colorNumber, int classNumber)
    {
        print($"컴바인 시도 : 색깔 : {colorNumber}, 클래스 : {classNumber}");
        if (CheckCombineable(Multi_Managers.Data.CombineConditionByUnitFalg[new UnitFlags(colorNumber, classNumber)]))
        {
            SacrificedUnit_ForCombine(Multi_Managers.Data.CombineConditionByUnitFalg[new UnitFlags(colorNumber, classNumber)]);
            Multi_SpawnManagers.NormalUnit.Spawn(new UnitFlags(colorNumber, classNumber));
            //OnUnitCombined?.Invoke(new KeyValuePair<UnitFlags, int>(data.UnitFlags, GetUnitFlagCount(data.UnitFlags)));
        }
    }

    bool CheckCombineable(CombineCondition conditions, int id)
        => conditions.UnitFlagsCountPair.All(x => unitListDictById[id].ContainsKey(x.Key) && GetUnitList(id, x.Key).Count >= x.Value);

    bool CheckCombineable(CombineCondition conditions)
        => conditions.UnitFlagsCountPair.All(x => _unitListByUnitFlags.ContainsKey(x.Key) && _unitListByUnitFlags[x.Key].Count >= x.Value);

    void SacrificedUnit_ForCombine(CombineCondition condition)
        => condition.UnitFlagsCountPair.ToList().ForEach(x => SacrificedUnit_ForCombine(x.Key, x.Value));
    void SacrificedUnit_ForCombine(UnitFlags unitFlag, int count)
    {
        Multi_TeamSoldier[] offerings = _unitListByUnitFlags[unitFlag].ToArray();
        for (int i = 0; i < count; i++)
            offerings[i].Dead();
    }

    void SacrificedUnit_ForCombine(int id, UnitFlags unitFlag, int count)
    {
        Multi_TeamSoldier[] offerings = GetUnitList(id, unitFlag).ToArray();
        for (int i = 0; i < count; i++)
            offerings[i].Dead();
    }

    //public bool FindCurrentUnit(int colorNum, int classNum, out Multi_TeamSoldier unit) => FindCurrentUnit(new UnitFlags(colorNum, classNum), out unit);
    //public bool FindCurrentUnit(UnitColor unitColor, UnitClass unitClass, out Multi_TeamSoldier unit) 
    //                            => FindCurrentUnit(new UnitFlags(unitColor, unitClass), out unit);
    //public bool FindCurrentUnit(UnitFlags unitFlags, out Multi_TeamSoldier unit)
    //{
    //    if (_unitListByUnitFlags.ContainsKey(unitFlags))
    //    {
    //        unit = _unitListByUnitFlags[unitFlags][0];
    //        return true;
    //    }
    //    else
    //    {
    //        unit = null;
    //        return false;
    //    }
    //}


    // 아래는 쭉 리팩터링 전 코드들

    Multi_UnitDataBase unitDB = null;
    public void ApplyUnitData(string _tag, Multi_TeamSoldier _team) => unitDB.ApplyUnitBaseData(_tag, _team);
    public void ApplyPassiveData(string _key, Multi_UnitPassive _passive, UnitColor _color) => unitDB.ApplyPassiveData(_key, _passive, _color);

    public GameObject[] startUnitArray;
    public void ReSpawnStartUnit()
    {
        int random = Random.Range(0, startUnitArray.Length);

        GameObject startUnit = Instantiate(startUnitArray[random],
            startUnitArray[random].transform.position, startUnitArray[random].transform.rotation);
        startUnit.SetActive(true);
    }

    [SerializeField] int maxUnit;
    public int MaxUnit => maxUnit;
    public void ExpendMaxUnit(int addUnitCount) => maxUnit += addUnitCount;

    public bool UnitOver
    {
        get
        {
            if (CurrentUnitCount >= maxUnit)
            {
                UnitOverGuide();
                return true;
            }

            return false;
        }
    }

    [SerializeField] GameObject unitOverGuideTextObject = null;
    public void UnitOverGuide()
    {
        SoundManager.instance.PlayEffectSound_ByName("LackPurchaseGold");
        unitOverGuideTextObject.SetActive(true);
        StartCoroutine(Co_HideUnitOverText());
    }
    IEnumerator Co_HideUnitOverText()
    {
        yield return new WaitForSeconds(1.5f);
        unitOverGuideTextObject.SetActive(false);
    }

    //public string GetUnitKey(UnitColor _color, UnitClass _class) => unitDB.GetUnitKey(_color, _class);

    //public TeamSoldier[] GetCurrnetUnits(string _tag) => CurrentUnitManager.GetUnits(_tag);
    //public TeamSoldier[] GetCurrnetUnits(UnitColor _color) => CurrentUnitManager.GetUnits(_color);
    //public TeamSoldier[] GetCurrnetUnits(UnitClass _class) => CurrentUnitManager.GetUnits(_class);
    //public TeamSoldier[] GetCurrnetUnits(UnitColor _color, UnitClass _class) => CurrentUnitManager.GetUnits(_color, _class);


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

    public void UnitTranslate_To_EnterStroyMode()
    {
        for (int i = 0; i < CurrentUnitCount; i++)
        {
            Multi_TeamSoldier unit = _currentUnits[i].GetComponent<Multi_TeamSoldier>();
            if (unit.enterStoryWorld) unit.Unit_WorldChange();
        }
    }

    //public void ShowReinforceEffect(int colorNumber)
    //{
    //    for (int i = 0; i < unitArrays[colorNumber].unitArray.Length; i++)
    //    {
    //        TeamSoldier unit = unitArrays[colorNumber].unitArray[i].transform.GetChild(0).GetComponent<TeamSoldier>();
    //        unit.reinforceEffect.SetActive(true);
    //    }
    //}
}
