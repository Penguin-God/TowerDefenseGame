using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SellMethodFactory
{
    public Action<IReadOnlyList<int>> GetSellMeghod(int type)
    {
        switch (type)
        {
            case 0: return SellUnit;
            case 1: return SellGold;
            case 2: return SellFood;
        }
        return null;
    }

    void SellUnit(IReadOnlyList<int> datas) => Multi_SpawnManagers.NormalUnit.Spawn(datas[0], datas[1]);
    void SellGold(IReadOnlyList<int> datas) => Multi_GameManager.instance.AddGold(datas[0]);
    void SellFood(IReadOnlyList<int> datas) => Multi_GameManager.instance.AddFood(datas[0]);
}