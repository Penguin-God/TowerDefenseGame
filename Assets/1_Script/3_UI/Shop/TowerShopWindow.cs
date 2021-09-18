using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerShopWindow : MonoBehaviour
{
    delegate void On_Sell();
    On_Sell Sell;

    private void Awake()
    {
        Sell = () => UnitManager.instance.ExpendMaxUnit(5);
    }

    public void SpendMoney(int price)
    {
        if(GameManager.instance.Gold >= price)
        {
            GameManager.instance.Gold -= price;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);
            Sell();
        }
    }
}
