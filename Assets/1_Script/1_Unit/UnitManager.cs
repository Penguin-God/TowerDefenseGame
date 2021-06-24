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
    public UnitArray[] unitArrays; // red, blue, yellow, green, orange, violet, black 순 7개 배열

    public AudioSource unitAudioManagerSource;


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
        //Debug.Log(unitArrays[0].unitArray[0]);
        currentUnitList = new List<GameObject>();
        StartCoroutine(UnitListCheck_Coroutine());

        unitAudioManagerSource = GetComponent<AudioSource>();
    }

    public List<GameObject> currentUnitList;

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

        GameObject startUnit = Instantiate(startUnitArray[random], startUnitArray[random].transform.position, startUnitArray[random].transform.rotation);
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

        Debug.Log(range_X);

        Vector3 respawnPosition = standardPosition + rangeVector;
        Debug.Log(rangeVector);
        return respawnPosition;
    }
}
