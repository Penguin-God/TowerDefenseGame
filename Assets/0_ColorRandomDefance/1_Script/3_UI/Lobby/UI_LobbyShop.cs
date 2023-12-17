using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum SkillBoxType
{
    희귀상자,
    고급상자,
    전설상자,
}

public class UI_LobbyShop : UI_Popup
{
    enum GameObjects
    {
        BoxGoodsParnet,
    }

    protected override void Init()
    {
        base.Init();
        Bind<GameObject>(typeof(GameObjects));
        CreateBoxGoods();
    }
    PlayerDataManager _playerDataManager;
    SkillDrawer _skillDrawer;
    public void DependencyInject(PlayerDataManager playerDataManager, SkillDrawer skillDrawer)
    {
        _playerDataManager = playerDataManager;
        _skillDrawer = skillDrawer;
    }

    IEnumerable<Vector2Int> moneyDatas = new Vector2Int[]
    {
        new Vector2Int(1000, 100),
        new Vector2Int(2000, 200),
        new Vector2Int(3000, 300),
    };

    void CreateBoxGoods()
    {
        foreach (Transform child in GetObject((int)GameObjects.BoxGoodsParnet).transform)
            Destroy(child.gameObject);

        foreach (SkillBoxType item in Enum.GetValues(typeof(SkillBoxType)))
            MakeGoods(new BoxPurchaseOperator(_skillDrawer), new MoneyData(PlayerMoneyType.Gem, 1000), Enum.GetName(typeof(SkillBoxType), item));

        foreach (var moneyData in moneyDatas)
            MakeGoods(new GoldPurchaseOperator(moneyData.x), new MoneyData(PlayerMoneyType.Gem, moneyData.y), new MoneyPersenter().GetMoneyText(new MoneyData(PlayerMoneyType.Gold, moneyData.x)));
    }

    void MakeGoods(IPurchaseOperator purchase, MoneyData moneyData, string productName) 
        => Managers.UI.MakeSubItem<UI_SkillBoxGoods>(GetObject((int)GameObjects.BoxGoodsParnet).transform).DependencyInject(new PurchaseManager(purchase, _playerDataManager), moneyData, productName);
}

public class MoneyPersenter
{
    public string GetMoneyText(MoneyData price)
    {
        switch (price.MoneyType)
        {
            case PlayerMoneyType.Gold: return $"골드 {price.Amount}원";
            case PlayerMoneyType.Gem: return $"젬 {price.Amount}개";
            default: return "";
        }
    }
}
