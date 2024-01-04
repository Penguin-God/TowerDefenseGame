using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public struct SkillDrawData
{
    [SerializeField] UserSkillClass _skillClass;
    [SerializeField] int _min;
    [SerializeField] int _max;

    public SkillDrawInfo GetDrawInfo() => new(_skillClass, _min, _max);
}

[Serializable]
public class BoxProductData
{
    [SerializeField] SkillBoxType _boxType;
    [SerializeField] PlayerMoneyType _moneyType;
    [SerializeField] int _amount;
    [SerializeField] SkillDrawData[] _skillDatas;

    public SkillBoxType BoxType => _boxType;
    public MoneyData GetPriceData() => new(_moneyType, _amount);
    public IEnumerable<SkillDrawInfo> GetDrawInfos() => _skillDatas.Select(x => x.GetDrawInfo());
}


[Serializable]
public struct GoldProductData
{
    [SerializeField] int _goldAmount;
    [SerializeField] int _gemPrice;

    public int GetGoldAmount() => _goldAmount;
    public MoneyData GetPriceData() => new(PlayerMoneyType.Gem, _gemPrice);
}

[Serializable]
public struct IAP_ProductData
{
    [SerializeField] int _gemAmount;
    [SerializeField] string _productId;
    [SerializeField] string _krw;

    public int GemAmount => _gemAmount;
    public string ProductId => _productId;
    public string KRW => _krw;
}