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
    }

    public List<GameObject> currentUnitList;

    IEnumerator UnitListCheck_Coroutine()
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
}
