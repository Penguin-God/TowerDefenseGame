using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitArray
{
    public GameObject[] unitArray;
}

public class UnitManager : MonoBehaviour
{
    public static UnitManager instance;
    public int maxUnit;
    public UnitArray[] unitArrays; // red, blue, yellow, green, orange, violet 순 6개 배열
    
    public bool UnitOver
    {
        get
        {
            if(currentUnitList.Count >= maxUnit)
            {
                UnitOverGuide();
                return true;
            }

            return false;
        }
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("UnitManager 2개");
            Destroy(gameObject);
        }
        currentUnitList = new List<GameObject>();
        StartCoroutine(UnitListCheck_Coroutine());
    }

    public List<GameObject> currentUnitList;

    
    [SerializeField] GameObject unitOverGuideTextObject = null;
    public void UnitOverGuide()
    {
        SoundManager.instance.PlayEffectSound_ByName("LackPurchaseGold");
        unitOverGuideTextObject.SetActive(true);
        StartCoroutine(HideUnitOverGuide());
    }

    IEnumerator HideUnitOverGuide()
    {
        yield return new WaitForSeconds(1.5f);
        unitOverGuideTextObject.SetActive(false);
    }

    IEnumerator UnitListCheck_Coroutine() // 유닛 리스트 무한반복문
    {
        while (true)
        {
            for(int i = 0; i < currentUnitList.Count; i++)
            {
                if (currentUnitList[i] == null) currentUnitList.RemoveAt(i);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    //private void Update()
    //{
    //    SetUnitPresence();
    //}

    // 현재 있는 유닛만 뜨게하기
    // 유닛 담아논 배열들 있으니까 그 배열의 크기가 0보다 크면 관련 bool변수를 true로 하고 그 값은 UI의 SetActive값으로 넣어서 Update에서 돌리면 될 듯
    // UI마다 int변수
    // 현재 켜져있는 UI Object List를 만들고 UI마다 int변수를 줘서 int가 높을수록 뒤에가게 해서 int비교하면서 위치 정하면 될 듯 
    //bool redSowrdman;
    //bool blueSowrdman;
    //bool yellowSowrdman;
    //bool greenSowrdman;
    //bool orangeSowrdman;
    //bool violetSowrdman;

    //[SerializeField] SoldiersTags unitCounts;
    //[HideInInspector] public bool thereIs_RedUnit;
    //[HideInInspector] public bool thereIs_BlueUnit;
    //[HideInInspector] public bool thereIs_YellowUnit;
    //[HideInInspector] public bool thereIs_GreenUnit;
    //[HideInInspector] public bool thereIs_OrangeUnit;
    //[HideInInspector] public bool thereIs_VioletUnit;

    //void SetUnitPresence()
    //{
    //    if (unitCounts.RedSpearman.Length > 0) thereIs_RedUnit = true;
    //    if (unitCounts.BlueSpearman.Length > 0) thereIs_BlueUnit = true;
    //    if (unitCounts.YellowSwordman.Length > 0) thereIs_YellowUnit = true;
    //    if (unitCounts.GreenSwordman.Length > 0) thereIs_GreenUnit = true;
    //    if (unitCounts.OrangeSwordman.Length > 0) thereIs_OrangeUnit = true;
    //    if (unitCounts.VioletSwordman.Length > 0) thereIs_VioletUnit = true;
    //}



    [SerializeField]
    private GameObject[] tp_Effects;
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
        for(int i = 0; i < currentUnitList.Count; i++)
        {
            TeamSoldier unit = currentUnitList[i].GetComponent<TeamSoldier>();
            if (unit.enterStoryWorld) unit.Unit_WorldChange();
        }
    }

    public GameObject[] startUnitArray;
    public void ReSpawnStartUnit()
    {
        int random = Random.Range(0, startUnitArray.Length);

        GameObject startUnit = Instantiate(startUnitArray[random], 
            startUnitArray[random].transform.position, startUnitArray[random].transform.rotation);
        startUnit.SetActive(true);
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

    public void ShowReinforceEffect(int colorNumber)
    {
        for(int i = 0; i < unitArrays[colorNumber].unitArray.Length; i++)
        {
            TeamSoldier unit = unitArrays[colorNumber].unitArray[i].transform.GetChild(0).GetComponent<TeamSoldier>();
            unit.reinforceEffect.SetActive(true);
        }
    }
}
