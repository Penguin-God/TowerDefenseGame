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


    Dictionary<int, Dictionary<UnitFlags, List<Multi_TeamSoldier>>> unitListDictById = new Dictionary<int, Dictionary<UnitFlags, List<Multi_TeamSoldier>>>();
    List<Multi_TeamSoldier> GetUnitList(Multi_TeamSoldier unit) => GetUnitList(unit.GetComponent<RPCable>().UsingId, unit.UnitFlags);
    List<Multi_TeamSoldier> GetUnitList(int id, UnitFlags flags) => unitListDictById[id][flags];
    int GetUnitListCount(int id, UnitFlags flags) => GetUnitList(id, flags).Count;

    public event Action<UnitFlags, int> OnUnitFlagDictChanged = null;
    public void Raise_OnUnitFlagDictChanged_RPC(int id, UnitFlags flag)
        => photonView.RPC("Raise_OnUnitFlagDictChanged", RpcTarget.MasterClient, id, flag.ColorNumber, flag.ClassNumber);

    public void Raise_OnUnitFlagDictChanged_RPC(Multi_TeamSoldier unit)
    {
        Raise_OnUnitFlagDictChanged_RPC(unit.GetComponent<RPCable>().UsingId, unit.UnitFlags);
        print($"{unit.name} : {GetUnitList(unit).Count}마리 있음");
    }
    [PunRPC]
    void Raise_OnUnitFlagDictChanged(int id, int colorNum, int classNum) 
        => photonView.RPC("Raise_OnUnitFlagDictChanged", RpcTarget.All, id, colorNum, classNum, GetUnitListCount(id, new UnitFlags(colorNum, classNum)));
    [PunRPC]
    void Raise_OnUnitFlagDictChanged(int id, int colorNum, int classNum, int count)
    {
        if (Multi_Data.instance.CheckIdSame(id) == false) return;
        OnUnitFlagDictChanged?.Invoke(new UnitFlags(colorNum, classNum), count);
    }


    public event Action<bool, UnitFlags> OnTryCombine = null;
    void Raise_OnTryCombine_RPC(int id, bool isSuccess, UnitFlags flag) 
        => photonView.RPC("Raise_OnTryCombine", RpcTarget.All, id, isSuccess, flag.ColorNumber, flag.ClassNumber);
    [PunRPC]
    void Raise_OnTryCombine(int id, bool isSuccess, int colorNum, int classNum)
    {
        if (Multi_Data.instance.CheckIdSame(id) == false) return;
        OnTryCombine?.Invoke(isSuccess, new UnitFlags(colorNum, classNum));
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
        //unitDB = GetComponent<Multi_UnitDataBase>();
    }

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Multi_SpawnManagers.NormalUnit.OnSpawn += AddUnit;
            Multi_SpawnManagers.NormalUnit.OnDead += RemoveUnit;

            SetUnitFlagsDic();

            OnTryCombine += (isSuccess, flag) => print($"컴바인 시도 결과 : {isSuccess} \n 색깔 : {flag.ColorNumber}, 클래스 : {flag.ClassNumber}");
        }
        OnCurrentUnitChanged += count => _unitCount = count;

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

    Dictionary<int, List<Multi_TeamSoldier>> _currentUnitsById = new Dictionary<int, List<Multi_TeamSoldier>>();
    List<Multi_TeamSoldier> GetCurrentUnitList(Multi_TeamSoldier unit) => _currentUnitsById[unit.GetComponent<RPCable>().UsingId];

    public event Action<int> OnCurrentUnitChanged = null;
    [SerializeField] int _unitCount;
    public int UnitCount => _unitCount;
    void Raise_OnCurrentUnitChanged_RPC(Multi_TeamSoldier unit)
    {
        int id = unit.GetComponent<RPCable>().UsingId;
        photonView.RPC("Raise_OnCurrentUnitChanged", RpcTarget.All, id, _currentUnitsById[id].Count);
    }

    [PunRPC]
    void Raise_OnCurrentUnitChanged(int id, int count)
    {
        if (Multi_Data.instance.CheckIdSame(id) == false) return;
        OnCurrentUnitChanged?.Invoke(count);
    }

    void AddUnit(Multi_TeamSoldier unit)
    {
        GetUnitList(unit).Add(unit);
        GetCurrentUnitList(unit).Add(unit);

        Raise_OnUnitFlagDictChanged_RPC(unit);
        Raise_OnCurrentUnitChanged_RPC(unit);
    }

    void RemoveUnit(Multi_TeamSoldier unit)
    {
        GetUnitList(unit).Remove(unit);
        GetCurrentUnitList(unit).Remove(unit);

        Raise_OnUnitFlagDictChanged_RPC(unit);
        Raise_OnCurrentUnitChanged_RPC(unit);
    }

    public void Combine_RPC(CombineData data)
        => photonView.RPC("Combine", RpcTarget.MasterClient, data.UnitFlags.ColorNumber, data.UnitFlags.ClassNumber, Multi_Data.instance.Id);

    [PunRPC]
    void Combine(int colorNumber, int classNumber, int id)
    {
        if (CheckCombineable(Multi_Managers.Data.CombineConditionByUnitFalg[new UnitFlags(colorNumber, classNumber)], id))
        {
            SacrificedUnit_ForCombine(Multi_Managers.Data.CombineConditionByUnitFalg[new UnitFlags(colorNumber, classNumber)], id);
            Multi_SpawnManagers.NormalUnit.Spawn(new UnitFlags(colorNumber, classNumber), id);
            Raise_OnTryCombine_RPC(id, true, new UnitFlags(colorNumber, classNumber));
        }
        else
        {
            Raise_OnTryCombine_RPC(id, false, new UnitFlags(colorNumber, classNumber));
        }
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




    // 아래는 쭉 리팩터링 전 코드들

    // Multi_UnitDataBase unitDB = null;
    //public void ApplyUnitData(string _tag, Multi_TeamSoldier _team) => unitDB.ApplyUnitBaseData(_tag, _team);
    // public void ApplyPassiveData(string _key, Multi_UnitPassive _passive, UnitColor _color) => unitDB.ApplyPassiveData(_key, _passive, _color);

    //public GameObject[] startUnitArray;
    //public void ReSpawnStartUnit()
    //{
    //    int random = Random.Range(0, startUnitArray.Length);

    //    GameObject startUnit = Instantiate(startUnitArray[random],
    //        startUnitArray[random].transform.position, startUnitArray[random].transform.rotation);
    //    startUnit.SetActive(true);
    //}

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
