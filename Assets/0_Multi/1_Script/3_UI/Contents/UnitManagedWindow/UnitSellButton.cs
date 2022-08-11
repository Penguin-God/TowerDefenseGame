using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSellButton : Multi_UI_Base
{
    [SerializeField] UnitFlags unitFlag;

    protected override void Init()
    {
        GetComponent<Button>().onClick.AddListener(SellUnit);
    }

    void OnDestroy()
    {
        GetComponent<Button>().onClick.RemoveAllListeners();
    }

    void SellUnit()
    {
        //Multi_UnitManager.Instance.
    }

    public void SetInfo(UnitFlags flag)
    {
        unitFlag = flag;
    }
}
