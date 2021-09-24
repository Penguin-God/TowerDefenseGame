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

    public void SpendFood(int price)
    {
        if(GameManager.instance.Food >= price)
        {
            GameManager.instance.Food -= price;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);
            Sell();
        }
    }
}
