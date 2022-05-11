using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multi_UnitManager : MonoBehaviour
{
    public static Multi_UnitManager instance;
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

        unitDB = GetComponent<Multi_UnitDataBase>();
    }

    [SerializeField] Multi_SoldierSpawner soldierSpawner; 
    #region unit prefabs key
    public string UnitGroupName => "Unit";
    public string SwordmanPath => "Unit/Swordman";
    public string ArcherPath => "Unit/Archer";
    public string SpearmanPath => "Unit/Spearman";
    public string MagePath => "Unit/Mage";

    Dictionary<UnitClass, string> pathByUnitClass = new Dictionary<UnitClass, string>();

    public string BuildPath(UnitClass unitClass, string unitName) => BuildPath(pathByUnitClass[unitClass], unitName);
    string BuildPath(string path, string name) => $"{path}/{name}";
    #endregion


    void Start()
    {
        pathByUnitClass.Add(UnitClass.sowrdman, SwordmanPath);
        pathByUnitClass.Add(UnitClass.archer, ArcherPath);
        pathByUnitClass.Add(UnitClass.spearman, SpearmanPath);
        pathByUnitClass.Add(UnitClass.mage, MagePath);

        //CreateUnitPools(soldierSpawner.Swordmans, SwordmanPath, 5);
        //CreateUnitPools(soldierSpawner.Archers, ArcherPath, 5);
        //CreateUnitPools(soldierSpawner.Spearmans, SpearmanPath, 4);
        //CreateUnitPools(soldierSpawner.Mages, MagePath, 2);
    }

    //void CreateUnitPools(GameObject[] units, string unitPath, int count)
    //{
    //    for (int i = 0; i < units.Length; i++)
    //        Multi_Managers.Pool.CreatePool(units[i], BuildPath(unitPath, units[i].name), count, UnitGroupName);
    //}

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

    public Multi_TeamSoldier[] CurrentAllUnits => Multi_SoldierPoolingManager.Instance.AllUnits;
    //public CurrentUnitManager CurrentUnitManager { get; private set; } = null;

    //public void AddCurrentUnit(TeamSoldier _unit)
    //{
    //    CurrentUnitManager.AddUnit(_unit);
    //    UIManager.instance.UpdateCurrentUnitText(CurrentAllUnits.Length, maxUnit);
    //}

    //public void RemvoeCurrentUnit(TeamSoldier _unit)
    //{
    //    CurrentUnitManager.RemoveUnit(_unit);
    //    UIManager.instance.UpdateCurrentUnitText(CurrentAllUnits.Length, maxUnit);
    //}

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
        for (int i = 0; i < CurrentAllUnits.Length; i++)
        {
            Multi_TeamSoldier unit = CurrentAllUnits[i].GetComponent<Multi_TeamSoldier>();
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
