using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum SellType
{
    None,
    Unit,
    Gold,
    Food,
}

public class SellMethodFactory
{
    public Action<IReadOnlyList<int>> GetSellMeghod(SellType type)
    {
        switch (type)
        {
            case SellType.Unit: return SellUnit;
            case SellType.Gold: return SellGold;
            case SellType.Food: return SellFood;
        }
        return null;
    }

    void SellUnit(IReadOnlyList<int> datas) => Multi_SpawnManagers.NormalUnit.Spawn(datas[0], datas[1]);
    void SellGold(IReadOnlyList<int> datas) => Multi_GameManager.instance.AddGold(datas[0]);
    void SellFood(IReadOnlyList<int> datas) => Multi_GameManager.instance.AddFood(datas[0]);
}