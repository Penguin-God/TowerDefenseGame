using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetUnitButton : MonoBehaviour
{
    [SerializeField] string unitName;
    Text txt_UnitCount;

    GameObject[] arr_CurrentUnits;
    string unitClass;
    private void Awake()
    {
        txt_UnitCount = GetComponentInChildren<Text>();
        unitClass = txt_UnitCount.text;
        txt_UnitCount.text = unitClass + " : 0"; 
        arr_CurrentUnits = SoldiersTags.dic_CurrentUnits[unitName];
    }

    private void Update()
    {
        arr_CurrentUnits = SoldiersTags.dic_CurrentUnits[unitName];
        txt_UnitCount.text = unitClass + " : " + arr_CurrentUnits.Length;
    }
}
