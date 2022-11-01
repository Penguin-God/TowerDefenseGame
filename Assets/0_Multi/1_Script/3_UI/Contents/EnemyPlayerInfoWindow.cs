using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyPlayerInfoWindow : Multi_UI_Popup
{
    [SerializeField] Text[] texts;

    protected override void Init()
    {
        base.Init();
        gameObject.SetActive(false);
    }

    public void UpdateCount()
    {
        for (int i = 0; i < texts.Length; i++)
            texts[i].text = Multi_UnitManager.Instance.EnemyPlayerUnitCountByClass[(UnitClass)System.Enum.ToObject(typeof(UnitClass), i)] + "";
    }
}
