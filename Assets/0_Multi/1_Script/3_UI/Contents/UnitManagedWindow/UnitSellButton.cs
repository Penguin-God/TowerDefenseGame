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
        if (Multi_UnitManager.Instance.HasUnit(unitFlag))
        {
            Multi_UnitManager.Instance.UnitDead_RPC(Multi_Data.instance.Id, unitFlag);
            Multi_GameManager.instance.AddGold(GetSellReward(unitFlag));
        }
    }

    int GetSellReward(UnitFlags flag) => Multi_GameManager.instance.BattleData.UnitSellPriceRecord.PriceDatas[flag.ClassNumber].Price;

    public void SetInfo(UnitFlags flag)
    {
        unitFlag = flag;
    }
}
