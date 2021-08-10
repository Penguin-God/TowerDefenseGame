using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetUnitButton : MonoBehaviour
{
    public string unitName;
    Text txt_UnitCount;

    GameObject[] arr_CurrentUnits;
    string unitClass;
    private void Awake()
    {
        txt_UnitCount = GetComponentInChildren<Text>();
        unitClass = txt_UnitCount.text;
        txt_UnitCount.text = unitClass + " : 0";
        //arr_CurrentUnits = SoldiersTags.dic_CurrentUnits[unitName];
    }

    private void OnEnable()
    {
        StartCoroutine("Co_UnitCount");
    }

    private void OnDisable()
    {
        StopCoroutine("Co_UnitCount");
    }

    int count = 0;
    IEnumerator Co_UnitCount()
    {
        while (true)
        {
            count = GameObject.FindGameObjectsWithTag(unitName).Length;
            txt_UnitCount.text = unitClass + " : " + count;
            yield return new WaitForSeconds(0.4f);
        }
    }
}
