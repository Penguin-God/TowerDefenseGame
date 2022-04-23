using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitColor { red, blue, yellow, green, orange, violet, white, black };
public enum UnitClass { sowrdman, archer, spearman, mage }

[System.Serializable]
public class CurrentUnitManager
{
    private Dictionary<string, List<TeamSoldier>> UnitDictionary = new Dictionary<string, List<TeamSoldier>>();
    private Dictionary<UnitColor, List<TeamSoldier>> UnitColorDictionary = new Dictionary<UnitColor, List<TeamSoldier>>();
    private Dictionary<UnitClass, List<TeamSoldier>> UnitClassDictionary = new Dictionary<UnitClass, List<TeamSoldier>>();
    private Dictionary<KeyValuePair<UnitColor, UnitClass>, List<TeamSoldier>> UnitPairDictionary = new Dictionary<KeyValuePair<UnitColor, UnitClass>, List<TeamSoldier>>();
    private List<TeamSoldier> AllUnit = new List<TeamSoldier>();

    public CurrentUnitManager(string[] _unitTags)
    {
        SettingUnitDictionary(_unitTags);
        SettingColorDictionary();
        SettingClassDictionary();
        SettingPairDictionary();
    }

    void SettingUnitDictionary(string[] _unitTags)
    {
        UnitDictionary = new Dictionary<string, List<TeamSoldier>>();
        for (int i = 0; i < _unitTags.Length; i++) UnitDictionary.Add(_unitTags[i], new List<TeamSoldier>());
    }

    void SettingColorDictionary()
    {
        foreach (UnitColor _color in System.Enum.GetValues(typeof(UnitColor)))
            UnitColorDictionary.Add(_color, new List<TeamSoldier>());
    }

    void SettingClassDictionary()
    {
        foreach (UnitClass _class in System.Enum.GetValues(typeof(UnitClass)))
            UnitClassDictionary.Add(_class, new List<TeamSoldier>());
    }

    void SettingPairDictionary()
    {
        foreach (UnitColor _color in System.Enum.GetValues(typeof(UnitColor)))
        {
            foreach (UnitClass _class in System.Enum.GetValues(typeof(UnitClass)))
            {
                KeyValuePair<UnitColor, UnitClass> _pair = new KeyValuePair<UnitColor, UnitClass>(_color, _class);
                UnitPairDictionary.Add(_pair, new List<TeamSoldier>());
            }
        }
    }

    public void AddUnit(TeamSoldier _unit)
    {
        UnitDictionary[_unit.gameObject.tag].Add(_unit);
        UnitColorDictionary[_unit.unitColor].Add(_unit);
        UnitClassDictionary[_unit.unitClass].Add(_unit);
        UnitPairDictionary[new KeyValuePair<UnitColor, UnitClass>(_unit.unitColor, _unit.unitClass)].Add(_unit);
        AllUnit.Add(_unit);
    }

    public void RemoveUnit(TeamSoldier _unit)
    {
        UnitDictionary[_unit.gameObject.tag].Remove(_unit);
        UnitColorDictionary[_unit.unitColor].Remove(_unit);
        UnitClassDictionary[_unit.unitClass].Remove(_unit);
        UnitPairDictionary[new KeyValuePair<UnitColor, UnitClass>(_unit.unitColor, _unit.unitClass)].Remove(_unit);
        AllUnit.Remove(_unit);
    }

    public TeamSoldier[] GetUnits(string _tag) => UnitDictionary[_tag].ToArray();

    public TeamSoldier[] GetUnits(string _tag, out int _count)
    {
        TeamSoldier[] _units = UnitDictionary[_tag].ToArray();
        _count = _units.Length;
        return _units;
    }

    public TeamSoldier[] GetUnits(UnitColor _color) => UnitColorDictionary[_color].ToArray();
    public TeamSoldier[] GetUnits(UnitColor _color, out int _count)
    {
        TeamSoldier[] _units = UnitColorDictionary[_color].ToArray();
        _count = _units.Length;
        return _units;
    }

    public TeamSoldier[] GetUnits(UnitClass _class) => UnitClassDictionary[_class].ToArray();

    public TeamSoldier[] GetUnits(UnitColor _color, UnitClass _class)
    {
        TeamSoldier[] _units = UnitPairDictionary[new KeyValuePair<UnitColor, UnitClass>(_color, _class)].ToArray();
        return _units;
    }

    public TeamSoldier[] GetAllUnit() =>AllUnit.ToArray();
}


public class UnitManager : MonoBehaviour
{
    public static UnitManager instance;
    [SerializeField] TeamSoldier[] debugCurrentAllUnit = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("UnitManager 2개");
            Destroy(gameObject);
        }

        unitDB = GetComponent<UnitDataBase>();
        CurrentUnitManager = new CurrentUnitManager(unitDB.unitTags);

        PlusMaxUnit = PlayerPrefs.GetInt("PlusMaxUnit");
        maxUnit +=  PlusMaxUnit;

        //StartCoroutine(UnitListCheck_Coroutine());
    }

    private void Update()
    {
        debugCurrentAllUnit = CurrentAllUnits;
    }

    public GameObject[] startUnitArray;
    public void ReSpawnStartUnit()
    {
        int random = Random.Range(0, startUnitArray.Length);

        GameObject startUnit = Instantiate(startUnitArray[random], 
            startUnitArray[random].transform.position, startUnitArray[random].transform.rotation);
        startUnit.SetActive(true);
    }


    int PlusMaxUnit;
    [SerializeField] int maxUnit;
    public void ExpendMaxUnit(int addUnitCount) => maxUnit += addUnitCount;

    public bool UnitOver
    {
        get
        {
            if (CurrentAllUnits.Length >= maxUnit)
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


    UnitDataBase unitDB = null;
    public void ApplyUnitData(string _tag, TeamSoldier _team) => unitDB.ApplyUnitBaseData(_tag, _team);

    public void ApplyPassiveData(string _key, UnitPassive _passive, UnitColor _color) => unitDB.ApplyPassiveData(_key, _passive, _color);

    public CurrentUnitManager CurrentUnitManager { get; private set; } = null;
    public TeamSoldier[] CurrentAllUnits => CurrentUnitManager.GetAllUnit();

    public void AddCurrentUnit(TeamSoldier _unit)
    {
        CurrentUnitManager.AddUnit(_unit);
        UIManager.instance.UpdateCurrentUnitText(CurrentAllUnits.Length, maxUnit);
    }

    public void RemvoeCurrentUnit(TeamSoldier _unit)
    {
        CurrentUnitManager.RemoveUnit(_unit);
        UIManager.instance.UpdateCurrentUnitText(CurrentAllUnits.Length, maxUnit);
    }

    public string GetUnitKey(UnitColor _color, UnitClass _class) => unitDB.GetUnitKey(_color, _class);

    public TeamSoldier[] GetCurrnetUnits(string _tag) => CurrentUnitManager.GetUnits(_tag);
    public TeamSoldier[] GetCurrnetUnits(UnitColor _color) => CurrentUnitManager.GetUnits(_color);
    public TeamSoldier[] GetCurrnetUnits(UnitClass _class) => CurrentUnitManager.GetUnits(_class);
    public TeamSoldier[] GetCurrnetUnits(UnitColor _color, UnitClass _class) => CurrentUnitManager.GetUnits(_color, _class);


    //readonly WaitForSeconds ws = new WaitForSeconds(0.1f);
    //IEnumerator UnitListCheck_Coroutine() // 유닛 리스트 무한반복문
    //{
    //    while (true)
    //    {
    //        for(int i = 0; i < CurrentUnitList.Count; i++)
    //        {
    //            if (CurrentUnitList[i] == null) CurrentUnitList.RemoveAt(i);
    //        }
    //        // 유닛 카운트 갱신할 때 Text도 같이 갱신
    //        UIManager.instance.UpdateCurrentUnitText(CurrentUnitList.Count, maxUnit);
    //        yield return ws;
    //    }
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

    public void UnitTranslate_To_EnterStroyMode()
    {
        for(int i = 0; i < CurrentAllUnits.Length; i++)
        {
            TeamSoldier unit = CurrentAllUnits[i].GetComponent<TeamSoldier>();
            if (unit.enterStoryWorld) unit.Unit_WorldChange();
        }
    }



    public Transform rangeTransfrom;
    public Vector3 Set_StroyModePosition()
    {
        Vector3 standardPosition = rangeTransfrom.position;

        BoxCollider rangeCollider = rangeTransfrom.gameObject.GetComponent<BoxCollider>();
        float range_X = rangeCollider.GetComponent<BoxCollider>().bounds.size.x;
        float range_Z = rangeCollider.GetComponent<BoxCollider>().bounds.size.z;
        range_X = Random.Range(range_X / 2 * -1, range_X / 2);
        range_Z = Random.Range(range_Z / 2 * -1, range_Z / 2);

        Vector3 rangeVector= new Vector3(range_X, 0, range_Z);
        Vector3 respawnPosition = standardPosition + rangeVector;
        return respawnPosition;
    }


    //public void ShowReinforceEffect(int colorNumber)
    //{
    //    for (int i = 0; i < unitArrays[colorNumber].unitArray.Length; i++)
    //    {
    //        TeamSoldier unit = unitArrays[colorNumber].unitArray[i].transform.GetChild(0).GetComponent<TeamSoldier>();
    //        unit.reinforceEffect.SetActive(true);
    //    }
    //}

    public void UpdateTarget_CurrnetFieldUnit()
    {
        foreach (TeamSoldier unit in CurrentAllUnits)
        {
            if (unit == null) continue;

            if (!unit.enterStoryWorld) unit.UpdateTarget();
        }
    }

    public void UpdateTarget_CurrnetStroyWolrdUnit(Transform _newTarget)
    {
        foreach (TeamSoldier unit in CurrentAllUnits)
        {
            if (unit == null) continue;

            if (unit.enterStoryWorld) unit.SetChaseSetting(_newTarget.gameObject);
        }
    }
}
