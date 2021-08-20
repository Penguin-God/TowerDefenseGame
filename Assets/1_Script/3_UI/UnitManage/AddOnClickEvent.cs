using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddOnClickEvent : MonoBehaviour
{
    UnitManageWindowDictionary unitWindowDic;
    private void Start()
    {
        unitWindowDic = GetComponentInParent<UnitManageWindowDictionary>();
    }

    private void OnDisable()
    {
        if (unitWindowDic.currentShowWindow != null) unitWindowDic.currentShowWindow.SetActive(false);
    }
}
