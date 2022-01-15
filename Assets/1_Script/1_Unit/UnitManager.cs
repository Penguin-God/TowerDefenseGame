using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitListArray
{
    private List<TeamSoldier>[,] allUnitArray = new List<TeamSoldier>[7, 4];

    //private List<TeamSoldier>[] redUnitListArray = new List<TeamSoldier>[4];
    //private List<TeamSoldier>[] blueUnitListArray = new List<TeamSoldier>[4];
    //private List<TeamSoldier>[] yellowUnitListArray = new List<TeamSoldier>[4];
    //private List<TeamSoldier>[] greenUnitListArray = new List<TeamSoldier>[4];
    //private List<TeamSoldier>[] orangeUnitListArray = new List<TeamSoldier>[4];
    //private List<TeamSoldier>[] violetUnitListArray = new List<TeamSoldier>[4];
    //private List<TeamSoldier>[] blackUnitListArray = new List<TeamSoldier>[4];

    public void ResetList()
    {
        for (int i = 0; i < allUnitArray.GetLength(0); i++)
        {
            for (int j = 0; j < allUnitArray.GetLength(1); j++)
                allUnitArray[i, j] = new List<TeamSoldier>();
        }
    }

    public TeamSoldier[] GetCurrentUnits(int _colorNum)
    {
        List<TeamSoldier> _currnetUnits = new List<TeamSoldier>();
        for(int i = 0; i < allUnitArray.GetLength(1); i++)
        {
            for (int j = 0; j < allUnitArray[_colorNum, i].Count; j++)
                _currnetUnits.Add(allUnitArray[_colorNum, i][j]);
        }

        return _currnetUnits.ToArray();
    }

    public void AddUnit(int _colorNum, int classNum,TeamSoldier _unit) => allUnitArray[_colorNum, classNum].Add(_unit);

    public void RemoveUnit(int _colorNum, int classNum,TeamSoldier _unit) => allUnitArray[_colorNum, classNum].Remove(_unit);
}



public class UnitManager : MonoBehaviour
{
    public static UnitManager instance;

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

        for (int i = 0; i < CurrentUnitListArray.Length; i++) CurrentUnitListArray[i] = new List<TeamSoldier>();

        PlusMaxUnit = PlayerPrefs.GetInt("PlusMaxUnit");
        maxUnit +=  PlusMaxUnit;
        unitDB = GetComponent<UnitDataBase>();
        CurrentUnitList = new List<GameObject>();

        StartCoroutine(UnitListCheck_Coroutine());
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
            if (CurrentUnitList.Count >= maxUnit)
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

    public void ApplyPassiveData(string _key, TeamSoldier _unit) => unitDB.ApplyPassiveData(_key, _unit);


    // 임시. 나중에 커스텀 클래스 Array로 바꿔서 사용할 거임
    //UnitListArray unitListArray = new UnitListArray();

    public List<TeamSoldier>[] CurrentUnitListArray = new List<TeamSoldier>[7];
    public TeamSoldier[] GetItems(int _colorNum)
    {
        return CurrentUnitListArray[_colorNum].ToArray();
    }

    public void AddUnit(int _colorNum, TeamSoldier _unit) => CurrentUnitListArray[_colorNum].Add(_unit);

    public void RemoveItem(int _colorNum, TeamSoldier _unit) => CurrentUnitListArray[_colorNum].Remove(_unit);


    public List<GameObject> CurrentUnitList { get; private set; }
    readonly WaitForSeconds ws = new WaitForSeconds(0.1f);
    IEnumerator UnitListCheck_Coroutine() // 유닛 리스트 무한반복문
    {
        while (true)
        {
            for(int i = 0; i < CurrentUnitList.Count; i++)
            {
                if (CurrentUnitList[i] == null) CurrentUnitList.RemoveAt(i);
            }
            // 유닛 카운트 갱신할 때 Text도 같이 갱신
            UIManager.instance.UpdateCurrentUnitText(CurrentUnitList.Count, maxUnit);
            yield return ws;
        }
    }


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
        for(int i = 0; i < CurrentUnitList.Count; i++)
        {
            TeamSoldier unit = CurrentUnitList[i].GetComponent<TeamSoldier>();
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
        foreach (GameObject unit in CurrentUnitList)
        {
            if (unit == null) continue;

            TeamSoldier teamSoldier = unit.GetComponent<TeamSoldier>();
            if (!teamSoldier.enterStoryWorld) teamSoldier.UpdateTarget();
        }
    }
}
